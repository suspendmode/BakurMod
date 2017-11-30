namespace BakurRepulsorCorp {

    public class Copter_UseCopterSwitch : Switch<Copter> {

        public Copter_UseCopterSwitch() : base("Copter_UseCopterSwitch", "Use Copter", "Use Copter (false/true)", "On", "Off") {
        }

        protected override bool GetValue(Copter equipment) {
            return equipment.useCopter;
        }

        protected override void SetValue(Copter equipment, bool value) {
            equipment.useCopter = value;
        }
    }
}