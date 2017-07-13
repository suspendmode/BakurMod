using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseUpSwitch : SwitchControl<RepulsorLinearGenerator> {

        public LinearGenerator_UseUpSwitch() : base("LinearGenerator_UseUpSwitch", "Use Up", "Use Up (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorLinearGenerator equipment) {
            return equipment.useUp;
        }

        protected override void SetValue(RepulsorLinearGenerator equipment, bool value) {
            equipment.useUp = value;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}