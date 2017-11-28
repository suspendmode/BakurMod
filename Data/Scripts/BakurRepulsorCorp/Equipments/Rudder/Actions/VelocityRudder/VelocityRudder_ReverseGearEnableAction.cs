
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityRudder_ReverseGearEnableAction : BaseControlAction<VelocityRudder> {

        public VelocityRudder_ReverseGearEnableAction() : base("VelocityRudder_ReverseGearEnableAction", "Reverse Gear") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.reverseGear = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityRudder component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append("Gear " + (component.reverseGear ? "Reverese" : "Forward")));
        }
    }
}