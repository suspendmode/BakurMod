
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_UseRollEnableAction : UIControlAction<AngularInertialCompensator> {

        public Compensator_UseRollEnableAction() : base("Compensator_UseRollEnableAction", "Enable Use Roll") {
        }

        public override void Action(IMyTerminalBlock block) {
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useRoll = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AngularInertialCompensator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useRoll ? "Roll On" : "Roll Off");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useAngularCompensator;
        }
    }
}