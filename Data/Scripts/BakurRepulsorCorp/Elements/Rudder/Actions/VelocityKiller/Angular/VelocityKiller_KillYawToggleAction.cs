
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class VelocityKiller_KillYawToggleAction : UIControlAction<VelocityKiller> {

        public VelocityKiller_KillYawToggleAction() : base("VelocityKiller_KillYawToggleAction", "Kill Yaw On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.killYaw = !equipment.killYaw;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityKiller component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append("Yaw " + (component.killYaw ? "On" : "Off")));
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useVelocityKiller;
        }
    }
}