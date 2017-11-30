using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseSidewaysDisableAction : UIControlAction<LinearInertialCompensator> {

        public Compensator_UseSidewaysDisableAction() : base("Compensator_UseSidewaysDisableAction", "Disable Use Sideways") {
        }

        public override void Action(IMyTerminalBlock block) {
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useSideways = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            LinearInertialCompensator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useSideways ? "Sideways On" : "Sideways Off");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            LinearInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useLinearCompensator;
        }
    }
}