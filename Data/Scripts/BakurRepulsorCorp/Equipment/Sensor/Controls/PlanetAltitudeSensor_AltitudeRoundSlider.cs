using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PlanetAltitudeSensor_AltitudeRoundSlider : Slider<PlanetAltitudeSensor> {

        public static double minValue = 0;
        public static double maxValue = 10;

        public PlanetAltitudeSensor_AltitudeRoundSlider() : base("PlanetAltitudeSensor_AltitudeRoundSlider", "Altitude Round", Math.Round(minValue, 1) + ".." + Math.Round(maxValue, 1), (float)minValue, (float)maxValue) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            PlanetAltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.altitudeRound, 3));
        }

        protected override float GetValue(PlanetAltitudeSensor equipment) {
            return (float)equipment.altitudeRound;
        }

        protected override void SetValue(PlanetAltitudeSensor equipment, float value) {
            equipment.altitudeRound = value;
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