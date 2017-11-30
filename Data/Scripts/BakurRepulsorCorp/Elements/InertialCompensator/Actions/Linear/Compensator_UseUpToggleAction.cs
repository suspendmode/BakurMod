
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class Compensator_UseUpToggleAction : UIControlAction<LinearInertialCompensator> {

        public Compensator_UseUpToggleAction() : base("Compensator_UseUpToggleAction", "Use Up On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useUp = !equipment.useUp;
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