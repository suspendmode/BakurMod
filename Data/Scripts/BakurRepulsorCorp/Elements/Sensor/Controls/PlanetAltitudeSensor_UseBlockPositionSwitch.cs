using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class PlanetAltitudeSensor_UseBlockPositionSwitch : Switch<PlanetAltitudeSensor> {

        public PlanetAltitudeSensor_UseBlockPositionSwitch() : base("PlanetAltitudeSensor_UseBlockPositionSwitch", "Use Block Position", "Use Block Position (false/true)", "On", "Off") {
        }

        protected override bool GetValue(PlanetAltitudeSensor equipment) {
            return equipment.useBlockPosition;
        }

        protected override void SetValue(PlanetAltitudeSensor equipment, bool value) {
            equipment.useBlockPosition = value;
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