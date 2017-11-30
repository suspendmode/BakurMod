using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace BakurRepulsorCorp
{

    public abstract class LogicComponent : MyGameLogicComponent
    {

        public MyObjectBuilder_EntityBase objectBuilder = null;

        public IMyTerminalBlock block;
        public DefaultLogicElement defaultLogicElement;
        public SoundController soundController;
        public Rigidbody rigidbody;

        #region component

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            base.Init(objectBuilder);

            //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "Init");

            NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
            NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            rigidbody = new Rigidbody(this);
            objectBuilder = objectBuilder as MyObjectBuilder_EntityBase;
            block = Entity as IMyTerminalBlock;
            if (Entity.Storage == null)
            {
                Entity.Storage = new MyModStorageComponent();
            }

            //MyLog.Default.WriteLine("Init: " + block.BlockDefinition.SubtypeId);            
        }

        #endregion

        #region lifecycle

        public bool initialized = false;

        protected virtual void Initialize()
        {
            //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "Initialize");

            LoadStorage();

            defaultLogicElement = new DefaultLogicElement(this);
            AddEquipment(defaultLogicElement);

            soundController = new SoundController(this, soundIds[0], soundIds[1], soundIds[2]);
            AddEquipment(soundController);

            block.IsWorkingChanged += OnIsWorkingChanged;
            defaultLogicElement.EnabledChangedEvent += OnEnabledChanged;
            block.AppendingCustomInfo += AppendCustomInfo;
        }

        protected virtual void Destroy()
        {

            SaveStorage();

            //MyLog.Default.WriteLine("Destroy: " + block.BlockDefinition.SubtypeId);                
            block.IsWorkingChanged -= OnIsWorkingChanged;
            block.AppendingCustomInfo -= AppendCustomInfo;
            defaultLogicElement.EnabledChangedEvent -= OnEnabledChanged;

            RemoveEquipment(defaultLogicElement);
            RemoveEquipment(soundController);

            defaultLogicElement = null;
            soundController = null;
        }

        public override void MarkForClose()
        {
            base.MarkForClose();
            if (initialized)
            {
                foreach (LogicElement equipment in equipments)
                {
                    equipment.Destroy();
                    equipment.isInitialized = false;
                }
                Destroy();
                initialized = false;
            }
        }

        #endregion

        protected virtual void OnIsWorkingChanged(IMyCubeBlock block)
        {
            UpdateVisual();
            soundController.UpdateSound();
        }

        protected virtual void OnEnabledChanged()
        {
            UpdateVisual();
            soundController.UpdateSound();
        }


        #region update        

        bool powered = false;

        public override void UpdateBeforeSimulation100()
        {
            powered = block != null && block.ResourceSink != null && block.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && block.IsWorking;

        }

        public override void UpdateAfterSimulation()
        {

            //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "UpdateAfterSimulation");

            //if (block == null || block.MarkedForClose || block.Closed || !block.IsWorking || block.CubeGrid == null || block.CubeGrid.Physics == null || !block.CubeGrid.Physics.Enabled) {
            if (block == null || block.MarkedForClose || block.Closed || block.CubeGrid == null)
            {
                //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "block == null || block.MarkedForClose || block.Closed || block.CubeGrid == null");
                return;
            }

            if (!initialized)
            {
                //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "!initialized");
                initialized = true;
                Initialize();
                for (int i = 0; i < equipments.Count; i++)
                {
                    LogicElement equipment = equipments[i];
                    equipment.Initialize();
                    equipment.isInitialized = true;
                }
                UpdateVisual();
            }

            if (!initialized || !enabled)
            {
                //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "!initialized || !enabled");
                return;
            }

            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;

            UpdateSimulation(physicsDeltaTime);

            if (debugEnabled)
            {
                foreach (LogicElement equipment in equipments)
                {
                    equipment.Debug();
                }
                Debug();
            }
        }

        #endregion

        #region simulation

        protected virtual void Debug()
        {
        }

        protected abstract void UpdateSimulation(double physicsDeltaTime);

        public override void UpdateAfterSimulation100()
        {

            if (!initialized)
            {
                return;
            }

            UpdateVisual();
            soundController.UpdateSound();
        }

        #endregion

        #region equipment

        protected List<LogicElement> equipments = new List<LogicElement>();

        public void AddEquipment(LogicElement equipment)
        {
            equipments.Add(equipment);
        }

        public void RemoveEquipment(LogicElement equipment)
        {
            equipments.Remove(equipment);
        }

        public TEquipment GetEquipment<TEquipment>() where TEquipment : LogicElement
        {
            foreach (LogicElement equipment in equipments)
            {
                if (equipment is TEquipment)
                {
                    return (TEquipment)equipment;
                }
            }
            return null;
        }

        protected void UpdateEquipmentsVisual(IMyTerminalBlock block, StringBuilder customInfo)
        {
            foreach (LogicElement equipment in equipments)
            {
                equipment.AppendCustomInfo(block, customInfo);
            }
        }

        #endregion

        #region visuals

        public virtual void UpdateVisual()
        {
            DrawEmissive();
            block.RefreshCustomInfo();
        }

        public virtual void DrawEmissive()
        {
            if (!block.IsWorking || block.CubeGrid.IsStatic || !enabled)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 0, 0), 1);
            }
            else
            {
                block.SetEmissiveParts("Emissive1", new Color(0, 255, 0), 1);
            }
        }

        protected virtual void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Component ==");
            customInfo.AppendLine("Is Working : " + block.IsWorking);
            customInfo.AppendLine("Marked For Close : " + MarkedForClose);
            customInfo.AppendLine("Is In Gravity : " + rigidbody.IsInGravity);
            customInfo.AppendLine("Gravity : " + rigidbody.gravity.Length());

            Vector3D linearVelocity = block.CubeGrid.Physics.LinearVelocity;
            Vector3D angularVelocity = block.CubeGrid.Physics.AngularVelocity * (float)BakurMathHelper.Rad2Deg;

            customInfo.AppendLine("Linear Velocity : " + Math.Round(linearVelocity.Length(), 1));
            customInfo.AppendLine("Angular Velocity : " + Math.Round(angularVelocity.Length(), 1));

            UpdateEquipmentsVisual(block, customInfo);
        }

        public string GeneratatePropertyId(string name)
        {
            return Entity.EntityId + "_" + name;
        }

        #endregion

        #region enabled 
        public bool enabled
        {
            set
            {
                if (defaultLogicElement != null)
                {
                    defaultLogicElement.enabled = value;
                }
            }
            get
            {
                return defaultLogicElement == null ? false : defaultLogicElement.enabled;
            }

        }

        #endregion

        #region debugEnabled

        public bool debugEnabled
        {
            set
            {
                if (defaultLogicElement != null)
                {
                    defaultLogicElement.debugEnabled = value;
                }
            }
            get
            {
                return defaultLogicElement == null ? false : defaultLogicElement.debugEnabled;
            }
        }

        #endregion

        #region storage        

        SerializableDictionary<string, object> storage = new SerializableDictionary<string, object>();

        void SaveStorage()
        {
            Guid guid = blockGUID();
            Entity.Storage[guid] = MyAPIGateway.Utilities.SerializeToXML<SerializableDictionary<string, object>>(storage);
        }

        void LoadStorage()
        {
            Guid guid = blockGUID();
            if (Entity.Storage.ContainsKey(guid))
            {
                string buffer = Entity.Storage[guid];
                storage = MyAPIGateway.Utilities.SerializeFromXML<SerializableDictionary<string, object>>(buffer);
            }
        }

        protected abstract Guid blockGUID();

        public void SetVariable<T>(string id, T value)
        {
            MyAPIGateway.Utilities.SetVariable<T>(id, value);
        }

        public bool GetVariable<T>(string id, out T result)
        {
            return MyAPIGateway.Utilities.GetVariable<T>(id, out result);
        }

        #endregion

        public void Assert(bool condition, string message)
        {
            if (!condition)
            {
                Exception e = new Exception(message);
                MyLog.Default.WriteLine(e);
                throw e;
            }
        }

        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            return objectBuilder;
        }

        protected abstract string[] soundIds
        {
            get;
        }
    }
}
