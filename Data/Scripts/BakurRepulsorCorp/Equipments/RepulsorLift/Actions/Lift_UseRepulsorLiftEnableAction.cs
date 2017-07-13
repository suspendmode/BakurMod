using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Lift_UseRepulsorLiftEnableAction : BaseControlAction<RepulsorLift> {

        public Lift_UseRepulsorLiftEnableAction() : base("Lift_UseRepulsorLiftEnableAction", "Enable Use Repulsor Lift") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useLift = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLift component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useLift ? "Lift On" : "Lift Off");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}