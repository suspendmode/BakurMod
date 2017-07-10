using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class Compensator_UseLinearCompensationToggleAction : BaseControlAction<LinearInertialCompensator> {

        public Compensator_UseLinearCompensationToggleAction() : base("LinearInertialCompensator_UseLinearCompensationToggleAction", "Use Linear Compensation On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useLinearCompensator = !equipment.useLinearCompensator;
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