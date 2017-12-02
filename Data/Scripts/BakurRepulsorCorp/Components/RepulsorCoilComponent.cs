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

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorCoil", "LargeBlockRepulsorCoil" })]
    public class RepulsorCoilComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        PlanetAltitudeSensor planetAltitudeSensor;
        PlanetAltitudeSensorUIController<IMyUpgradeModule> planetAltitudeSensorUI;

        RepulsorCoil repulsorCoil;
        RepulsorCoilUIController<IMyUpgradeModule> repulsorCoilUI;


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

            /*SetPowerRequirements(block, () => {
                return repulsorCoil.PowerRequirements();
            });*/
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(repulsorCoil);
            repulsorCoil = null;

            RemoveElement(repulsorCoilUI);
            repulsorCoilUI = null;

            RemoveElement(planetAltitudeSensor);
            planetAltitudeSensor = null;

            RemoveElement(planetAltitudeSensorUI);
            planetAltitudeSensorUI = null;
        }

        Vector3D coilAcceleration;

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {
            coilAcceleration = Vector3D.Zero;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);

            // coil

            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length()) / 2;
            coilAcceleration = repulsorCoil.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);

            // apply

            rigidbody.AddLinearAcceleration(coilAcceleration);
        }

        protected override void Debug()
        {
            if (debugEnabled)
            {
                IMyCubeGrid grid = block.CubeGrid;
                DebugDraw.DrawLine(block.GetPosition(), block.GetPosition() + rigidbody.gravityUp * rigidbody.gravity.Length(), Color.DeepSkyBlue, 0.1f);
            }
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

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Coil Component");
            customInfo.AppendLine("Use: " + (repulsorCoil.useCoil ? "On" : "Off"));
            base.AppendCustomInfo(block, customInfo);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_coil_working_start", "repulsor_coil_working_loop", "repulsor_coil_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("bf8957e0-500d-4356-9875-91db9dd4a912");
        }

    }

}
