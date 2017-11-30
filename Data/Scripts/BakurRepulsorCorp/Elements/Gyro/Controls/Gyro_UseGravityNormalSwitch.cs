namespace BakurRepulsorCorp {

    public class Gyro_UseGravityNormalSwitch : Switch<GyroStabiliser> {

        public Gyro_UseGravityNormalSwitch() : base("GyroStabiliser_UseGravityNormalSwitch", "Use Gravity Normal", "Use Gravity Normal (false/true)", "On", "Off") {
        }

        protected override bool GetValue(GyroStabiliser equipment) {
            return equipment.useGravityNormal;
        }

        protected override void SetValue(GyroStabiliser equipment, bool value) {
            equipment.useGravityNormal = value;
        }
    }
}