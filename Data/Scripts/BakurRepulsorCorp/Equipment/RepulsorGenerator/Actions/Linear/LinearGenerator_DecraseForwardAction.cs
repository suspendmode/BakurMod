using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class LinearGenerator_DecraseForwardAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_DecraseForwardAction() : base("LinearGenerator_DecraseForwardAction", "Decrase Forward Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.05f;
            equipment.forward -= step;
            equipment.forward = MathHelper.Clamp(equipment.forward, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.forward * equipment.maxLinearAcceleration;
            builder.Append("-Fwd " + Math.Round(value, 1) + "m/s");
        }
        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}