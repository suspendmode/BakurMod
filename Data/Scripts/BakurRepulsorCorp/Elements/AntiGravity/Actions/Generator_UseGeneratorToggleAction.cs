using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp
{


    public class Generator_UseGeneratorToggleAction : UIControlAction<AntiGravityGenerator>
    {

        public Generator_UseGeneratorToggleAction() : base("Generator_UseGeneratorToggleAction", "Use Generator On/Off")
        {
        }

        public override void Action(IMyTerminalBlock block)
        {
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            equipment.useGenerator = !equipment.useGenerator;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            AntiGravityGenerator component = GetEquipment(block);
            if (component == null)
            {
                return;
            }
            builder.Append(component.useGenerator ? "Generator On" : "Generator Off");
        }
    }
}
