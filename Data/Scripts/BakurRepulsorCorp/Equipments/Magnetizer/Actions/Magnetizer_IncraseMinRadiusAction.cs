using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Magnetizer_IncraseMinRadiusAction : BaseControlAction<Magnetizer> {

        public Magnetizer_IncraseMinRadiusAction() : base("Magnetizer_IncraseMinRadiusAction", "Incrase Min Radius") {
        }

        public override void Action(IMyTerminalBlock block) {
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Magnetizer_MinRadiusSlider.minRadius, Magnetizer_MinRadiusSlider.maxRadius, 0.05);
            equipment.minRadius += step;
            equipment.minRadius = MathHelper.Clamp(equipment.minRadius, Magnetizer_MinRadiusSlider.minRadius, Magnetizer_MinRadiusSlider.maxRadius);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.minRadius, 1) + "m");
        }
    }
}