using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp
{

    public class Generator_PowerSlider : Slider<AntiGravityGenerator>
    {

        public Generator_PowerSlider() : base("Generator_PowerSlider", "Power", "0..1", 0, 1)
        {
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
            builder.Append(Math.Round(percentageValue, 1) + " %");
        }

        protected override float GetValue(AntiGravityGenerator equipment)
        {
            return (float)equipment.power;
        }

        protected override void SetValue(AntiGravityGenerator equipment, float value)
        {
            equipment.power = value;
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
