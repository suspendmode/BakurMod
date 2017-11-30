using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseLinearGeneratorEnableAction : UIControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_UseLinearGeneratorEnableAction() : base("LinearGenerator_UseLinearGeneratorEnableAction", "Enable Use Linear Generator") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useLinearGenerator = true;
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
            return equipment.logicComponent.enabled;
        }
    }
}