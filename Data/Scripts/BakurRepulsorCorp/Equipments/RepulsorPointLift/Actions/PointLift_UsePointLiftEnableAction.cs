using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class PointLift_UsePointLiftEnableAction : BaseControlAction<RepulsorPointLift> {

        public PointLift_UsePointLiftEnableAction() : base("PointLift_UsePointLiftEnableAction", "Enable Use Linear Generator") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.usePointLift = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorPointLift component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.usePointLift ? "Lin On" : "Lin Off");
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}