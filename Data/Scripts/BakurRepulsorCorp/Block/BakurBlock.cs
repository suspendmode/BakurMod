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

    #region rigidbody wrapper
    public class BakurRigidbodyWrapper
    {

        BakurBlock bakurBlock;

        public BakurRigidbodyWrapper(BakurBlock bakurBlock)
        {
            this.bakurBlock = bakurBlock;
        }

        public IMyCubeGrid grid
        {
            get
            {
                return bakurBlock.block.CubeGrid;
            }
        }
        #region gravity

        public double gridMass
        {
            get
            {
                return bakurBlock.block.CubeGrid.Physics.Mass;
            }
        }

        public Vector3D gravity
        {
            get
            {
                return bakurBlock.block.CubeGrid.Physics.Gravity;
            }
        }

        public Vector3D gravityUp
        {
            get
            {
                Vector3D up = -gravity;
                up.Normalize();
                return up;
            }
        }

        public Vector3D gravityDown
        {
            get
            {
                Vector3D up = gravity;
                up.Normalize();
                return up;
            }
        }

        public bool IsInGravity
        {
            get
            {
                return gravity.LengthSquared() > MyMathConstants.EPSILON_SQUARED;
            }
        }

        #endregion

        #region force

        public void AddLinearVelocity(Vector3D velocity)
        {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddLinearAcceleration(acceleration);
        }

        public void AddLinearVelocity(Vector3D velocity, Vector3D point)
        {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddLinearAcceleration(acceleration, point);
        }

        public void AddAngularVelocity(Vector3D velocity)
        {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddAngularAcceleration(acceleration);
        }

        public void AddLinearAcceleration(Vector3D acceleration)
        {

            double mass = grid.Physics.Mass;
            AddForce(acceleration * mass);
        }

        public void AddLinearAcceleration(Vector3D acceleration, Vector3D point)
        {

            double mass = grid.Physics.Mass;
            AddForce(acceleration * mass, point);
        }

        public void AddAngularAcceleration(Vector3D acceleration)
        {

            //Vector3D pseudoInertiaTensor = grid.Physics.Mass * grid.WorldAABB.Extents;

            double size = grid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5;

            Vector3D pseudoInertiaTensor = 0.5f * grid.Physics.Mass * grid.WorldAABB.Size.Length() * size * Vector3D.One;
            // MyAPIGateway.Utilities.ShowMessage("Torque", "Extents: " + Math.Round(tensor.X, 2) + "," + Math.Round(tensor.Y, 2) + "," + Math.Round(tensor.Z, 2) + ", " + Math.Round(tensor.Length(), 2));

            AddTorque(acceleration * BakurMathHelper.Deg2Rad * pseudoInertiaTensor);
        }

        public void AddForce(Vector3D force)
        {
            if (double.IsNaN(force.X) || double.IsNaN(force.Y) || double.IsNaN(force.Z) || force == Vector3.Zero)
            {
                return;
            }
            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localForce = Vector3D.Transform(force, invWorldRot);
            grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, localForce, grid.Physics.CenterOfMassWorld, null);
        }

        public void AddForce(Vector3D force, Vector3D position)
        {
            if (double.IsNaN(force.X) || double.IsNaN(force.Y) || double.IsNaN(force.Z) || force == Vector3.Zero)
            {
                return;
            }

            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localForce = Vector3D.Transform(force, invWorldRot);
            grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, localForce, position, null);
        }

        public void AddTorque(Vector3D torque)
        {
            if (double.IsNaN(torque.X) || double.IsNaN(torque.Y) || double.IsNaN(torque.Z) || torque == Vector3.Zero)
            {
                return;
            }

            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localTorque = Vector3D.Transform(torque, invWorldRot);

            // MyAPIGateway.Utilities.ShowMessage("Torque", "torque: " + Math.Round(localTorque.X, 2) + "," + Math.Round(localTorque.Y, 2) + "," + Math.Round(localTorque.Z, 2) + ", " + Math.Round(localTorque.Length(), 2));

            grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, grid.Physics.CenterOfMassWorld, localTorque);
        }

        #endregion

    }
    #endregion

    public abstract class BakurBlock : MyGameLogicComponent
    {

        public MyObjectBuilder_EntityBase objectBuilder = null;

        public IMyTerminalBlock block;
        public BakurBlockEquipment blockEquipment;
        public BlockSounds blockSound;
        public BakurRigidbodyWrapper rigidbody;

        #region component

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            base.Init(objectBuilder);

            //MyAPIGateway.Utilities.ShowMessage("BakurBlock", "Init");

            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;

            rigidbody = new BakurRigidbodyWrapper(this);
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

            blockEquipment = new BakurBlockEquipment(this);
            AddEquipment(blockEquipment);

            blockSound = new BlockSounds(this, soundIds[0], soundIds[1], soundIds[2]);
            AddEquipment(blockSound);

            block.IsWorkingChanged += OnIsWorkingChanged;
            blockEquipment.EnabledChangedEvent += OnEnabledChanged;
            block.AppendingCustomInfo += AppendCustomInfo;
        }

        protected virtual void Destroy()
        {

            SaveStorage();

            //MyLog.Default.WriteLine("Destroy: " + block.BlockDefinition.SubtypeId);                
            block.IsWorkingChanged -= OnIsWorkingChanged;
            block.AppendingCustomInfo -= AppendCustomInfo;
            blockEquipment.EnabledChangedEvent -= OnEnabledChanged;

            RemoveEquipment(blockEquipment);
            RemoveEquipment(blockSound);

            blockEquipment = null;
            blockSound = null;
        }

        public override void MarkForClose()
        {
            base.MarkForClose();
            if (initialized)
            {
                foreach (EquipmentBase equipment in equipments)
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
            blockSound.UpdateSound();
        }

        protected virtual void OnEnabledChanged()
        {
            UpdateVisual();
            blockSound.UpdateSound();
        }


        #region update        

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
                    EquipmentBase equipment = equipments[i];
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
                foreach (EquipmentBase equipment in equipments)
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
            blockSound.UpdateSound();
        }

        #endregion

        #region equipment

        protected List<EquipmentBase> equipments = new List<EquipmentBase>();

        public void AddEquipment(EquipmentBase equipment)
        {
            equipments.Add(equipment);
        }

        public void RemoveEquipment(EquipmentBase equipment)
        {
            equipments.Remove(equipment);
        }

        public TEquipment GetEquipment<TEquipment>() where TEquipment : EquipmentBase
        {
            foreach (EquipmentBase equipment in equipments)
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
            foreach (EquipmentBase equipment in equipments)
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
                if (blockEquipment != null)
                {
                    blockEquipment.enabled = value;
                }
            }
            get
            {
                return blockEquipment == null ? false : blockEquipment.enabled;
            }

        }

        #endregion

        #region debugEnabled

        public bool debugEnabled
        {
            set
            {
                if (blockEquipment != null)
                {
                    blockEquipment.debugEnabled = value;
                }
            }
            get
            {
                return blockEquipment == null ? false : blockEquipment.debugEnabled;
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
