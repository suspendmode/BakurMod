namespace BakurRepulsorCorp {

    public class Component_DebugEnabledSwitch : Switch<DefaultLogicElement> {

        public Component_DebugEnabledSwitch() : base("BakurBlockEquipment_BakurBlockEquipmentDebugEnabledSwitch", "Debug Enabled", "Debug Enabled (false/true)", "On", "Off") {
        }

        protected override bool GetValue(DefaultLogicElement equipment) {
            return equipment.debugEnabled;
        }

        protected override void SetValue(DefaultLogicElement equipment, bool value) {
            equipment.debugEnabled = value;
        }
    }
}