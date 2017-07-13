using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Copter_UseCopterDisableAction : BaseControlAction<Copter> {

        public Copter_UseCopterDisableAction() : base("Copter_UseCopterDisableAction", "Disable Copter") {
        }

        public override void Action(IMyTerminalBlock block) {
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useCopter = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Copter component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useCopter ? "Copter On" : "Copter Off");
        }
    }
}