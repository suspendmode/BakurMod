using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_UseAngularGeneratorToggleAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_UseAngularGeneratorToggleAction() : base("AngularGenerator_UseAngularGeneratorToggleAction", "Use Angular Generator On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useAngularGenerator = !equipment.useAngularGenerator;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(equipment.useAngularGenerator ? "Ang On" : "Ang Off");
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}