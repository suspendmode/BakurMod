using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Component_EnableAction : BaseControlAction<BakurBlockEquipment> {

        public Component_EnableAction() : base("Component_EnableAction", "Toggle block On") {
        }

        public override void Action(IMyTerminalBlock block) {
            BakurBlockEquipment equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.enabled = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            BakurBlockEquipment equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(equipment.enabled ? "On" : "Off");
        }
    }
}