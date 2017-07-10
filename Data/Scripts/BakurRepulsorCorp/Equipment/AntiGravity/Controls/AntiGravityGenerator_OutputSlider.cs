using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class AntiGravityGenerator_OutputSlider : Slider<AntiGravityGenerator> {

        public static double minOutput = 0;
        public static double maxOutput = 1;

        public AntiGravityGenerator_OutputSlider() : base("AntiGravityGenerator_OutputSlider", "Output", Math.Round(minOutput, 1) + ".." + Math.Round(maxOutput, 1), (float)minOutput, (float)maxOutput) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.output * 100, 1) + " %");
        }

        protected override float GetValue(AntiGravityGenerator equipment) {
            return (float)equipment.output;
        }

        protected override void SetValue(AntiGravityGenerator equipment, float value) {
            equipment.output = value;
        }
    }
}