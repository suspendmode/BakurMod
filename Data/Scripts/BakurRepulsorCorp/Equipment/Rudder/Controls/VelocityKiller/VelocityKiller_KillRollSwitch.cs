using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillRollSwitch : SwitchControl<VelocityKiller> {

        public VelocityKiller_KillRollSwitch() : base("VelocityKiller_KillRollSwitch", "Kill Roll", "Kill Roll (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityKiller equipment) {
            return equipment.killRoll;
        }

        protected override void SetValue(VelocityKiller equipment, bool value) {
            equipment.killRoll = value;
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