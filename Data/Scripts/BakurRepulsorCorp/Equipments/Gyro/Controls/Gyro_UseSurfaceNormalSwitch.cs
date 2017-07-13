namespace BakurRepulsorCorp {

    public class Gyro_UseSurfaceNormalSwitch : SwitchControl<GyroStabiliser> {

        public Gyro_UseSurfaceNormalSwitch() : base("GyroStabiliser_UseSurfaceNormalSwitch", "Use Surface Normal", "Use Surface Normal (false/true)", "On", "Off") {
        }

        protected override bool GetValue(GyroStabiliser equipment) {
            return equipment.useSurfaceNormal;
        }

        protected override void SetValue(GyroStabiliser equipment, bool value) {
            equipment.useSurfaceNormal = value;
        }
    }
}