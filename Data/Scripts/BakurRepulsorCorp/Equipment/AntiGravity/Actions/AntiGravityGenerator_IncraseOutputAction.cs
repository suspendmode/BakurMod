using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class AntiGravityGenerator_IncraseOutputAction : BaseControlAction<AntiGravityGenerator> {

        public AntiGravityGenerator_IncraseOutputAction() : base("AntiGravityGenerator_IncraseOutputAction", "Incrase Gravity") {
        }

        public override void Action(IMyTerminalBlock block) {
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = 0.05;
            equipment.output += step;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AntiGravityGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.output;
            builder.Append(Math.Round(value*100, 1) + "%");
        }
    }
}