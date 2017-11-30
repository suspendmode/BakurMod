using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillYawSwitch : Switch<VelocityKiller> {

        public VelocityKiller_KillYawSwitch() : base("VelocityKiller_KillYawSwitch", "Kill Yaw", "Kill Yaw (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityKiller equipment) {
            return equipment.killYaw;
        }

        protected override void SetValue(VelocityKiller equipment, bool value) {
            equipment.killYaw = value;
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