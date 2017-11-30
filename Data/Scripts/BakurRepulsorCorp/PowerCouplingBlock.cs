using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRageMath;


namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockPowerCoupling", "LargeBlockPowerCoupling" })]
    public class PowerCouplingBlock : BakurBlock
    {

        PowerCoupling powerCoupling;
        AttitudeStabiliser attitudeStabiliser;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            powerCoupling = new PowerCoupling(this);
            AddEquipment(powerCoupling);

            attitudeStabiliser = new AttitudeStabiliser(this);
            AddEquipment(attitudeStabiliser);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(powerCoupling);
            powerCoupling = null;

            RemoveEquipment(attitudeStabiliser);
            attitudeStabiliser = null;
        }

        #endregion

        #region update

        public override void UpdateBeforeSimulation10()
        {
            base.UpdateBeforeSimulation10();
        }

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            Vector3D direction = block.WorldMatrix.Forward;
            Vector3D force = powerCoupling.GetOutput(physicsDeltaTime, out direction);

            if (force != Vector3D.Zero)
            {
                Vector3D forcePosition = block.GetPosition();
                rigidbody.AddForce(force, forcePosition);
            }


            Vector3D currentUp = block.WorldMatrix.Up;
            Vector3D currentForward = block.WorldMatrix.Forward;
            Vector3D desiredUp = direction;
            Vector3D desiredForward = block.WorldMatrix.Forward;
            Vector3D angularStabilisationVelocity = attitudeStabiliser.GetAngularAcceleration(physicsDeltaTime, currentForward, currentUp, desiredForward, desiredUp);
            rigidbody.AddAngularAcceleration(angularStabilisationVelocity / physicsDeltaTime);
        }

        #endregion


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Power Coupling Block ==");

            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "power_coupler_working_start", "power_coupler_working_loop", "power_coupler_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("920716a3-2e52-4107-bc9d-43dae6c9b1ae");
        }
    }
}
