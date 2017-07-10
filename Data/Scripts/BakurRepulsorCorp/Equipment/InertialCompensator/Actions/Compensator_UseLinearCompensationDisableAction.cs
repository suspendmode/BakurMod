using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseLinearCompensationDisableAction : BaseControlAction<LinearInertialCompensator> {

        public Compensator_UseLinearCompensationDisableAction() : base("LinearInertialCompensator_UseLinearCompensationDisableAction", "Disable Use Linear Compensation") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useLinearCompensator = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            LinearInertialCompensator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useLinearCompensator ? "Lin On" : "Lin Off");
        }
    }
}