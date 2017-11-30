
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class VelocityRudder_UseVelocityRudderToggleAction : UIControlAction<VelocityRudder> {

        public VelocityRudder_UseVelocityRudderToggleAction() : base("VelocityRudder_UseVelocityRudderToggleAction", "Use Etheric Rudder On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useVelocityRudder = !equipment.useVelocityRudder;
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