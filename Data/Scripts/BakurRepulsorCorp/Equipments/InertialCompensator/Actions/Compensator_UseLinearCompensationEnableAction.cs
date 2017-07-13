using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseLinearCompensationEnableAction : BaseControlAction<LinearInertialCompensator> {

        public Compensator_UseLinearCompensationEnableAction() : base("LinearInertialCompensator_UseLinearCompensationEnableAction", "Enable Use Linear Compensation") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useLinearCompensator = true;
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