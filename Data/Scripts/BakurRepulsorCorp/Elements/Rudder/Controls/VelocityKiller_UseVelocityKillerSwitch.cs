namespace BakurRepulsorCorp {

    public class VelocityKiller_UseVelocityKillerSwitch : Switch<VelocityKiller> {

        public VelocityKiller_UseVelocityKillerSwitch() : base("VelocityKiller_UseVelocityKillerSwitch", "Use Velocity Killer", "Use Velocity Killer (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityKiller equipment) {
            return equipment.useVelocityKiller;
        }

        protected override void SetValue(VelocityKiller equipment, bool value) {
            equipment.useVelocityKiller = value;
        }
    }
}