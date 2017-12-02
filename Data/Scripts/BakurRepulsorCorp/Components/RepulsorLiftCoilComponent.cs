using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorLiftCoil", "LargeBlockRepulsorLiftCoil" })]
    public class RepulsorLiftCoilComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        private static readonly string[] subTypeIds = { "SmallBlockRepulsorLiftCoil", "LargeBlockRepulsorLiftCoil" };

        RepulsorCoil repulsorCoil;
        RepulsorCoilUIController<IMyUpgradeModule> repulsorCoilUI;

        RepulsorLift repulsorLift;
        RepulsorLiftUIController<IMyUpgradeModule> repulsorLiftUI;

        PlanetAltitudeSensor planetAltitudeSensor;
        PlanetAltitudeSensorUIController<IMyUpgradeModule> planetAltitudeSensorUI;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            repulsorCoil = new RepulsorCoil(this);
            AddElement(repulsorCoil);

            repulsorCoilUI = new RepulsorCoilUIController<IMyUpgradeModule>(this);
            AddElement(repulsorCoilUI);

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

            RemoveElement(repulsorCoil);
            repulsorCoil = null;

            RemoveElement(repulsorCoilUI);
            repulsorCoilUI = null;

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
            customInfo.AppendLine("Type: Repulsor Lift Coil Component");
            customInfo.AppendLine("Lift Force: " + Math.Round(liftAcceleration.Length(), 2));
            base.AppendCustomInfo(block, customInfo);
        }


        Vector3D coilAcceleration = Vector3D.Zero;
        Vector3D liftAcceleration = Vector3D.Zero;

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);

            liftAcceleration = Vector3D.Zero;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            // coil

            coilAcceleration = repulsorCoil.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude);

            // lift

            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length());
            liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);

            // apply

            rigidbody.AddLinearAcceleration(liftAcceleration);
            rigidbody.AddLinearAcceleration(coilAcceleration);
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
                return new string[] { "repulsor_lift_coil_working_start", "repulsor_lift_coil_working_loop", "repulsor_lift_coil_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("e2df91e5-e7c3-447c-a1e0-dd2da257ed06");
        }
    }

}
