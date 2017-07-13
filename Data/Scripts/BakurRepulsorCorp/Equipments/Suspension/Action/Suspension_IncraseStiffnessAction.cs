using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Suspension_IncraseStiffnessAction : BaseControlAction<RepulsorSuspension> {

        public Suspension_IncraseStiffnessAction() : base("RepulsorSuspension_IncraseStiffnessAction", "Incrase Stiffness") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorSuspension equipment = GetEquipment(block);            
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Suspension_StiffnessSlider.minimum, Suspension_StiffnessSlider.maximum, 0.05);
            equipment.stiffness += step;
            equipment.stiffness = MathHelper.Clamp(equipment.stiffness, Suspension_StiffnessSlider.minimum, Suspension_StiffnessSlider.maximum);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorSuspension equipment = GetEquipment(block);            
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(Suspension_StiffnessSlider.minimum, Suspension_StiffnessSlider.maximum, equipment.stiffness);
            builder.Append("+Stiff " + Math.Round(value , 1) + "N");
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