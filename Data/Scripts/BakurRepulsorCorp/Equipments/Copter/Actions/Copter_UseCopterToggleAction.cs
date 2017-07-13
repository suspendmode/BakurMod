using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Copter_UseCopterToggleAction : BaseControlAction<Copter> {

        public Copter_UseCopterToggleAction() : base("Copter_UseCopterToggleAction", "Use Copter On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useCopter = !equipment.useCopter;
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