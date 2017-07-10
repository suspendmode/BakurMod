using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;

namespace BakurRepulsorCorp {

    public class Separator<TEquipment> : BaseControl<TEquipment> where TEquipment : EquipmentBase {

        public Separator(
            string controlId)
            : base(controlId, "Separator", "Separator") {
        }

        protected override IMyTerminalControl CreateControl() {
            IMyTerminalControlSeparator seperator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyTerminalBlock>(controlId);
            seperator.SupportsMultipleBlocks = true;
            seperator.Enabled = Enabled;
            seperator.Visible = Visible;
            return seperator;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            TEquipment equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}