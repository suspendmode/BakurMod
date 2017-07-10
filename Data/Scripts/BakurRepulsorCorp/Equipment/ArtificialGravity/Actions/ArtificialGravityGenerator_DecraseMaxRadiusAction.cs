using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class ArtificialGravityGenerator_DecraseMaxRadiusAction : BaseControlAction<ArtificialGravityGenerator> {

        public ArtificialGravityGenerator_DecraseMaxRadiusAction() : base("ArtificialGravityGenerator_DecraseMaxRadiusAction", "Decrase Max Radius") {
        }

        public override void Action(IMyTerminalBlock block) {
            ArtificialGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Magnetizer_MaxRadiusSlider.minRadius, Magnetizer_MaxRadiusSlider.maxRadius, 0.05);
            equipment.maxRadius -= step;
            equipment.maxRadius = MathHelper.Clamp(equipment.maxRadius, Magnetizer_MaxRadiusSlider.minRadius, Magnetizer_MaxRadiusSlider.maxRadius);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            ArtificialGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.maxRadius, 1) + "m");
        }
    }
}