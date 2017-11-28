namespace BakurRepulsorCorp {

    public class VelocityRudder_ReverseGearSwitch : SwitchControl<VelocityRudder> {

        public VelocityRudder_ReverseGearSwitch() : base("VelocityRudder_ReverseGearSwitch", "Reverse Gear", "Reverse Gear (Backward,Forward)", "Backward", "Forward") {
        }

        protected override bool GetValue(VelocityRudder equipment) {
            return equipment.reverseGear;
        }

        protected override void SetValue(VelocityRudder equipment, bool value) {
            equipment.reverseGear = value;
        }
    }
}