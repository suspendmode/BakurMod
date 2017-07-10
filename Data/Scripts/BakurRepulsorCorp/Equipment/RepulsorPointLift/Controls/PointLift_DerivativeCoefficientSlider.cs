using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PointLift_DerivativeCoefficientSlider : Slider<RepulsorPointLift> {

        public static double minCoefficent = -1;
        public static double maxCoefficent = 1;

        public PointLift_DerivativeCoefficientSlider() : base("PointLift_DerivativeCoefficientSlider", "Derivative Coefficient", Math.Round(minCoefficent, 1) + ".." + Math.Round(maxCoefficent, 1), (float)minCoefficent, (float)maxCoefficent) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.derivativeCoefficient, 3));
        }

        protected override float GetValue(RepulsorPointLift equipment) {
            return (float)equipment.derivativeCoefficient;
        }

        protected override void SetValue(RepulsorPointLift equipment, float value) {
            equipment.derivativeCoefficient = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorPointLift equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
}