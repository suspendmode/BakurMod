using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PointLift_DecraseNormalizedDistanceAction : BaseControlAction<RepulsorPointLift> {

        public PointLift_DecraseNormalizedDistanceAction() : base("PointLift_DecraseNormalizedDistanceAction", "Decrase Normalized Distance") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.0005f;
            equipment.normalizedDistance -= step;
            equipment.normalizedDistance = BakurMathHelper.Clamp01(equipment.normalizedDistance);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }            
            builder.Append("-Dist " + Math.Round(equipment.distance, 1) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}