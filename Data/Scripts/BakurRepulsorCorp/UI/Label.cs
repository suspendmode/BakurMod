using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Utils;

namespace BakurRepulsorCorp {

    public class Label<TEquipment> : BaseControl<TEquipment> where TEquipment : EquipmentBase {

        public Label(
            string controlId, string text)
            : base(controlId, text, "Label") {
        }

        protected override IMyTerminalControl CreateControl() {
            IMyTerminalControlLabel label = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlLabel, IMyTerminalBlock>(controlId);
            label.Label = MyStringId.GetOrCompute(title);
            label.SupportsMultipleBlocks = true;            
            label.Enabled = Enabled;
            label.Visible = Visible;

            return label;
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