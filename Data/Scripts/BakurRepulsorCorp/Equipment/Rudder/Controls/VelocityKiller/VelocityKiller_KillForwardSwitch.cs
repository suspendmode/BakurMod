using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillForwardSwitch : SwitchControl<VelocityKiller> {

        public VelocityKiller_KillForwardSwitch() : base("VelocityKiller_KillForwardSwitch", "Kill Forward", "Kill Forward (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityKiller equipment) {
            return equipment.killForward;
        }

        protected override void SetValue(VelocityKiller equipment, bool value) {
            equipment.killForward = value;
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