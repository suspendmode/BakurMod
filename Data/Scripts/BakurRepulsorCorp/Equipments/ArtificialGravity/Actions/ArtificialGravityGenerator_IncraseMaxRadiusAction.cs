using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class ArtificialGravityGenerator_IncraseMaxRadiusAction : BaseControlAction<ArtificialGravityGenerator> {

        public ArtificialGravityGenerator_IncraseMaxRadiusAction() : base("ArtificialGravityGenerator_IncraseMaxRadiusAction", "Incrase Max Radius") {
        }

        public override void Action(IMyTerminalBlock block) {
            ArtificialGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(0, 7000, 0.05);
            equipment.maxRadius += step;
            equipment.maxRadius = MathHelper.Clamp(equipment.maxRadius, 0, 1);
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