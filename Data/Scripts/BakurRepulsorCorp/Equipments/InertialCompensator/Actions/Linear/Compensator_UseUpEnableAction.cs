
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseUpEnableAction : BaseControlAction<LinearInertialCompensator> {

        public Compensator_UseUpEnableAction() : base("Compensator_UseUpEnableAction", "Enable Use Up") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useUp = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            LinearInertialCompensator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useUp ? "Up On" : "Up Off");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useLinearCompensator;
        }
    }
}