using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PowerCoupler_PowerSlider : Slider<PowerCoupling> {

        public static double minimum = 0;
        public static double maximum = 1;

        public PowerCoupler_PowerSlider() : base("PowerCoupler_PowerSlider", "Power", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {

        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            
            builder.Append(Math.Round(equipment.power * 100, 1) + " %");
        }

        protected override float GetValue(PowerCoupling equipment) {
            return (float)equipment.power;
        }

        protected override void SetValue(PowerCoupling equipment, float value) {
            equipment.power = value;
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PowerCoupling equipment = GetEquipment(block);
            return equipment.logicComponent.enabled;
        }
    }
}