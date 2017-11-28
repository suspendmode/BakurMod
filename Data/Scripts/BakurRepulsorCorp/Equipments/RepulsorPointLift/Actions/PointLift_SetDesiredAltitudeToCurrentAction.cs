using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PointLift_SetDesiredAltitudeToCurrentAction : BaseControlAction<RepulsorPointLift> {

        public PointLift_SetDesiredAltitudeToCurrentAction() : base("PointLift_SetDesiredAltitudeToCurrentAction", "Set Current Altitude As Desired") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.normalizedAltitude = BakurMathHelper.InverseLerp(0, 100, equipment.altitude);
            equipment.desiredAltitude = MathHelper.Clamp(equipment.altitude, 0, 100);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append("Set " + Math.Round(equipment.altitude, 1) + "m");
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