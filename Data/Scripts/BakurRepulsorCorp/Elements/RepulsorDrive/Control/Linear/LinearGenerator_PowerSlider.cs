using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp
{

    public class LinearGenerator_PowerSlider : Slider<RepulsorLinearGenerator>
    {

        public LinearGenerator_PowerSlider() : base("LinearGenerator_PowerSlider", "Power", "0..1", 0, 1)
        {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            double percentageValue = equipment.power * 100;
            builder.Append(Math.Round(percentageValue, 1) + " %");
        }

        protected override float GetValue(RepulsorLinearGenerator equipment)
        {
            return (float)equipment.power;
        }

        protected override void SetValue(RepulsorLinearGenerator equipment, float value)
        {
            equipment.power = value;
        }

        protected override bool Visible(IMyTerminalBlock block)
        {
            if (!base.Visible(block)) { return false; }
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null)
            {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useLinearGenerator;
        }
    }
}
