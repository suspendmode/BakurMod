using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Compensator_UseRollSwitch : SwitchControl<AngularInertialCompensator> {

        public Compensator_UseRollSwitch() : base("Compensator_UseRollSwitch", "Use Roll", "Use Roll (false/true)", "On", "Off") {
        }

        protected override bool GetValue(AngularInertialCompensator equipment) {
            return equipment.useRoll;
        }

        protected override void SetValue(AngularInertialCompensator equipment, bool value) {
            equipment.useRoll = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useAngularCompensator;
        }
    }
}