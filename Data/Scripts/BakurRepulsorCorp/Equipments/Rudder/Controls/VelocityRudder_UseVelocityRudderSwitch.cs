namespace BakurRepulsorCorp {

    public class VelocityRudder_UseVelocityRudderSwitch : SwitchControl<VelocityRudder> {

        public VelocityRudder_UseVelocityRudderSwitch() : base("VelocityRudder_UseEthericRudderSwitch", "Use Etheric Rudder", "Use Etheric Rudder (false/true)", "On", "Off") {
        }

        protected override bool GetValue(VelocityRudder equipment) {
            return equipment.useVelocityRudder;
        }

        protected override void SetValue(VelocityRudder equipment, bool value) {
            equipment.useVelocityRudder = value;
        }
    }
}