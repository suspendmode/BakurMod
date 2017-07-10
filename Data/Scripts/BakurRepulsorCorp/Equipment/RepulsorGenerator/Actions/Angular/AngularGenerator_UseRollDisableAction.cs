using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_UseRollDisableAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_UseRollDisableAction() : base("AngularGenerator_UseRollDisableAction", "Disable Use Roll") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useRoll = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useRoll ? "Roll On" : "Roll Off");
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