using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_UsePitchDisableAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_UsePitchDisableAction() : base("AngularGenerator_UsePitchDisableAction", "Disable Use Pitch") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.usePitch = false;
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