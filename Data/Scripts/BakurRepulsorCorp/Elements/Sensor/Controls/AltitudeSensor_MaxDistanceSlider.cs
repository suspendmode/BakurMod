using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class AltitudeSensor_MaxDistanceSlider : Slider<AltitudeSensor> {

        public static double minValue = 3;
        public static double maxValue = 100;

        public AltitudeSensor_MaxDistanceSlider() : base("AltitudeSensor_MaxDistanceSlider", "Max Distance", Math.Round(minValue, 1) + ".." + Math.Round(maxValue, 1), (float)minValue, (float)maxValue) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.maxDistance, 1));
        }

        protected override float GetValue(AltitudeSensor equipment) {
            return (float)equipment.maxDistance;
        }

        protected override void SetValue(AltitudeSensor equipment, float value) {
            equipment.maxDistance = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}