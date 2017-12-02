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
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        VelocityRudder velocityRudder;
        VelocityRudderUIController<IMyUpgradeModule> velocityRudderUI;

        VelocityKiller velocityKiller;
        VelocityKillerUIController<IMyUpgradeModule> velocityKillerUI;

        #region lifecycle

        protected override void Initialize()
        {
            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            velocityRudder = new VelocityRudder(this);
            AddElement(velocityRudder);

            velocityRudderUI = new VelocityRudderUIController<IMyUpgradeModule>(this);
            AddElement(velocityRudderUI);

            velocityKiller = new VelocityKiller(this);
            AddElement(velocityKiller);

            velocityKillerUI = new VelocityKillerUIController<IMyUpgradeModule>(this);
            AddElement(velocityKillerUI);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(velocityRudder);
            velocityRudder = null;

            RemoveElement(velocityRudderUI);
            velocityRudderUI = null;

            RemoveElement(velocityKiller);
            velocityKiller = null;

            RemoveElement(velocityKillerUI);
            velocityKillerUI = null;
        }

        #endregion

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
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
