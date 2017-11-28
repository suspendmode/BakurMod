using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityRudder_ReverseGearDisableAction : BaseControlAction<VelocityRudder> {

        public VelocityRudder_ReverseGearDisableAction() : base("VelocityRudder_ReverseGearDisableAction", "Forward Gear") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.reverseGear = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityRudder component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append("Gear " + (component.reverseGear ? "Reverse" : "Forward")));
        }
    }
}