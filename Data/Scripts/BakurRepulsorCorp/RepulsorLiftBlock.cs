using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsorLift", "LargeBlockRepulsorLift" })]
    public class RepulsorLiftBlock : BakurBlock
    {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorLift", "LargeBlockRepulsorLift" };

        PlanetAltitudeSensor planetAltitudeSensor;
        RepulsorLift repulsorLift;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddEquipment(planetAltitudeSensor);

            repulsorLift = new RepulsorLift(this);
            AddEquipment(repulsorLift);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(repulsorLift);
            repulsorLift = null;

            RemoveEquipment(planetAltitudeSensor);
            planetAltitudeSensor = null;
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Lift Block ==");
            base.AppendCustomInfo(block, customInfo);
        }

        Vector3D liftAcceleration = Vector3D.Zero;
        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            liftAcceleration = Vector3D.Zero;

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            // lift
            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length()) / 2;
            liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);

            // apply

            rigidbody.AddLinearAcceleration(liftAcceleration);
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
