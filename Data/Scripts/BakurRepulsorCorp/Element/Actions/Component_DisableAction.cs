using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Component_DisableAction : UIControlAction<DefaultLogicElement> {

        public Component_DisableAction() : base("Component_DisableAction", "Toggle block Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            DefaultLogicElement equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.enabled = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            DefaultLogicElement equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(equipment.enabled ? "On" : "Off");
        }
    }
}