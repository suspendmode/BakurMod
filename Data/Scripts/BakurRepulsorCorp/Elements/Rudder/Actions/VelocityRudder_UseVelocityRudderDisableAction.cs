using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityRudder_UseVelocityRudderDisableAction : UIControlAction<VelocityRudder> {

        public VelocityRudder_UseVelocityRudderDisableAction() : base("VelocityRudder_UseVelocityRudderDisableAction", "Disable Use Etheric Rudder") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useVelocityRudder = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityRudder component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append((component.useVelocityRudder ? "On" : "Off")));
        }
    }
}