using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class PlanetAltitudeSensor_PrecisionModeEnableAction : BaseControlAction<PlanetAltitudeSensor> {

        public PlanetAltitudeSensor_PrecisionModeEnableAction() : base("PlanetAltitudeSensor_PrecisionModeEnableAction", "Enable Precision Mode") {
        }

        public override void Action(IMyTerminalBlock block) {
            PlanetAltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.precisionMode = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            PlanetAltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append("Prec " + (equipment.precisionMode ? "On" : "Off"));
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