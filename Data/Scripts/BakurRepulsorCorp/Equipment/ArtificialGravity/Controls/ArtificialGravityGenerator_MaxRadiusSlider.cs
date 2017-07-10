using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class ArtificialGravityGenerator_MaxRadiusSlider : Slider<ArtificialGravityGenerator> {

        public static double minRadius = 0;
        public static double maxRadius = 1000;

        public ArtificialGravityGenerator_MaxRadiusSlider() : base("ArtificialGravityGenerator_MaxRadiusSlider", "Max Radius", Math.Round(minRadius, 1) + ".." + Math.Round(maxRadius, 1), (float)minRadius, (float)maxRadius) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            ArtificialGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.maxRadius, 1) + " m");
        }

        protected override float GetValue(ArtificialGravityGenerator equipment) {
            return (float)equipment.maxRadius;
        }

        protected override void SetValue(ArtificialGravityGenerator equipment, float value) {
            equipment.maxRadius = value;
        }
    }
}