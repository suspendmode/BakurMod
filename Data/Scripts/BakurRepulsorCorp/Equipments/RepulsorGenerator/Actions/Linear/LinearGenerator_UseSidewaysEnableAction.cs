
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseSidewaysEnableAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_UseSidewaysEnableAction() : base("LinearGenerator_UseSidewaysEnableAction", "Enable Use Sideways") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useSideways = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useSideways ? "Sideways On" : "Sideways Off");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}