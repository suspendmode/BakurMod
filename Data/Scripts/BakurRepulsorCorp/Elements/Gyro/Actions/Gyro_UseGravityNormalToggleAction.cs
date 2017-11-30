
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Gyro_UseGravityNormalToggleAction : UIControlAction<GyroStabiliser> {

        public Gyro_UseGravityNormalToggleAction() : base("GyroStabiliser_UseGravityNormalToggleAction", "Use Gravity Normal On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            GyroStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useGravityNormal = !equipment.useGravityNormal;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            GyroStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(equipment.useGravityNormal ? "Grav On" : "Grav Off");
        }
    }
}