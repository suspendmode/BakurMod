
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Gyro_UseGravityNormalEnableAction : UIControlAction<GyroStabiliser> {

        public Gyro_UseGravityNormalEnableAction() : base("GyroStabiliser_UseGravityNormalEnableAction", "Enable Use Gravity Normal") {
        }

        public override void Action(IMyTerminalBlock block) {
            GyroStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useSurfaceNormal = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            GyroStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(equipment.useSurfaceNormal ? "Grav On" : "Grav Off");
        }
    }
}