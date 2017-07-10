
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class AngularGenerator_UseYawToggleAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_UseYawToggleAction() : base("AngularGenerator_UseYawToggleAction", "Use Yaw On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useYaw = !equipment.useYaw;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useYaw ? "Yaw On" : "Yaw Off");
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