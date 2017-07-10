using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class AngularGenerator_UsePitchSwitch : SwitchControl<RepulsorAngularGenerator> {

        public AngularGenerator_UsePitchSwitch() : base("AngularGenerator_UsePitchSwitch", "Use Pitch", "Use Pitch (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorAngularGenerator equipment) {
            return equipment.usePitch;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, bool value) {
            equipment.usePitch = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useAngularGenerator;
        }
    }
}