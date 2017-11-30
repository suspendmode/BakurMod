
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseForwardEnableAction : UIControlAction<LinearInertialCompensator> {

        public Compensator_UseForwardEnableAction() : base("Compensator_UseForwardEnableAction", "Enable Use Forward") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useForward = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            LinearInertialCompensator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useForward ? "Fwd On" : "Fwd Off");
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