using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Lift_UseRepulsorLiftSwitch : Switch<RepulsorLift> {

        public Lift_UseRepulsorLiftSwitch() : base("Lift_UseRepulsorLiftSwitch", "Use Repulosr Lift", "Use Repulsor Lift (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorLift equipment) {
            return equipment.useLift;
        }

        protected override void SetValue(RepulsorLift equipment, bool value) {
            equipment.useLift = value;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}