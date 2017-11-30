
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class Compensator_UsePitchToggleAction : UIControlAction<AngularInertialCompensator> {

        public Compensator_UsePitchToggleAction() : base("Compensator_UsePitchToggleAction", "Use Pitch On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.usePitch = !equipment.usePitch;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AngularInertialCompensator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.usePitch ? "Pitch On" : "Pitch Off");
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