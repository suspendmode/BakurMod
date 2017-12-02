using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_RemoteControl), true, new string[] { "SmallBlockRepulsorDriveRemoteControl", "LargeBlockRepulsorDriveRemoteControl" })]
    public class RepulsorDriveRemoteControlComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        private static readonly string[] subTypeIds = { "SmallBlockRepulsorDriveRemoteControl", "LargeBlockRepulsorDriveRemoteControl" };

        public RepulsorLinearGenerator repulsorLinearGenerator;
        public RepulsorLinearGeneratorUIController<IMyUpgradeModule> repulsorLinearGeneratorUI;

        public RepulsorAngularGenerator repulsorAngularGenerator;
        public RepulsorAngularGeneratorUIController<IMyUpgradeModule> repulsorAngularGeneratorUI;

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            repulsorLinearGenerator = new RepulsorLinearGenerator(this);
            AddElement(repulsorLinearGenerator);

            repulsorLinearGeneratorUI = new RepulsorLinearGeneratorUIController<IMyUpgradeModule>(this);
            AddElement(repulsorLinearGeneratorUI);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this);
            AddElement(repulsorAngularGenerator);

            repulsorAngularGeneratorUI = new RepulsorAngularGeneratorUIController<IMyUpgradeModule>(this);
            AddElement(repulsorAngularGeneratorUI);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            RemoveElement(repulsorLinearGeneratorUI);
            repulsorLinearGeneratorUI = null;

            RemoveElement(repulsorAngularGenerator);
            repulsorAngularGenerator = null;

            RemoveElement(repulsorAngularGeneratorUI);
            repulsorAngularGeneratorUI = null;
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Generator Component");
            base.AppendCustomInfo(block, customInfo);
        }

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
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
            return new Guid("3a12f73f-2c1d-4811-9a12-7a7e513b9403");
        }
    }

}
