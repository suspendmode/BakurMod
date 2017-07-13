using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PowerCoupler_MaxForceSlider : Slider<PowerCoupling> {

        public static double minimum = 1;
        public static double maximum = 1000000;

        public PowerCoupler_MaxForceSlider() : base("PowerCoupler_MaxForceSlider", "Max Force", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {

        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            
            builder.Append(Math.Round(equipment.maxForce / 1000, 3) + " kN");
        }

        protected override float GetValue(PowerCoupling equipment) {
            return (float)equipment.maxForce;
        }

        protected override void SetValue(PowerCoupling equipment, float value) {
            equipment.maxForce = value;
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PowerCoupling equipment = GetEquipment(block);
            return equipment.component.enabled;
        }
    }
}