using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseForwardSwitch : SwitchControl<RepulsorLinearGenerator> {

        public LinearGenerator_UseForwardSwitch() : base("LinearGenerator_UseForwardSwitch", "Use Forward", "Use Forward (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorLinearGenerator equipment) {
            return equipment.useForward;
        }

        protected override void SetValue(RepulsorLinearGenerator equipment, bool value) {
            equipment.useForward = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}