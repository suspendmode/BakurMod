using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;

namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockEthericRudder", "LargeBlockEthericRudder" })]
    public class EthericRudderComponent : LogicComponent
    {

        VelocityRudder velocityRudder;
        VelocityKiller velocityKiller;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            velocityRudder = new VelocityRudder(this);
            AddEquipment(velocityRudder);

            velocityKiller = new VelocityKiller(this);
            AddEquipment(velocityKiller);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(velocityRudder);
            velocityRudder = null;

            RemoveEquipment(velocityKiller);
            velocityKiller = null;
        }

        #endregion

        protected override void UpdateSimulation(double physicsDeltaTime)
        {
            if (velocityRudder.useVelocityRudder)
            {
                velocityRudder.UpdateAfterSimulation(physicsDeltaTime);
            }
            if (velocityKiller.useVelocityKiller)
            {
                velocityKiller.UpdateAfterSimulation(physicsDeltaTime);
            }
        }


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Etheric Rudder Component");

            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "etheric_rudder_coupler_working_start", "etheric_rudder_coupler_working_loop", "etheric_rudder_coupler_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("a4f121f2-37ee-4b30-9d04-a0adc17cdfd9");
        }
    }
}
