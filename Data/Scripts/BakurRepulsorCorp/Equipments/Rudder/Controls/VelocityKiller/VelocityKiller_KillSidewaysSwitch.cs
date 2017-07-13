using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillSidewaysSwitch : SwitchControl<VelocityKiller> {

        public VelocityKiller_KillSidewaysSwitch() : base("VelocityKiller_KillSidewaysSwitch", "Kill Sideways", "Kill Sideways (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityKiller equipment) {
            return equipment.killSideways;
        }

        protected override void SetValue(VelocityKiller equipment, bool value) {
            equipment.killSideways = value;
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