
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class AngularGenerator_UseRollToggleAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_UseRollToggleAction() : base("AngularGenerator_UseRollToggleAction", "Use Roll On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useRoll = !equipment.useRoll;
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