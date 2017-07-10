using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UpSlider : Slider<RepulsorLinearGenerator> {

        public static double minimum = -1;
        public static double maximum = 1;

        public LinearGenerator_UpSlider() : base("LinearGenerator_UpSlider", "Up", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.up * equipment.maxLinearAcceleration, 1) + " m/s");
        }

        protected override float GetValue(RepulsorLinearGenerator equipment) {
            return (float)equipment.up;
        }

        protected override void SetValue(RepulsorLinearGenerator equipment, float value) {
            equipment.up = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}