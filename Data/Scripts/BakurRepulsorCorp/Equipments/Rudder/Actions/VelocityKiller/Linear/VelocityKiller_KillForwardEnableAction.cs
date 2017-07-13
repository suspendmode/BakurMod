
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillForwardEnableAction : BaseControlAction<VelocityKiller> {

        public VelocityKiller_KillForwardEnableAction() : base("VelocityKiller_KillForwardEnableAction", "Enable Kill Forward") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.killForward = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityKiller component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append("Fwd " + (component.killForward ? "On" : "Off"));
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