using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class Lift_UseRepulsorLiftToggleAction : BaseControlAction<RepulsorLift> {

        public Lift_UseRepulsorLiftToggleAction() : base("Lift_UseRepulsorLiftToggleAction", "Use Repulsor Lift On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useLift = !equipment.useLift;
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