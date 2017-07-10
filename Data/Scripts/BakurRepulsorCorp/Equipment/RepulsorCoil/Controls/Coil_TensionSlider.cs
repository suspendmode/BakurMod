using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Coil_TensionSlider : Slider<RepulsorCoil> {

        public static double minTension = 0;
        public static double maxTension = 2;

        public Coil_TensionSlider() : base("Coil_TensionSlider", "Tension", Math.Round(minTension, 1) + ".." + Math.Round(maxTension, 1) + ")", (float)minTension, (float)maxTension) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(minTension, maxTension, equipment.tension);
            double percentageValue = value * 100;
            builder.Append(Math.Round(percentageValue, 1) + " %");
        }

        protected override float GetValue(RepulsorCoil equipment) {
            return (float)equipment.tension;
        }

        protected override void SetValue(RepulsorCoil equipment, float value) {
            equipment.tension = value;
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