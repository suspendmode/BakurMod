using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Compensator_UseLinearCompensatorSwitch : Switch<LinearInertialCompensator> {

        public Compensator_UseLinearCompensatorSwitch() : base("LinearInertialCompensator_UseLinearCompensatorSwitch", "Use Linear Compensation", "Use Linear Compensation (false/true)", "On", "Off") {
        }

        protected override bool GetValue(LinearInertialCompensator equipment) {
            return equipment.useLinearCompensator;
        }

        protected override void SetValue(LinearInertialCompensator equipment, bool value) {
            equipment.useLinearCompensator = value;
        }

        
    }
}