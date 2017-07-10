using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class AngularGenerator_UseRollSwitch : SwitchControl<RepulsorAngularGenerator> {

        public AngularGenerator_UseRollSwitch() : base("AngularGenerator_UseRollSwitch", "Use Roll", "Use Roll (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorAngularGenerator equipment) {
            return equipment.useRoll;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, bool value) {
            equipment.useRoll = value;
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