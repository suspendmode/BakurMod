using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_LinearDumpenerSlider : Slider<LinearInertialCompensator> {

        public static double minTension = 0;
        public static double maxTension = 1;

        public Compensator_LinearDumpenerSlider() : base("Compensator_LinearDumpenerSlider", "Linear Dumpener", 0 + ".." + 1 + ")", 0, 1) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(0, 1, equipment.dumpener);
            double percentageValue = value * 100;
            builder.Append(Math.Round(percentageValue, 1) + " %");
        }

        protected override float GetValue(LinearInertialCompensator equipment) {
            return (float)equipment.dumpener;
        }

        protected override void SetValue(LinearInertialCompensator equipment, float value) {
            equipment.dumpener = value;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useLinearCompensator;
        }
    }
}