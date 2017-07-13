
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class AngularGenerator_UsePitchToggleAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_UsePitchToggleAction() : base("AngularGenerator_UsePitchToggleAction", "Use Pitch On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.usePitch = !equipment.usePitch;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.usePitch ? "Pitch On" : "Pitch Off");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useAngularGenerator;
        }
    }
}