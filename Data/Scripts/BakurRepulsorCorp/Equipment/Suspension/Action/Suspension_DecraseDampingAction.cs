using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Suspension_DecraseDampingAction : BaseControlAction<RepulsorSuspension> {

        public Suspension_DecraseDampingAction() : base("RepulsorSuspension_DecraseDampingAction", "Decrase Damping") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Suspension_DampingSlider.minimum, Suspension_DampingSlider.maximum, 0.05);
            equipment.damping -= step;
            equipment.damping = MathHelper.Clamp(equipment.damping, Suspension_DampingSlider.minimum, Suspension_DampingSlider.maximum);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(Suspension_DampingSlider.minimum, Suspension_DampingSlider.maximum, equipment.damping);
            builder.Append("-Damp " + Math.Round(value, 1) + "N");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}