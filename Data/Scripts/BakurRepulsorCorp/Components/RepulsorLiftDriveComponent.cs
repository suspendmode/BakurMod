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
    public class RepulsorLiftDriveComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        PlanetAltitudeSensor planetAltitudeSensor;
        PlanetAltitudeSensorUIController<IMyUpgradeModule> planetAltitudeSensorUI;

        RepulsorLift repulsorLift;
        RepulsorLiftUIController<IMyUpgradeModule> repulsorLiftUI;

        RepulsorLinearGenerator repulsorLinearGenerator;
        RepulsorLinearGeneratorUIController<IMyUpgradeModule> repulsorLinearGeneratorUI;

        RepulsorAngularGenerator repulsorAngularGenerator;
        RepulsorAngularGeneratorUIController<IMyUpgradeModule> repulsorAngularGeneratorUI;

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddElement(planetAltitudeSensor);

            planetAltitudeSensorUI = new PlanetAltitudeSensorUIController<IMyUpgradeModule>(this);
            AddElement(planetAltitudeSensorUI);

            repulsorLift = new RepulsorLift(this);
            AddElement(repulsorLift);

            repulsorLiftUI = new RepulsorLiftUIController<IMyUpgradeModule>(this);
            AddElement(repulsorLiftUI);

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

            RemoveElement(repulsorLift);
            repulsorLift = null;

            RemoveElement(repulsorLiftUI);
            repulsorLiftUI = null;

            RemoveElement(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            RemoveElement(repulsorLinearGeneratorUI);
            repulsorLinearGeneratorUI = null;

            RemoveElement(repulsorAngularGenerator);
            repulsorAngularGenerator = null;

            RemoveElement(repulsorAngularGeneratorUI);
            repulsorAngularGeneratorUI = null;

            RemoveElement(planetAltitudeSensor);
            planetAltitudeSensor = null;

            RemoveElement(planetAltitudeSensorUI);
            planetAltitudeSensorUI = null;
        }

        Vector3D linearAcceleration;
        Vector3D angularAcceleration;
        Vector3D liftAcceleration;

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
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
                double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length());
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
            customInfo.AppendLine("Type: Repulsor Lift Drive Component");
            base.AppendCustomInfo(block, customInfo);
        }
        public override void DrawEmissive()
        {
            if (block.CubeGrid.IsStatic || !rigidbody.IsInGravity)
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
