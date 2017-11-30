using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorDrive", "LargeBlockRepulsorDrive" })]
    public class RepulsorDriveComponent : LogicComponent
    {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorDrive", "LargeBlockRepulsorDrive" };

        public RepulsorLinearGenerator repulsorLinearGenerator;
        public RepulsorAngularGenerator repulsorAngularGenerator;

        protected override void Initialize()
        {

            base.Initialize();

            repulsorLinearGenerator = new RepulsorLinearGenerator(this);
            AddEquipment(repulsorLinearGenerator);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this);
            AddEquipment(repulsorAngularGenerator);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            RemoveEquipment(repulsorAngularGenerator);
            repulsorAngularGenerator = null;
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Generator Component");
            base.AppendCustomInfo(block, customInfo);
        }

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            IMyCubeGrid grid = block.CubeGrid;


            Vector3D linearAcceleration = Vector3D.Zero;
            Vector3D forcePoint = Vector3D.Zero;

            Vector3D angularAcceleration = Vector3D.Zero;

            // generator

            linearAcceleration = repulsorLinearGenerator.GetLinearAcceleration(physicsDeltaTime);


            angularAcceleration = repulsorAngularGenerator.GetAngularAcceleration(physicsDeltaTime);


            // apply

            rigidbody.AddLinearAcceleration(linearAcceleration);
            rigidbody.AddAngularAcceleration(angularAcceleration);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_drive_working_start", "repulsor_drive_working_loop", "repulsor_drive_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("952f402f-12f4-4828-892e-1918f015629e");
        }
    }

}
