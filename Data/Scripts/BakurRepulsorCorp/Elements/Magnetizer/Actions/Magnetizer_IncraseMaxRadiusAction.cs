using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Magnetizer_IncraseMaxRadiusAction : UIControlAction<Magnetizer> {

        public Magnetizer_IncraseMaxRadiusAction() : base("Magnetizer_IncraseRadiusAction", "Incrase Max Radius") {
        }

        public override void Action(IMyTerminalBlock block) {
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(0, 7000, 0.05);
            equipment.maxRadius += step;
            equipment.maxRadius = MathHelper.Clamp(equipment.maxRadius, 0, 1);
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