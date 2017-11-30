using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PointLift_DecraseNormalizedAltitudeAction : UIControlAction<RepulsorPointLift> {

        public PointLift_DecraseNormalizedAltitudeAction() : base("PointLift_DecraseNormalizedAltitudeAction", "Decrase Normalized Altitude") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.0005f;
            equipment.normalizedAltitude -= step;
            equipment.normalizedAltitude = BakurMathHelper.Clamp01(equipment.normalizedAltitude);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }            
            builder.Append("-Dist " + Math.Round(equipment.altitude, 1) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}