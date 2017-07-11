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

namespace BakurRepulsorCorp {

    public abstract class BakurBlock : MyGameLogicComponent {

        public MyObjectBuilder_EntityBase objectBuilder = null;

        public IMyTerminalBlock block;
        public BakurBlockEquipment blockEquipment;
        public BlockSounds blockSound;

        #region component

        public override void Init(MyObjectBuilder_EntityBase objectBuilder) {
            base.Init(objectBuilder);
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
            Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
            objectBuilder = objectBuilder as MyObjectBuilder_EntityBase;
            block = Entity as IMyTerminalBlock;
            if (Entity.Storage == null) {
                Entity.Storage = new MyModStorageComponent();
            }

            //MyLog.Default.WriteLine("Init: " + block.BlockDefinition.SubtypeId);            
        }

        #endregion

        #region lifecycle

        public bool initialized = false;

        protected virtual void Initialize() {

            LoadStorage();

            blockEquipment = new BakurBlockEquipment(this);
            Add(blockEquipment);

            blockSound = new BlockSounds(this, soundIds[0], soundIds[1], soundIds[2]);
            Add(blockSound);

            block.IsWorkingChanged += OnIsWorkingChanged;
            blockEquipment.EnabledChangedEvent += OnEnabledChanged;
            block.AppendingCustomInfo += AppendCustomInfo;


        }

        protected virtual void Destroy() {

            SaveStorage();

            //MyLog.Default.WriteLine("Destroy: " + block.BlockDefinition.SubtypeId);                
            block.IsWorkingChanged -= OnIsWorkingChanged;
            block.AppendingCustomInfo -= AppendCustomInfo;
            blockEquipment.EnabledChangedEvent -= OnEnabledChanged;

            Remove(blockEquipment);
            Remove(blockSound);

            blockEquipment = null;
            blockSound = null;
        }

        public override void MarkForClose() {
            base.MarkForClose();
            if (initialized) {
                initialized = false;
                foreach (EquipmentBase equipment in equipments) {
                    equipment.Destroy();
                }
                Destroy();
            }
        }

        #endregion

        protected virtual void OnIsWorkingChanged(IMyCubeBlock block) {
            UpdateVisual();
            blockSound.UpdateSound();
        }

        protected virtual void OnEnabledChanged(bool state) {
            UpdateVisual();
            blockSound.UpdateSound();
        }

        #region equipment

        protected List<EquipmentBase> equipments = new List<EquipmentBase>();

        protected void Add(EquipmentBase equipment) {
            equipments.Add(equipment);
        }

        protected void Remove(EquipmentBase equipment) {
            equipments.Remove(equipment);
        }

        public TEquipment GetEquipment<TEquipment>() where TEquipment : EquipmentBase {
            foreach (EquipmentBase equipment in equipments) {
                if (equipment is TEquipment) {
                    return (TEquipment)equipment;
                }
            }
            return null;
        }

        protected void UpdateEquipmentsVisual(IMyTerminalBlock block, StringBuilder customInfo) {
            foreach (EquipmentBase equipment in equipments) {
                equipment.AppendCustomInfo(block, customInfo);
            }
        }

        #endregion

        #region update

        public override void UpdateBeforeSimulation() {

            if (!initialized || !enabled || block == null || block.MarkedForClose || block.Closed || !block.IsWorking || block.CubeGrid == null || block.CubeGrid.Physics == null || !block.CubeGrid.Physics.Enabled) {
                return;
            }

            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            double updateDeltaTime = MyEngineConstants.UPDATE_STEP_SIZE_IN_SECONDS;

            UpdateBeforeFrame(physicsDeltaTime, updateDeltaTime);
            if (debugEnabled) {
                foreach (EquipmentBase equipment in equipments) {
                    equipment.Debug();
                }
                Debug();
            }
        }

        protected virtual void Debug() {
        }

        protected abstract void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime);

        public override void UpdateBeforeSimulation10() {

            if (!initialized) {
                initialized = true;
                Initialize();
                for (int i = 0; i < equipments.Count; i++) {
                    EquipmentBase equipment = equipments[i];
                    equipment.Initialize();
                }

            }

            visualFrame++;

            if (visualFrame % 10 == 0) {
                UpdateVisual();
                blockSound.UpdateSound();
            }
        }

        int visualFrame;

        public override void UpdateBeforeSimulation100() {

            if (!initialized || block == null || block.MarkedForClose || block.Closed || !block.IsWorking || block.CubeGrid == null || block.CubeGrid.Physics == null) {
                return;
            }

            DrawEmissive();
        }

        #endregion

        #region visuals

        public virtual void UpdateVisual() {
            DrawEmissive();
            block.RefreshCustomInfo();
        }

        public virtual void DrawEmissive() {
            if (!block.IsWorking) {
                block.SetEmissiveParts("Emissive1", new Color(255, 0, 0), 1);
            } else {
                block.SetEmissiveParts("Emissive1", new Color(0, 255, 0), 1);
            }
        }

        protected virtual void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Component ==");
            customInfo.AppendLine("Is Working : " + block.IsWorking);
            customInfo.AppendLine("Marked For Close : " + MarkedForClose);
            customInfo.AppendLine("Is In Gravity : " + IsInGravity);
            customInfo.AppendLine("Gravity : " + gravity.Length());

            Vector3D linearVelocity = block.CubeGrid.Physics.LinearVelocity;
            Vector3D angularVelocity = block.CubeGrid.Physics.AngularVelocity * (float)BakurMathHelper.Rad2Deg;

            customInfo.AppendLine("Linear Velocity : " + Math.Round(linearVelocity.Length(), 1));
            customInfo.AppendLine("Angular Velocity : " + Math.Round(angularVelocity.Length(), 1));


            UpdateEquipmentsVisual(block, customInfo);
        }

        public string GeneratatePropertyId(string name) {
            return Entity.EntityId + "_" + name;
        }

        #endregion

        #region enabled 
        public bool enabled
        {
            set
            {
                if (blockEquipment != null) {
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
                if (blockEquipment != null) {
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
        /*
        public void SetVariable<T>(string id, T value) {
            storage[id] = value;
            //SaveStorage();
        }

        public bool GetVariable<T>(string id, out T result) {            
            //LoadStorage();
            if (!storage.Dictionary.ContainsKey(id)) {
                result = default(T);
                return false;
            }
            result = (T)storage[id];
            return true;
        }
        */

        void SaveStorage() {
            Guid guid = blockGUID();
            Entity.Storage[guid] = MyAPIGateway.Utilities.SerializeToXML<SerializableDictionary<string, object>>(storage);
        }

        void LoadStorage() {
            Guid guid = blockGUID();
            if (Entity.Storage.ContainsKey(guid)) {
                string buffer = Entity.Storage[guid];
                storage = MyAPIGateway.Utilities.SerializeFromXML<SerializableDictionary<string, object>>(buffer);
            }
        }

        protected abstract Guid blockGUID();


        public void SetVariable<T>(string id, T value) {
            MyAPIGateway.Utilities.SetVariable<T>(id, value);
        }
        public bool GetVariable<T>(string id, out T result) {
            return MyAPIGateway.Utilities.GetVariable<T>(id, out result);
        }


        /*
        static string BAKUR_PROPERTY_VALUES_FILE = "bakur-mod.xml";
        public void SetVariableToFile<T>(string id, T value) {
            string fileName = GetFileName();
            Dictionary<string, object> properties;

            if (MyAPIGateway.Utilities.FileExistsInGlobalStorage(BAKUR_PROPERTY_VALUES_FILE)) {
                BinaryReader reader = MyAPIGateway.Utilities.ReadBinaryFileInGlobalStorage(BAKUR_PROPERTY_VALUES_FILE);
                byte[] data = reader.ReadBytes((int)reader.BaseStream.Length);
                properties = MyAPIGateway.Utilities.SerializeFromBinary<Dictionary<string, object>>(data);
            } else {
                properties = new Dictionary<string, object>();
            }

            properties[id] = value;

            byte[] serialized = MyAPIGateway.Utilities.SerializeToBinary(properties);
            BinaryWriter writer = MyAPIGateway.Utilities.WriteBinaryFileInGlobalStorage(fileName);
            writer.Write(serialized);
            writer.Close();
        }
        */

        /*
        public bool GetVariableFromFile<T>(string id, out T result) {
            string fileName = GetFileName();
            Dictionary<string, object> properties;

            if (MyAPIGateway.Utilities.FileExistsInGlobalStorage(BAKUR_PROPERTY_VALUES_FILE)) {
                BinaryReader reader = MyAPIGateway.Utilities.ReadBinaryFileInGlobalStorage(BAKUR_PROPERTY_VALUES_FILE);
                byte[] data = reader.ReadBytes((int)reader.BaseStream.Length);
                properties = MyAPIGateway.Utilities.SerializeFromBinary<Dictionary<string, object>>(data);
                if (properties.ContainsKey(id)) {
                    result = (T)properties[id];
                    return true;
                } else {
                    result = default(T);
                    return false;
                }
            } else {
                result = default(T);
                return false;
            }
        }

        string GetFileName() {
            return block.BlockDefinition.SubtypeId + "_" + block.EntityId + "_properties.xml";
        }
        */

        #endregion

        #region gravity

        public double gridMass
        {
            get
            {
                return block.CubeGrid.Physics.Mass;
            }
        }

        public Vector3D gravity
        {
            get
            {
                return block.CubeGrid.Physics.Gravity;
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

        public void AddLinearVelocity(Vector3D velocity) {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddLinearAcceleration(acceleration);
        }

        public void AddLinearVelocity(Vector3D velocity, Vector3D point) {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddLinearAcceleration(acceleration, point);
        }

        public void AddAngularVelocity(Vector3D velocity) {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddLinearAcceleration(acceleration);
        }

        public void AddLinearAcceleration(Vector3D acceleration) {
            IMyCubeGrid grid = block.CubeGrid;
            double mass = grid.Physics.Mass;
            AddForce(acceleration * mass);
        }

        public void AddLinearAcceleration(Vector3D acceleration, Vector3D point) {
            IMyCubeGrid grid = block.CubeGrid;
            double mass = grid.Physics.Mass;
            AddForce(acceleration * mass, point);
        }

        public void AddAngularAcceleration(Vector3D acceleration) {
            IMyCubeGrid grid = block.CubeGrid;
            double mass = grid.Physics.Mass;
            AddTorque(acceleration * mass);
        }

        public void AddAngularAcceleration(Vector3D acceleration, Vector3D point) {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            IMyCubeGrid grid = block.CubeGrid;
            double mass = grid.Physics.Mass;
            AddForce(acceleration * mass, point);
        }

        public void AddForce(Vector3D force) {
            if (double.IsNaN(force.X) || double.IsNaN(force.Y) || double.IsNaN(force.Z) || force == Vector3.Zero) {
                return;
            }
            IMyCubeGrid grid = block.CubeGrid;
            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localForce = Vector3D.Transform(force, invWorldRot);
            block.CubeGrid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, localForce, grid.Physics.CenterOfMassWorld, null);
        }

        public void AddForce(Vector3D force, Vector3D position) {
            if (double.IsNaN(force.X) || double.IsNaN(force.Y) || double.IsNaN(force.Z) || force == Vector3.Zero) {
                return;
            }
            IMyCubeGrid grid = block.CubeGrid;
            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localForce = Vector3D.Transform(force, invWorldRot);
            block.CubeGrid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, localForce, position, null);
        }

        public void AddTorque(Vector3D torque) {
            if (double.IsNaN(torque.X) || double.IsNaN(torque.Y) || double.IsNaN(torque.Z) || torque == Vector3.Zero) {
                return;
            }
            IMyCubeGrid grid = block.CubeGrid;
            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localTorque = Vector3D.Transform(torque, invWorldRot);
            block.CubeGrid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, grid.Physics.CenterOfMassWorld, localTorque);
        }

        #endregion

        public void Assert(bool condition, string message) {
            if (!condition) {
                Exception e = new Exception(message);
                MyLog.Default.WriteLine(e);
                throw e;
            }
        }

        public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false) {
            return objectBuilder;
        }

        protected abstract string[] soundIds
        {
            get;
        }
    }
}
