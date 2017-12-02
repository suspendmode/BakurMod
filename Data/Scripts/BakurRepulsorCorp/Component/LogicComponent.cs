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
        public IMyCubeGrid grid;

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
            grid = block.CubeGrid;
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
            AddElement(defaultLogicElement);

            soundController = new SoundController(this, soundIds[0], soundIds[1], soundIds[2]);
            AddElement(soundController);

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

            RemoveElement(defaultLogicElement);
            RemoveElement(soundController);

            defaultLogicElement = null;
            soundController = null;
        }

        public override void MarkForClose()
        {
            base.MarkForClose();
            if (initialized)
            {
                foreach (LogicElement element in elements)
                {
                    element.Destroy();
                    element.isInitialized = false;
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

        public override void UpdateAfterSimulation100()
        {
            powered = block != null && block.ResourceSink != null && block.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && block.IsWorking;

            if (!initialized || grid == null)
            {
                return;
            }

            UpdateVisual();
            soundController.UpdateSound();
        }

        public override void UpdateAfterSimulation()
        {

            //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "UpdateAfterSimulation");

            //if (block == null || block.MarkedForClose || block.Closed || !block.IsWorking || block.CubeGrid == null || block.CubeGrid.Physics == null || !block.CubeGrid.Physics.Enabled) {
            if (block == null || block.MarkedForClose || block.Closed || grid == null ||
                grid.Physics == null || !grid.Physics.Enabled)
            {
                //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "block == null || block.MarkedForClose || block.Closed || block.CubeGrid == null");
                return;
            }

            if (!initialized)
            {
                //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "!initialized");
                initialized = true;
                Initialize();
                for (int i = 0; i < elements.Count; i++)
                {
                    LogicElement element = elements[i];
                    element.Initialize();
                    element.isInitialized = true;
                }
                UpdateVisual();
            }

            if (!initialized || !enabled)
            {
                //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "!initialized || !enabled");
                return;
            }

            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;

            UpdateAfterSimulation(physicsDeltaTime);

            if (debugEnabled)
            {
                foreach (LogicElement element in elements)
                {
                    element.Debug();
                }
                Debug();
            }
        }

        #endregion

        #region simulation

        protected virtual void Debug()
        {
        }

        protected abstract void UpdateAfterSimulation(double physicsDeltaTime);

        #endregion

        #region element

        protected List<LogicElement> elements = new List<LogicElement>();

        public void AddElement(LogicElement element)
        {
            elements.Add(element);
        }

        public void RemoveElement(LogicElement element)
        {
            elements.Remove(element);
        }

        public TLogicElement GetElement<TLogicElement>() where TLogicElement : LogicElement
        {
            foreach (LogicElement element in elements)
            {
                if (element is TLogicElement)
                {
                    return (TLogicElement)element;
                }
            }
            return null;
        }

        protected void UpdateElementsVisual(IMyTerminalBlock block, StringBuilder customInfo)
        {
            foreach (LogicElement element in elements)
            {
                element.AppendCustomInfo(block, customInfo);
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
            if (block.CubeGrid.IsStatic)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 120, 0), 1);
            }
            else if (!block.IsWorking || !block.IsFunctional || !enabled)
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
            customInfo.AppendLine("Type: Component");
            customInfo.AppendLine("Is Working: " + block.IsWorking);
            customInfo.AppendLine("Is Functional: " + block.IsFunctional);
            customInfo.AppendLine("Marked For Close: " + MarkedForClose);

            UpdateElementsVisual(block, customInfo);
        }

        public string GeneratePropertyId(string name)
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
