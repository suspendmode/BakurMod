using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Magnetizer_DecraseMaxRadiusAction : UIControlAction<Magnetizer> {

        public Magnetizer_DecraseMaxRadiusAction() : base("Magnetizer_DecraseRadiusAction", "Decrase Max Radius") {
        }

        public override void Action(IMyTerminalBlock block) {
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Magnetizer_MaxRadiusSlider.minRadius, Magnetizer_MaxRadiusSlider.maxRadius, 0.05);
            equipment.maxRadius -= step;
            equipment.maxRadius = MathHelper.Clamp(equipment.maxRadius, Magnetizer_MaxRadiusSlider.minRadius, Magnetizer_MaxRadiusSlider.maxRadius);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.maxRadius, 1) + "m");
        }
    }
}