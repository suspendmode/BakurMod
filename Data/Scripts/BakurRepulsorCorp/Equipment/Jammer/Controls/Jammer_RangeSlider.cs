using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Jammer_RangeSlider : Slider<RepulsorJammer> {

        public static double minRange = 0;
        public static double maxRange = 1000;

        public Jammer_RangeSlider() : base("Jammer_RangeSlider", "Range", Math.Round(minRange, 1) + ".." + Math.Round(maxRange, 1), (float)minRange, (float)maxRange) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorJammer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.range, 1) + " m");
        }

        protected override float GetValue(RepulsorJammer equipment) {
            return (float)equipment.range;
        }

        protected override void SetValue(RepulsorJammer equipment, float value) {
            equipment.range = value;
        }
    }
}