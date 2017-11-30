
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class VelocityKiller_KillForwardToggleAction : UIControlAction<VelocityKiller> {

        public VelocityKiller_KillForwardToggleAction() : base("VelocityKiller_KillForwardToggleAction", "Kill Forward On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.killForward = !equipment.killForward;
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