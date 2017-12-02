using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorPointLift", "LargeBlockRepulsorPointLift" })]
    public class RepulsorPointLiftComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        private static readonly string[] subTypeIds = { "SmallBlockRepulsorPointLift", "LargeBlockRepulsorPointLift" };

        RepulsorPointLift repulsorPointLift;
        RepulsorPointLiftUIController<IMyUpgradeModule> repulsorPointLiftUI;

        AltitudeSensor altitudeSensor;
        AltitudeSensorUIController<IMyUpgradeModule> altitudeSensorUI;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            altitudeSensor = new AltitudeSensor(this);
            AddElement(altitudeSensor);

            altitudeSensorUI = new AltitudeSensorUIController<IMyUpgradeModule>(this);
            AddElement(altitudeSensorUI);

            repulsorPointLift = new RepulsorPointLift(this);
            AddElement(repulsorPointLift);

            repulsorPointLiftUI = new RepulsorPointLiftUIController<IMyUpgradeModule>(this);
            AddElement(repulsorPointLiftUI);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(repulsorPointLift);
            repulsorPointLift = null;

            RemoveElement(repulsorPointLiftUI);
            repulsorPointLiftUI = null;

            RemoveElement(altitudeSensor);
            altitudeSensor = null;

            RemoveElement(altitudeSensorUI);
            altitudeSensorUI = null;
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Lift Component");
            base.AppendCustomInfo(block, customInfo);
        }

        Vector3D desiredLinearAcceleration;
        Vector3D desiredUp;

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {
            desiredLinearAcceleration = Vector3D.Zero;
            desiredUp = block.WorldMatrix.Up;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            altitudeSensor.UpdateSensor();

            if (double.IsNaN(altitudeSensor.altitude))
            {
                //MyAPIGateway.Utilities.ShowMessage("RepulsorLiftBlock", "double.IsNaN(planetAltitudeSensor.altitude)");
                return;
            }

            // lift           

            double size = block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5;
            desiredLinearAcceleration = repulsorPointLift.GetDesiredLinearAcceleration(physicsDeltaTime, desiredUp, altitudeSensor.altitude - size);

            // apply

            rigidbody.AddLinearAcceleration(desiredLinearAcceleration);
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
            return new Guid("49e48146-4c44-40dd-a002-aac5629960c6");
        }
    }

}
