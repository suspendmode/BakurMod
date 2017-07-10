using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Compensator_UseSidewaysSwitch : SwitchControl<LinearInertialCompensator> {

        public Compensator_UseSidewaysSwitch() : base("Compensator_UseSidewaysSwitch", "Use Sideways", "Use Forward (false/true)", "On", "Off") {
        }

        protected override bool GetValue(LinearInertialCompensator equipment) {
            return equipment.useSideways;
        }

        protected override void SetValue(LinearInertialCompensator equipment, bool value) {
            equipment.useSideways = value;
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