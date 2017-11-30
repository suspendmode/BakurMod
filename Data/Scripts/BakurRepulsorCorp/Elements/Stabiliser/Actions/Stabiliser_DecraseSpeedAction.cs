using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Stabiliser_DecraseSpeedAction : UIControlAction<AttitudeStabiliser> {

        public Stabiliser_DecraseSpeedAction() : base("TorqueStabiliser_DecraseSpeedAction", "Decrase Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Stabiliser_SpeedSlider.minimum, Stabiliser_SpeedSlider.maximum, 0.05);            
            equipment.speed -= step;
            equipment.speed = MathHelper.Clamp(equipment.speed, Stabiliser_SpeedSlider.minimum, Stabiliser_SpeedSlider.maximum);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.speed;
            builder.Append("-Spd " + Math.Round(value, 1) + "°/s");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}