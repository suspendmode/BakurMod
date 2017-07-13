using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Stabiliser_StabilitySlider : Slider<AttitudeStabiliser> {

        public static double minimum = 0.2;
        public static double maximum = 0.9;

        public Stabiliser_StabilitySlider() : base("Stabiliser_StabilitySlider", "Stability", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {

        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }

            double value = BakurMathHelper.InverseLerp(minimum, maximum, equipment.stability);
            builder.Append(Math.Round((1 - value) * 100, 1) + " %");
        }

        protected override float GetValue(AttitudeStabiliser equipment) {
            return (float)equipment.stability;
        }

        protected override void SetValue(AttitudeStabiliser equipment, float value) {
            equipment.stability = value;
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