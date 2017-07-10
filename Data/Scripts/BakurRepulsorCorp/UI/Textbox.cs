using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Text;
using VRage.Utils;

namespace BakurRepulsorCorp {

    public abstract class Textbox<TEquipment> : PropertyBasedControl<TEquipment, StringBuilder> where TEquipment : EquipmentBase {

        public Textbox(
            
            string controlId,
            string title,
            string description
            )
            : base(controlId, title, description) {
        }

        protected override IMyTerminalControl CreateControl() {
            IMyTerminalControlTextbox textBoxControl = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlTextbox, IMyTerminalBlock>(controlId);
            textBoxControl.Enabled = Enabled;
            textBoxControl.Visible = Visible;
            textBoxControl.SupportsMultipleBlocks = true;
            textBoxControl.Getter = Getter;
            textBoxControl.Setter = Setter;
            textBoxControl.Title = MyStringId.GetOrCompute(title);
            textBoxControl.Tooltip = MyStringId.GetOrCompute(description);
            return textBoxControl;
        }
    }
}