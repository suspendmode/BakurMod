namespace BakurRepulsorCorp {

    public class Component_DebugEnabledSwitch : SwitchControl<BakurBlockEquipment> {

        public Component_DebugEnabledSwitch() : base("BakurBlockEquipment_BakurBlockEquipmentDebugEnabledSwitch", "Debug Enabled", "Debug Enabled (false/true)", "On", "Off") {
        }

        protected override bool GetValue(BakurBlockEquipment equipment) {
            return equipment.debugEnabled;
        }

        protected override void SetValue(BakurBlockEquipment equipment, bool value) {
            equipment.debugEnabled = value;
        }
    }
}