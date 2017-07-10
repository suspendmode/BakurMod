
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityRudder_UseVelocityRudderEnableAction : BaseControlAction<VelocityRudder> {

        public VelocityRudder_UseVelocityRudderEnableAction() : base("VelocityRudder_UseVelocityRudderEnableAction", "Enable Use Etheric Rudder") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useVelocityRudder = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityRudder component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append("Rudder " + (component.useVelocityRudder ? "On" : "Off")));
        }
    }
}