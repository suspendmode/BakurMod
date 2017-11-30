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


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorPointLift", "LargeBlockRepulsorPointLift" })]
    public class RepulsorPointLiftComponent : LogicComponent
    {

        private static readonly string[] subTypeIds = { "SmallBlockRepulsorPointLift", "LargeBlockRepulsorPointLift" };

        RepulsorPointLift repulsorPointLift;
        AltitudeSensor altitudeSensor;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            altitudeSensor = new AltitudeSensor(this);
            AddEquipment(altitudeSensor);

            repulsorPointLift = new RepulsorPointLift(this);
            AddEquipment(repulsorPointLift);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(repulsorPointLift);
            repulsorPointLift = null;

            RemoveEquipment(altitudeSensor);
            altitudeSensor = null;
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

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            altitudeSensor.UpdateSensor();

            IMyCubeGrid grid = block.CubeGrid;

            desiredLinearAcceleration = Vector3D.Zero;
            desiredUp = block.WorldMatrix.Up;

            // lift
            double gridHalfSize = block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5;
            desiredLinearAcceleration = repulsorPointLift.GetDesiredLinearAcceleration(physicsDeltaTime, desiredUp, altitudeSensor.altitude - gridHalfSize);
            rigidbody.AddLinearAcceleration(desiredLinearAcceleration);

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
