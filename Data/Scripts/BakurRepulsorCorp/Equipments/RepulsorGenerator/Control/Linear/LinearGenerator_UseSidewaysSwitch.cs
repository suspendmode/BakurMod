using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseSidewaysSwitch : SwitchControl<RepulsorLinearGenerator> {

        public LinearGenerator_UseSidewaysSwitch() : base("LinearGenerator_UseSidewaysSwitch", "Use Sideways", "Use Sideways (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorLinearGenerator equipment) {
            return equipment.useSideways;
        }

        protected override void SetValue(RepulsorLinearGenerator equipment, bool value) {
            equipment.useSideways = value;
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