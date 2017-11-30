using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Copter_UseCopterEnableAction : UIControlAction<Copter> {

        public Copter_UseCopterEnableAction() : base("Copter_UseCopterEnableAction", "Enable Copter") {
        }

        public override void Action(IMyTerminalBlock block) {
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useCopter = true;
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