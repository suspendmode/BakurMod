using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Lift_NormalizedDistanceSlider : Slider<RepulsorLift> {

        public Lift_NormalizedDistanceSlider() : base("RepulsorLift_NormalizedDistance", "Distance", "0..1", 0f, 1f) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.normalizedDistance * equipment.maxDistance, 1) + " m");
        }

        protected override float GetValue(RepulsorLift equipment) {
            return (float)equipment.normalizedDistance;
        }

        protected override void SetValue(RepulsorLift equipment, float value) {
            equipment.normalizedDistance = value;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }

            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLift;
        }
    }
}