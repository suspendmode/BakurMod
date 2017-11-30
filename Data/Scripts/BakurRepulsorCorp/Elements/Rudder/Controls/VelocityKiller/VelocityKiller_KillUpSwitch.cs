using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillUpSwitch : Switch<VelocityKiller> {

        public VelocityKiller_KillUpSwitch() : base("VelocityKiller_KillUpSwitch", "Kill Up", "Kill Up (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityKiller equipment) {
            return equipment.killUp;
        }

        protected override void SetValue(VelocityKiller equipment, bool value) {
            equipment.killUp = value;
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