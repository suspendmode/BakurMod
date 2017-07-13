
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseForwardEnableAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_UseForwardEnableAction() : base("LinearGenerator_UseForwardEnableAction", "Enable Use Forward") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useForward = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useForward ? "Fwd On" : "Fwd Off");
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