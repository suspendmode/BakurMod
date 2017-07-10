using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Suspension_StiffnessSlider : Slider<RepulsorSuspension> {

        public static double minimum = 0;
        public static double maximum = 10;

        public Suspension_StiffnessSlider() : base("RepulsorSuspension_StiffnessSlider", "Stiffness", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }            
            builder.Append(Math.Round(equipment.stiffness, 1));
        }

        protected override float GetValue(RepulsorSuspension component) {
            return (float)component.stiffness;
        }

        protected override void SetValue(RepulsorSuspension component, float value) {
            component.stiffness = value;
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