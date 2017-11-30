using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class LinearGenerator_DecraseUpAction : UIControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_DecraseUpAction() : base("LinearGenerator_DecraseUpAction", "Decrase Up Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.05f;
            equipment.up -= step;
            equipment.up = MathHelper.Clamp(equipment.up, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.up * equipment.maxLinearAcceleration;
            builder.Append("-Up " + Math.Round(value, 1) + "m/s");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useLinearGenerator;
        }
    }
}