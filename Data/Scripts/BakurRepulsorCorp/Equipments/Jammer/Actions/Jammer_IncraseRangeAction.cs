using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Jammer_IncraseRangeAction : BaseControlAction<RepulsorJammer> {

        public Jammer_IncraseRangeAction() : base("Jammer_IncraseRangeAction", "Incrase Range") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorJammer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Jammer_RangeSlider.minRange, Jammer_RangeSlider.maxRange, 0.05);
            equipment.range += step;
            equipment.range = MathHelper.Clamp(equipment.range, Jammer_RangeSlider.minRange, Jammer_RangeSlider.maxRange);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorJammer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.range;
            builder.Append(Math.Round(value, 1) + "m");
        }
    }
}