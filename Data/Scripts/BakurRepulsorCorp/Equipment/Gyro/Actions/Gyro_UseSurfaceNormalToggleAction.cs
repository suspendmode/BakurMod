
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class Gyro_UseSurfaceNormalToggleAction : BaseControlAction<GyroStabiliser> {

        public Gyro_UseSurfaceNormalToggleAction() : base("GyroStabiliser_UseSurfaceNormalToggleAction", "Use Surface Normal On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            GyroStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useSurfaceNormal = !equipment.useSurfaceNormal;
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