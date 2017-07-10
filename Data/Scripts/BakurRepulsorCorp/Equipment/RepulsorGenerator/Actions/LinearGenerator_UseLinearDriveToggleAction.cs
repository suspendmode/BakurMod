using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class LinearGenerator_UseLinearGeneratorToggleAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_UseLinearGeneratorToggleAction() : base("LinearGenerator_UseLinearGeneratorToggleAction", "Use Linear Generator On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useLinearGenerator = !equipment.useLinearGenerator;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useLinearGenerator ? "Lin On" : "Lin Off");
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}