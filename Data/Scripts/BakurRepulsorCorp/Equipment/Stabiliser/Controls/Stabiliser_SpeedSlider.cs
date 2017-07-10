using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Stabiliser_SpeedSlider : Slider<AttitudeStabiliser> {

        public static double minimum = 0.2;
        public static double maximum = 2;

        public Stabiliser_SpeedSlider() : base("Stabiliser_SpeedSlider", "Speed", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(minimum, maximum, equipment.speed);
            builder.Append(Math.Round(value * 100, 1) + " %");
        }

        protected override float GetValue(AttitudeStabiliser equipment) {
            return (float)equipment.speed;
        }

        protected override void SetValue(AttitudeStabiliser equipment, float value) {
            equipment.speed = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}