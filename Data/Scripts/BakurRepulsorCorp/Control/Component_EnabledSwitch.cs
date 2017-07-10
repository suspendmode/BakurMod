namespace BakurRepulsorCorp {

    public class Component_EnabledSwitch : SwitchControl<BakurBlockEquipment> {

        public Component_EnabledSwitch() : base("BakurBlockEquipment_BakurBlockEquipmentEnabledSwitch", "Enabled", "Enabled (false/true)", "On", "Off") {
        }

        protected override bool GetValue(BakurBlockEquipment equipment) {
            return equipment.enabled;
        }

        protected override void SetValue(BakurBlockEquipment equipment, bool value) {
            equipment.enabled = value;
        }
    }
}