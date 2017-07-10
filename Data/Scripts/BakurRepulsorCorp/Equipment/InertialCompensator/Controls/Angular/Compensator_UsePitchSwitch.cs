using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Compensator_UsePitchSwitch : SwitchControl<AngularInertialCompensator> {

        public Compensator_UsePitchSwitch() : base("Compensator_UsePitchSwitch", "Use Pitch", "Use Pitch (false/true)", "On", "Off") {
        }

        protected override bool GetValue(AngularInertialCompensator equipment) {
            return equipment.usePitch;
        }

        protected override void SetValue(AngularInertialCompensator equipment, bool value) {
            equipment.usePitch = value;
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