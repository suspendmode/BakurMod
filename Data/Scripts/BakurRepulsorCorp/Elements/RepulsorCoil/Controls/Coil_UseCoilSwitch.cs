namespace BakurRepulsorCorp {

    public class Coil_UseCoilSwitch : Switch<RepulsorCoil> {

        public Coil_UseCoilSwitch() : base("Coil_UseCoilSwitch", "Use Coil", "Use Coil (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorCoil equipment) {
            return equipment.useCoil;
        }

        protected override void SetValue(RepulsorCoil equipment, bool value) {
            equipment.useCoil = value;
        }
    }
}