using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Suspension_IncraseRestLengthAction : UIControlAction<RepulsorSuspension> {

        public Suspension_IncraseRestLengthAction() : base("RepulsorSuspension_IncraseRestLengthAction", "Incrase Rest Length") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Suspension_RestLengthSlider.minimum, Suspension_RestLengthSlider.maximum, 0.05);
            equipment.restLength += step;
            equipment.restLength = MathHelper.Clamp(equipment.restLength, Suspension_RestLengthSlider.minimum, Suspension_RestLengthSlider.maximum);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(Suspension_RestLengthSlider.minimum, Suspension_RestLengthSlider.maximum, equipment.restLength);
            builder.Append("+Rest " + Math.Round(value * 100, 0) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}