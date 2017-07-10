using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PlanetAltitudeSensor_AltitudeSmoothTimeSlider : Slider<PlanetAltitudeSensor> {

        public static double minSmoothTime = 0;
        public static double maxSmoothTime = 1;

        public PlanetAltitudeSensor_AltitudeSmoothTimeSlider() : base("PlanetAltitudeSensor_AltitudeSmoothTimeSlider", "Altitude Smooth Time", Math.Round(minSmoothTime, 1) + ".." + Math.Round(maxSmoothTime, 1), (float)minSmoothTime, (float)maxSmoothTime) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            PlanetAltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.altitudeSmoothTime * 100, 1) + " %");
        }

        protected override float GetValue(PlanetAltitudeSensor equipment) {
            return (float)equipment.altitudeSmoothTime;
        }

        protected override void SetValue(PlanetAltitudeSensor equipment, float value) {
            equipment.altitudeSmoothTime = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PlanetAltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}