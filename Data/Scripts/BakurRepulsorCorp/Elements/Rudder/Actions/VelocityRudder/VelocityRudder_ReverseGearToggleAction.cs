
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class VelocityRudder_ReverseGearToggleAction : UIControlAction<VelocityRudder> {

        public VelocityRudder_ReverseGearToggleAction() : base("VelocityRudder_ReverseGearToggleAction", "Toggle Gear") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.reverseGear = !equipment.reverseGear;
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