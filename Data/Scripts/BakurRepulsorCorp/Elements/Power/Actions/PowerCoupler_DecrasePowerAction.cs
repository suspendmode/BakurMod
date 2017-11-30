using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PowerCoupler_DecrasePowerAction : UIControlAction<PowerCoupling> {

        public PowerCoupler_DecrasePowerAction() : base("PowerCoupler_DecrasePowerAction", "Decrase Power") {
        }

        public override void Action(IMyTerminalBlock block) {
            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(PowerCoupler_PowerSlider.minimum, PowerCoupler_PowerSlider.maximum, 0.05);
            equipment.power -= step;
            equipment.power = MathHelper.Clamp(equipment.power, PowerCoupler_PowerSlider.minimum, PowerCoupler_PowerSlider.maximum);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            PowerCoupling equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.power;
            builder.Append("-Power " + Math.Round(value, 1) + " %");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PowerCoupling equipment = GetEquipment(block);
            return equipment.logicComponent.enabled;
        }
    }
}