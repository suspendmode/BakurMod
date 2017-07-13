using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Compensator_UseForwardSwitch : SwitchControl<LinearInertialCompensator> {

        public Compensator_UseForwardSwitch() : base("Compensator_UseForwardSwitch", "Use Forward", "Use Forward (false/true)", "On", "Off") {
        }

        protected override bool GetValue(LinearInertialCompensator equipment) {
            return equipment.useForward;
        }

        protected override void SetValue(LinearInertialCompensator equipment, bool value) {
            equipment.useForward = value;
        }
        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useLinearCompensator;
        }
    }
}