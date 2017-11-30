using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;

namespace BakurRepulsorCorp
{

    public class Separator<TEquipment> : UIControl<TEquipment> where TEquipment : LogicElement
    {

        public Separator(
            string controlId)
            : base(controlId, "Separator", "Separator")
        {
        }

        protected override IMyTerminalControl CreateControl()
        {
            IMyTerminalControlSeparator seperator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyUpgradeModule>(controlId);
            seperator.SupportsMultipleBlocks = true;
            seperator.Enabled = Enabled;
            seperator.Visible = Visible;
            return seperator;
        }

        protected override bool Visible(IMyTerminalBlock block)
        {
            if (!base.Visible(block))
            {
                return false;
            }
            TEquipment equipment = GetEquipment(block);
            if (equipment == null)
            {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}
