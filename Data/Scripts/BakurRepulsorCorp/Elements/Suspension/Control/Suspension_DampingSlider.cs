using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Suspension_DampingSlider : Slider<RepulsorSuspension> {
        public static double minimum = 0;
        public static double maximum = 100;

        public Suspension_DampingSlider() : base("RepulsorSuspension_DampingSlider", "Damping", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.damping, 1));
        }

        protected override float GetValue(RepulsorSuspension equipment) {
            return (float)equipment.damping;
        }

        protected override void SetValue(RepulsorSuspension equipment, float value) {
            equipment.damping = value;
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