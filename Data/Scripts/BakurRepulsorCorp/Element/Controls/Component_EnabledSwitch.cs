namespace BakurRepulsorCorp {

    public class Component_EnabledSwitch : Switch<DefaultLogicElement> {

        public Component_EnabledSwitch() : base("Component_EnabledSwitch", "Toggle block", "Toggle block On/Off", "On", "Off") {
        }

        protected override bool GetValue(DefaultLogicElement equipment) {
            return equipment.enabled;
        }

        protected override void SetValue(DefaultLogicElement equipment, bool value) {
            equipment.enabled = value;
        }
    }
}