
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Gyro_UseSurfaceNormalEnableAction : UIControlAction<GyroStabiliser> {

        public Gyro_UseSurfaceNormalEnableAction() : base("GyroStabiliser_UseSurfaceNormalEnableAction", "Enable Use Surface Normal") {
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
            GyroStabiliser component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useSurfaceNormal ? "Surf On" : "Surf Off");
        }
    }
}