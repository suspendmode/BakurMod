using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class ArtificialGravityGenerator_IncraseGravityAction : BaseControlAction<ArtificialGravityGenerator> {

        public ArtificialGravityGenerator_IncraseGravityAction() : base("ArtificialGravityGenerator_IncraseGravityAction", "Incrase Gravity") {
        }

        public override void Action(IMyTerminalBlock block) {
            ArtificialGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(ArtificialGravityGenerator_GravitySlider.minGravity, ArtificialGravityGenerator_GravitySlider.maxGravity, 0.05);
            equipment.gravity += step;
            equipment.gravity = MathHelper.Clamp(equipment.gravity, ArtificialGravityGenerator_GravitySlider.minGravity, ArtificialGravityGenerator_GravitySlider.maxGravity);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            ArtificialGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.gravity;
            builder.Append(Math.Round(value, 1) + "G");
        }
    }
}