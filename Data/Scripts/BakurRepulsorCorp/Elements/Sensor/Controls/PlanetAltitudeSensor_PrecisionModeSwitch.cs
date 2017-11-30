using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class PlanetAltitudeSensor_PrecisionModeSwitch : Switch<PlanetAltitudeSensor> {

        public PlanetAltitudeSensor_PrecisionModeSwitch() : base("PlanetAltitudeSensor_PrecisionModeSwitch", "Precision Mode", "Precision Mode (false/true)", "On", "Off") {
        }

        protected override bool GetValue(PlanetAltitudeSensor equipment) {
            return equipment.precisionMode;
        }

        protected override void SetValue(PlanetAltitudeSensor equipment, bool value) {
            equipment.precisionMode = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PlanetAltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}