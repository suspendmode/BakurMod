using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseAngularCompensationToggleAction : BaseControlAction<AngularInertialCompensator> {

        public Compensator_UseAngularCompensationToggleAction() : base("AngularInertialCompensator_UseAngularCompensationToggleAction", "Use Angular Compensation On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useAngularCompensator = !equipment.useAngularCompensator;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(equipment.useAngularCompensator ? "Ang On" : "Ang Off");
        }
    }
}