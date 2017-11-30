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
    public class RepulsorLiftCoilBlock : BakurBlock
    {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorLiftCoil", "LargeBlockRepulsorLiftCoil" };

        RepulsorCoil repulsorCoil;
        RepulsorLift repulsorLift;
        PlanetAltitudeSensor planetAltitudeSensor;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            repulsorCoil = new RepulsorCoil(this);
            AddEquipment(repulsorCoil);

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddEquipment(planetAltitudeSensor);

            repulsorLift = new RepulsorLift(this);
            AddEquipment(repulsorLift);

        }

        protected override void Destroy()
        {


            base.Destroy();

            RemoveEquipment(repulsorCoil);
            repulsorCoil = null;

            RemoveEquipment(repulsorLift);
            repulsorLift = null;

            RemoveEquipment(planetAltitudeSensor);
            planetAltitudeSensor = null;
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Lift Coil Block ==");
            customInfo.AppendLine("Lift Force : " + Math.Round(liftAcceleration.Length(), 2));
            base.AppendCustomInfo(block, customInfo);
        }


        Vector3D coilAcceleration = Vector3D.Zero;
        Vector3D liftAcceleration = Vector3D.Zero;

        protected override void UpdateSimulation(double physicsDeltaTime)
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

            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length()) / 2;
            liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);

            // apply

            rigidbody.AddLinearAcceleration(liftAcceleration);
            rigidbody.AddLinearAcceleration(coilAcceleration);
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
