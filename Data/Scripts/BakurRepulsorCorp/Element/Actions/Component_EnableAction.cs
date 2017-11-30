using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp
{

    public class Component_EnableAction : UIControlAction<DefaultLogicElement>
    {

        public Component_EnableAction() : base("Component_EnableAction", "Toggle block On")
        {
        }

        public override void Action(IMyTerminalBlock block)
        {
            DefaultLogicElement equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            equipment.enabled = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            DefaultLogicElement equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            builder.Append(equipment.enabled ? "On" : "Off");
        }
    }
}
