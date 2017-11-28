
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityKiller_UseVelocityKillerEnableAction : BaseControlAction<VelocityKiller> {

        public VelocityKiller_UseVelocityKillerEnableAction() : base("VelocityKiller_UseVelocityKillerEnableAction", "Enable Use Velocity Killer") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useVelocityKiller = true;
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