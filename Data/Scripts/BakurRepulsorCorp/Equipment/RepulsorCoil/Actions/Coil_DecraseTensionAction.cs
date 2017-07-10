using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Coil_DecraseTensionAction : BaseControlAction<RepulsorCoil> {

        public Coil_DecraseTensionAction() : base("Coil_DecraseTensionAction", "Decrase Tension") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Coil_TensionSlider.minTension, Coil_TensionSlider.maxTension, 0.01);
            equipment.tension -= step;
            equipment.tension = MathHelper.Clamp(equipment.tension, Coil_TensionSlider.minTension, Coil_TensionSlider.maxTension);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(Coil_TensionSlider.minTension, Coil_TensionSlider.maxTension, equipment.tension);
            double percentageValue = value * Coil_TensionSlider.maxTension * 100;
            builder.Append("-Tens " +Math.Round(percentageValue, 0) + "%");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useCoil;
        }
    }
}