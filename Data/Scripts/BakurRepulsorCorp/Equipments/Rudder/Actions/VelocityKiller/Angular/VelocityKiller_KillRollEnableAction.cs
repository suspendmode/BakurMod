
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillRollEnableAction : BaseControlAction<VelocityKiller> {

        public VelocityKiller_KillRollEnableAction() : base("VelocityKiller_KillRollEnableAction", "Enable Kill Roll") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.killUp = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityKiller component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(builder.Append("Roll " + (component.killRoll ? "On" : "Off")));
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useVelocityKiller;
        }
    }
}