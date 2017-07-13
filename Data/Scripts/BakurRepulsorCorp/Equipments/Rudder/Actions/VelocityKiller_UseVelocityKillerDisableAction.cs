using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityKiller_UseVelocityKillerDisableAction : BaseControlAction<VelocityKiller> {

        public VelocityKiller_UseVelocityKillerDisableAction() : base("VelocityKiller_UseVelocityKillerDisableAction", "Disable Use Velocity Killer") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useVelocityKiller = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityKiller component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append("Vel " + (component.useVelocityKiller ? "On" : "Off")));
        }
    }
}