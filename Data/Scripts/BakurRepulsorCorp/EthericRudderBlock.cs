using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;

namespace BakurRepulsorCorp {

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockEthericRudder", "LargeBlockEthericRudder" })]
    public class EthericRudderBlock : NonStaticBakurBlock {

        VelocityRudder velocityRudder;
        VelocityKiller velocityKiller;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();

            velocityRudder = new VelocityRudder(this);
            Add(velocityRudder);

            velocityKiller = new VelocityKiller(this);
            Add(velocityKiller);

        }

        protected override void Destroy() {

            base.Destroy();

            Remove(velocityRudder);
            velocityRudder = null;

            Remove(velocityKiller);
            velocityKiller = null;
        }

        #endregion


        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {            

            if (velocityRudder.useVelocityRudder) {
                velocityRudder.UpdateBeforeSimulation(physicsDeltaTime, updateDeltaTime);
            }
            if (velocityKiller.useVelocityKiller) {
                velocityKiller.UpdateBeforeSimulation(physicsDeltaTime, updateDeltaTime);
            }
        }


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Etheric Rudder Block ==");

            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "etheric_rudder_coupler_working_start", "etheric_rudder_coupler_working_loop", "etheric_rudder_coupler_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("a4f121f2-37ee-4b30-9d04-a0adc17cdfd9");
        }
    }
}

