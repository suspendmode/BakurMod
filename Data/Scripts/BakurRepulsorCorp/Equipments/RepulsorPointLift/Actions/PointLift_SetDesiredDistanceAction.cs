using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PointLift_SetDesiredDistanceAction : BaseControlAction<RepulsorPointLift> {

        public PointLift_SetDesiredDistanceAction() : base("PointLift_SetDesiredDistanceAction", "Set Desired Distance") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorPointLift equipment = GetEquipment(block);           
            if (equipment == null) {
                return;
            }            
            equipment.normalizedDistance = BakurMathHelper.InverseLerp(0, 100, equipment.distance);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append("Set " + Math.Round(equipment.distance, 1) + "m");
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