using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PointLift_NormalizedDistanceSlider : Slider<RepulsorPointLift> {

        public PointLift_NormalizedDistanceSlider() : base("PointLift_NormalizedDistanceSlider", "Distance", "0..100", 0f, 100f) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.normalizedDistance * 100, 1) + " m");
        }

        protected override float GetValue(RepulsorPointLift equipment) {
            return (float)equipment.normalizedDistance;
        }

        protected override void SetValue(RepulsorPointLift equipment, float value) {
            equipment.normalizedDistance = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}