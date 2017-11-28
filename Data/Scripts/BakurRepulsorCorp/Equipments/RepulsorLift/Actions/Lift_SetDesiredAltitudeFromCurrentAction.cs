using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Lift_SetDesiredAltitudeFromCurrentAction : BaseControlAction<RepulsorLift> {

        public Lift_SetDesiredAltitudeFromCurrentAction() : base("Lift_SetDesiredAltitudeFromCurrentAction", "Set Altitude To Current") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.desiredAltitude = BakurMathHelper.InverseLerp(0, equipment.maxAltitude, equipment.altitude);            
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.desiredAltitude * equipment.maxAltitude;
            builder.Append("= " + Math.Round(value, 1) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}