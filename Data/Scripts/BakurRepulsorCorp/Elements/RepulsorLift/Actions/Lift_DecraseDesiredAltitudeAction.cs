using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Lift_DecraseDesiredAltitudeAction : UIControlAction<RepulsorLift> {

        public Lift_DecraseDesiredAltitudeAction() : base("Lift_DecraseDesiredAltitudeAction", "Decrase Altitude") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = equipment.maxAltitude / 1000;
            equipment.desiredAltitude -= 0.01f;
            equipment.desiredAltitude = MathHelper.Clamp(equipment.desiredAltitude, 0, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.desiredAltitude;
            builder.Append("- " + Math.Round(value, 1) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useLift;
        }
    }
}