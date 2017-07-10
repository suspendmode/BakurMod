using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PowerCoupler_IncraseDesiredDistanceAction : BaseControlAction<PowerCoupling> {

        public PowerCoupler_IncraseDesiredDistanceAction() : base("PowerCoupler_IncraseDesiredDistanceAction", "Incrase Desired Distance") {
        }

        public override void Action(IMyTerminalBlock block) {
            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(PowerCoupler_DesiredDistanceSlider.minimum, PowerCoupler_DesiredDistanceSlider.maximum, 0.05);
            equipment.desiredDistance += step;
            equipment.desiredDistance = MathHelper.Clamp(equipment.desiredDistance, PowerCoupler_DesiredDistanceSlider.minimum, PowerCoupler_DesiredDistanceSlider.maximum);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.desiredDistance;
            builder.Append("+Dist " + Math.Round(value, 1) + "m");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PowerCoupling equipment = GetEquipment(block);
            return equipment.component.enabled;
        }
    }
}