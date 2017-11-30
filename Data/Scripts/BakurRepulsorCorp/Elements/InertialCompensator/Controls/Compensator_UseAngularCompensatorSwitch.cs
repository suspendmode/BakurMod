namespace BakurRepulsorCorp {

    public class Compensator_UseAngularCompensatorSwitch : Switch<AngularInertialCompensator> {

        public Compensator_UseAngularCompensatorSwitch() : base("AngularInertialCompensator_UseAngularCompensatorSwitch", "Use Angular Compensation", "Use Angular Compensation (false/true)", "On", "Off") {
        }

        protected override bool GetValue(AngularInertialCompensator equipment) {
            return equipment.useAngularCompensator;
        }

        protected override void SetValue(AngularInertialCompensator equipment, bool value) {
            equipment.useAngularCompensator = value;
        }
    }
}