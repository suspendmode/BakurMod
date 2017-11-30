
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class Compensator_UseYawToggleAction : UIControlAction<AngularInertialCompensator> {

        public Compensator_UseYawToggleAction() : base("Compensator_UseYawToggleAction", "Use Yaw On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useYaw = !equipment.useYaw;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AngularInertialCompensator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useYaw ? "Yaw On" : "Yaw Off");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useAngularCompensator;
        }
    }
}