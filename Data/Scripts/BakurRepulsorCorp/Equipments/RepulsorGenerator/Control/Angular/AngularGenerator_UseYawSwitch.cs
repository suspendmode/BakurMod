using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class AngularGenerator_UseYawSwitch : SwitchControl<RepulsorAngularGenerator> {

        public AngularGenerator_UseYawSwitch() : base("AngularGenerator_UseYawSwitch", "Use Yaw", "Use Yaw (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorAngularGenerator equipment) {
            return equipment.useYaw;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, bool value) {
            equipment.useYaw = value;
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