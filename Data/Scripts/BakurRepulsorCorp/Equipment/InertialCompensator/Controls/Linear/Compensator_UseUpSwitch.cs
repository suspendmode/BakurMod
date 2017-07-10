using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Compensator_UseUpSwitch : SwitchControl<LinearInertialCompensator> {

        public Compensator_UseUpSwitch() : base("Compensator_UseUpSwitch", "Use Up", "Use Up (false/true)", "On", "Off") {
        }

        protected override bool GetValue(LinearInertialCompensator equipment) {
            return equipment.useUp;
        }

        protected override void SetValue(LinearInertialCompensator equipment, bool value) {
            equipment.useUp = value;
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