using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp
{

    public class Generator_UseGeneratorDisableAction : UIControlAction<AntiGravityGenerator>
    {

        public Generator_UseGeneratorDisableAction() : base("Generator_UseGeneratorDisableAction", "Disable Generator")
        {
        }

        public override void Action(IMyTerminalBlock block)
        {
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            equipment.useGenerator = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            AntiGravityGenerator component = GetEquipment(block);
            if (component == null)
            {
                return;
            }
            builder.Append(component.useGenerator ? "On" : "Off");
        }
    }
}
