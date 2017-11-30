
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class VelocityKiller_KillRollToggleAction : UIControlAction<VelocityKiller> {

        public VelocityKiller_KillRollToggleAction() : base("VelocityKiller_KillRollToggleAction", "Kill Roll On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.killUp = !equipment.killUp;
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