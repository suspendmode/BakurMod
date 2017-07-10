using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Gyro_UseGravityNormalDisableAction : BaseControlAction<GyroStabiliser> {

        public Gyro_UseGravityNormalDisableAction() : base("GyroStabiliser_UseGravityNormalDisableAction", "Disable Use Gravity Normal") {
        }

        public override void Action(IMyTerminalBlock block) {
            GyroStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useSurfaceNormal = false;
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