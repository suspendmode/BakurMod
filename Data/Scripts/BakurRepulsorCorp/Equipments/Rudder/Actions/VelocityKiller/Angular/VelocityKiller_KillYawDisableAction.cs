using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillYawDisableAction : BaseControlAction<VelocityKiller> {

        public VelocityKiller_KillYawDisableAction() : base("VelocityKiller_KillYawDisableAction", "Disable Kill Yaw") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.killYaw = false;
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