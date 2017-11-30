using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PowerCoupler_IncraseMaxForceAction : UIControlAction<PowerCoupling> {

        public PowerCoupler_IncraseMaxForceAction() : base("PowerCoupler_IncraseMaxForceAction", "Incrase Max Force") {
        }

        public override void Action(IMyTerminalBlock block) {
            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(PowerCoupler_MaxForceSlider.minimum, PowerCoupler_MaxForceSlider.maximum, 0.1);
            equipment.maxForce += step;
            equipment.maxForce = MathHelper.Clamp(equipment.maxForce, PowerCoupler_MaxForceSlider.minimum, PowerCoupler_MaxForceSlider.maximum);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.maxForce;
            builder.Append("+Force " + Math.Round(value / 1000, 3) + " kN");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PowerCoupling equipment = GetEquipment(block);
            return equipment.logicComponent.enabled;
        }
    }
}