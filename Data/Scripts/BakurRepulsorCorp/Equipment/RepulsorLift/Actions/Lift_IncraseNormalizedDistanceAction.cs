using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Lift_IncraseNormalizedDistanceAction : BaseControlAction<RepulsorLift> {

        public Lift_IncraseNormalizedDistanceAction() : base("RepulsorLift_IncraseNormalizedDistanceAction", "Incrase Normalized Distance") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.0005f;
            equipment.normalizedDistance += step;
            equipment.normalizedDistance = MathHelper.Clamp(equipment.normalizedDistance, 0, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.normalizedDistance * equipment.maxDistance;
            builder.Append("+Alt " + Math.Round(value, 1) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useLift;
        }
    }
}