using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class PlanetAltitudeSensor_PrecisionModeToggleAction : UIControlAction<PlanetAltitudeSensor> {

        public PlanetAltitudeSensor_PrecisionModeToggleAction() : base("PlanetAltitudeSensor_PrecisionModeToggleAction", "Toggle Precision Mode") {
        }

        public override void Action(IMyTerminalBlock block) {
            PlanetAltitudeSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.precisionMode = !equipment.precisionMode;
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
            return equipment.logicComponent.enabled;
        }
    }
}