
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class VelocityKiller_UseVelocityKillerToggleAction : BaseControlAction<VelocityKiller> {

        public VelocityKiller_UseVelocityKillerToggleAction() : base("VelocityKiller_UseVelocityKillerToggleAction", "Use Velocity Killer On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useVelocityKiller = !equipment.useVelocityKiller;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityKiller component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append((component.useVelocityKiller ? "On" : "Off")));
        }
    }
}