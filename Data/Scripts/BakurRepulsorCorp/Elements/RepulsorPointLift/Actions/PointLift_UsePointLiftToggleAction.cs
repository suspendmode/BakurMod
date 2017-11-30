using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class PointLift_UsePointLiftToggleAction : UIControlAction<RepulsorPointLift> {

        public PointLift_UsePointLiftToggleAction() : base("PointLift_UsePointLiftToggleAction", "Use Linear Generator On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.usePointLift = !equipment.usePointLift;
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
            return equipment.logicComponent.enabled;
        }
    }
}