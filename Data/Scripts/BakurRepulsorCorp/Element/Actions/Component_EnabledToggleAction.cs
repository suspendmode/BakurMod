using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Component_EnabledToggleAction : UIControlAction<DefaultLogicElement> {

        public Component_EnabledToggleAction() : base("Component_EnabledToggleAction", "Toggle block On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            DefaultLogicElement equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }            
            equipment.enabled = !equipment.enabled;
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