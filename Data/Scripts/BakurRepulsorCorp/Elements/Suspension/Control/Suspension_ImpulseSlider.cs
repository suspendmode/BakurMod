using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Suspension_ImpulseSlider : Slider<RepulsorSuspension> {

        public static double minImpulse = 0.1;
        public static double maxImpulse = 1;

        public Suspension_ImpulseSlider() : base("Suspension_ImpulseSlider", "Impulse", Math.Round(minImpulse, 1) + ".." + Math.Round(maxImpulse, 1), (float)minImpulse, (float)maxImpulse) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorSuspension equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.impulse, 1));
        }

        protected override float GetValue(RepulsorSuspension equipment) {
            return (float)equipment.impulse;
        }

        protected override void SetValue(RepulsorSuspension equipment, float value) {
            equipment.impulse = value;
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