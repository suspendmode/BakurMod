using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseAngularCompensationDisableAction : UIControlAction<AngularInertialCompensator> {

        public Compensator_UseAngularCompensationDisableAction() : base("Angular_UseAngularCompensationDisableAction", "Disable Use Angular Compensation") {
        }

        public override void Action(IMyTerminalBlock block) {
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useAngularCompensator = false;
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