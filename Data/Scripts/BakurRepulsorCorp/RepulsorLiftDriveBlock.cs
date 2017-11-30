using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorLiftDrive", "LargeBlockRepulsorLiftDrive" })]
    public class RepulsorLiftDriveBlock : BakurBlock
    {

        PlanetAltitudeSensor planetAltitudeSensor;
        RepulsorLift repulsorLift;
        RepulsorLinearGenerator repulsorLinearGenerator;
        RepulsorAngularGenerator repulsorAngularGenerator;

        protected override void Initialize()
        {

            base.Initialize();

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddEquipment(planetAltitudeSensor);

            repulsorLift = new RepulsorLift(this);
            AddEquipment(repulsorLift);

            repulsorLinearGenerator = new RepulsorLinearGenerator(this);
            AddEquipment(repulsorLinearGenerator);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this);
            AddEquipment(repulsorAngularGenerator);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(repulsorLift);
            repulsorLift = null;

            RemoveEquipment(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            RemoveEquipment(repulsorAngularGenerator);
            repulsorAngularGenerator = null;

            RemoveEquipment(planetAltitudeSensor);
            planetAltitudeSensor = null;
        }

        Vector3D linearAcceleration;
        Vector3D angularAcceleration;
        Vector3D liftAcceleration;

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);

            IMyCubeGrid grid = block.CubeGrid;
            Vector3D desiredUp = rigidbody.gravityUp;

            linearAcceleration = Vector3D.Zero;
            angularAcceleration = Vector3D.Zero;
            liftAcceleration = Vector3D.Zero;

            // generator

            linearAcceleration = repulsorLinearGenerator.GetLinearAcceleration(physicsDeltaTime);
            angularAcceleration = repulsorAngularGenerator.GetAngularAcceleration(physicsDeltaTime);

            // lift

            if (rigidbody.IsInGravity)
            {
                double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length()) / 2;
                liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);
            }

            // apply

            rigidbody.AddLinearAcceleration(liftAcceleration);
            rigidbody.AddLinearAcceleration(linearAcceleration);
            rigidbody.AddAngularAcceleration(angularAcceleration);
        }


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Lift Drive Block ==");
            base.AppendCustomInfo(block, customInfo);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_lift_drive_working_start", "repulsor_lift_drive_working_loop", "repulsor_lift_drive_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("a0498045-505d-4691-bb0c-00c515b01793");
        }
    }

}
