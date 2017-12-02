using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorLift", "LargeBlockRepulsorLift" })]
    public class RepulsorLiftComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        private static readonly string[] subTypeIds = { "SmallBlockRepulsorLift", "LargeBlockRepulsorLift" };

        PlanetAltitudeSensor planetAltitudeSensor;
        PlanetAltitudeSensorUIController<IMyUpgradeModule> planetAltitudeSensorUI;

        RepulsorLift repulsorLift;
        RepulsorLiftUIController<IMyUpgradeModule> repulsorLiftUI;

        #region lifecycle

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

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(repulsorLift);
            repulsorLift = null;

            RemoveElement(repulsorLiftUI);
            repulsorLiftUI = null;

            RemoveElement(planetAltitudeSensor);
            planetAltitudeSensor = null;

            RemoveElement(planetAltitudeSensorUI);
            planetAltitudeSensorUI = null;
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Lift Component");
            base.AppendCustomInfo(block, customInfo);
        }

        Vector3D liftAcceleration = Vector3D.Zero;
        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {
            liftAcceleration = Vector3D.Zero;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);

            if (double.IsNaN(planetAltitudeSensor.altitude))
            {
                //MyAPIGateway.Utilities.ShowMessage("RepulsorLiftBlock", "double.IsNaN(planetAltitudeSensor.altitude)");
                return;
            }

            // lift
            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length());
            liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);

            // apply

            rigidbody.AddLinearAcceleration(liftAcceleration);
        }

        public override void DrawEmissive()
        {
            if (block.CubeGrid.IsStatic)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 120, 0), 1);
            }
            else if (!block.IsWorking || !block.IsFunctional || !enabled || !rigidbody.IsInGravity)
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
                return new string[] { "repulsor_lift_working_start", "repulsor_lift_working_loop", "repulsor_lift_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("3a95a757-3c62-4d4a-a88e-7fa2a2835922");
        }

    }

}
