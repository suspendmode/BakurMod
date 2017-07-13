using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseAngularCompensationEnableAction : BaseControlAction<AngularInertialCompensator> {

        public Compensator_UseAngularCompensationEnableAction() : base("AngularInertialCompensator_UseAngularCompensationEnableAction", "Enable Angular Compensation") {
        }

        public override void Action(IMyTerminalBlock block) {
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useAngularCompensator = true;
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