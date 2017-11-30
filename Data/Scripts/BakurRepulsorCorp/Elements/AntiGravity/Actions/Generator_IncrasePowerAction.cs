using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class Generator_IncrasePowerAction : UIControlAction<AntiGravityGenerator>
    {

        public Generator_IncrasePowerAction() : base("Generator_IncrasePowerAction", "Incrase Power")
        {
        }

        public override void Action(IMyTerminalBlock block)
        {
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            double step = 0.01f;
            equipment.power += step;
            equipment.power = MathHelper.Clamp(equipment.power, 0, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            double percentageValue = equipment.power * 100;
            builder.Append("+ " + Math.Round(percentageValue, 0) + "%");
        }

        protected override bool Visible(IMyTerminalBlock block)
        {
            if (!base.Visible(block))
            {
                return false;
            }
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null)
            {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useGenerator;
        }
    }
}
