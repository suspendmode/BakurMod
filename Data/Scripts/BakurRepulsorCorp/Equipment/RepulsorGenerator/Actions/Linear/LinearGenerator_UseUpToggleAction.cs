
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class LinearGenerator_UseUpToggleAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_UseUpToggleAction() : base("LinearGenerator_UseUpToggleAction", "Use Up On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useUp = !equipment.useUp;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useUp ? "Up On" : "Up Off");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}