using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Compensator_IncraseLinearDumpenerAction : UIControlAction<LinearInertialCompensator> {

        public Compensator_IncraseLinearDumpenerAction() : base("Compensator_IncraseLinearDumpenerAction", "Incrase Linear Dumpener") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(0, 1, 0.01);
            equipment.dumpener += step;
            equipment.dumpener = MathHelper.Clamp(equipment.dumpener, 0, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double percentageValue = equipment.dumpener * 100;
            builder.Append("+Lin " + Math.Round(percentageValue, 0) + "%");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useLinearCompensator;
        }
    }
}