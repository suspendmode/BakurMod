using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class ArtificialGravityGenerator_GravitySlider : Slider<ArtificialGravityGenerator> {

        public static double minGravity = -2;
        public static double maxGravity = 2;

        public ArtificialGravityGenerator_GravitySlider() : base("ArtificialGravityGenerator_GravitySlider", "Gravity", Math.Round(minGravity, 1) + ".." + Math.Round(maxGravity, 1), (float)minGravity, (float)maxGravity) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            ArtificialGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.gravity, 1) + " G");
        }

        protected override float GetValue(ArtificialGravityGenerator equipment) {
            return (float)equipment.gravity;
        }

        protected override void SetValue(ArtificialGravityGenerator equipment, float value) {
            equipment.gravity = value;
        }
    }
}