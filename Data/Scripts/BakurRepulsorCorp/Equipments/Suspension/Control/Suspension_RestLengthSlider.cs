using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Suspension_RestLengthSlider : Slider<RepulsorSuspension> {

        public static double minimum = 0;
        public static double maximum = 100;

        public Suspension_RestLengthSlider() : base("RepulsorSuspension_RestLengthSlider", "Rest Length", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorSuspension equipment = GetEquipment(block);            
            if (equipment == null) {
                return;
            }            
            builder.Append(Math.Round(equipment.restLength, 1) + " m");
        }

        protected override float GetValue(RepulsorSuspension component) {
            return (float)component.restLength;
        }

        protected override void SetValue(RepulsorSuspension component, float value) {
            component.restLength = value;
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