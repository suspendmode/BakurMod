using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp
{

    public class Coil_PowerSlider : Slider<RepulsorCoil>
    {

        public Coil_PowerSlider() : base("Coil_PowerSlider", "Power", "0..1", 0, 1)
        {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            double percentageValue = equipment.power * 100;
            builder.Append(Math.Round(percentageValue, 1) + " %");
        }

        protected override float GetValue(RepulsorCoil equipment)
        {
            return (float)equipment.power;
        }

        protected override void SetValue(RepulsorCoil equipment, float value)
        {
            equipment.power = value;
        }

        protected override bool Visible(IMyTerminalBlock block)
        {
            if (!base.Visible(block))
            {
                return false;
            }
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null)
            {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useCoil;
        }
    }
}
