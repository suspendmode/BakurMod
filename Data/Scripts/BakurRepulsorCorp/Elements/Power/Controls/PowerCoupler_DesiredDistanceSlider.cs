using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PowerCoupler_DesiredDistanceSlider : Slider<PowerCoupling> {

        public static double minimum = 5;
        public static double maximum = 50;

        public PowerCoupler_DesiredDistanceSlider() : base("PowerCoupler_DesiredDistanceSlider", "Desired Distance", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }            
            builder.Append(Math.Round(equipment.desiredDistance, 1) + " m");
        }

        protected override float GetValue(PowerCoupling equipment) {
            return (float)equipment.desiredDistance;
        }

        protected override void SetValue(PowerCoupling equipment, float value) {
            equipment.desiredDistance = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PowerCoupling equipment = GetEquipment(block);
            return equipment.logicComponent.enabled;
        }
    }
}