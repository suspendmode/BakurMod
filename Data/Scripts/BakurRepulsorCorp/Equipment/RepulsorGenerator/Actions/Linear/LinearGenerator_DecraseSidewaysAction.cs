using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class LinearGenerator_DecraseSidewaysAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_DecraseSidewaysAction() : base("LinearGenerator_DecraseSidewaysAction", "Decrase Sideways Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.05f;
            equipment.sideways -= step;
            equipment.sideways = MathHelper.Clamp(equipment.sideways, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.sideways * equipment.maxLinearAcceleration;
            builder.Append("-Side " + Math.Round(value, 1) + "m/s");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}