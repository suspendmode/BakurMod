using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Lift_SetDesiredDistanceToCurrentAction : BaseControlAction<RepulsorLift> {

        public Lift_SetDesiredDistanceToCurrentAction() : base("Lift_SetDesiredDistanceToCurrentAction", "Set Current As Desired Distance") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.normalizedDistance = BakurMathHelper.InverseLerp(0, equipment.maxDistance, equipment.distance);
            equipment.desiredDistance = MathHelper.Clamp(equipment.distance, 0, equipment.maxDistance);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.normalizedDistance * equipment.maxDistance;
            builder.Append("Set " + Math.Round(value, 1) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}