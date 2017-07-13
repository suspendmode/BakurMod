
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseUpEnableAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_UseUpEnableAction() : base("LinearGenerator_UseUpEnableAction", "Enable Use Up") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useUp = true;
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