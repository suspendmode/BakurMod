using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Stabiliser_DecraseStabilityAction : UIControlAction<AttitudeStabiliser> {

        public Stabiliser_DecraseStabilityAction() : base("TorqueStabiliser_DecraseStabilityAction", "Decrase Stability") {
        }

        public override void Action(IMyTerminalBlock block) {
            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Stabiliser_StabilitySlider.minimum, Stabiliser_StabilitySlider.maximum, 0.05);
            equipment.stability -= step;
            equipment.stability = MathHelper.Clamp(equipment.stability, Stabiliser_StabilitySlider.minimum, Stabiliser_StabilitySlider.maximum);

        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AttitudeStabiliser equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.stability;
            builder.Append("-Stb " + Math.Round(value, 1) + "°");
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