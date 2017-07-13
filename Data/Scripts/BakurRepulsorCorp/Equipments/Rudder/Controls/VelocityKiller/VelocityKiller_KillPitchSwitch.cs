using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillPitchSwitch : SwitchControl<VelocityKiller> {

        public VelocityKiller_KillPitchSwitch() : base("VelocityKiller_KillPitchSwitch", "Kill Pitch", "Kill Pitch (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityKiller equipment) {
            return equipment.killPitch;
        }

        protected override void SetValue(VelocityKiller equipment, bool value) {
            equipment.killPitch = value;
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