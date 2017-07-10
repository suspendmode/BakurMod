using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Compensator_AngularDumpenerSlider : Slider<AngularInertialCompensator> {

        public Compensator_AngularDumpenerSlider() : base("Compensator_AngularDumpenerSlider", "Angular Dumpener", 0 + ".." + 1 + ")", 0, 1) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = BakurMathHelper.InverseLerp(0, 1, equipment.dumpener);
            double percentageValue = value * 100;
            builder.Append(Math.Round(percentageValue, 1) + " %");
        }

        protected override float GetValue(AngularInertialCompensator equipment) {
            return (float)equipment.dumpener;
        }

        protected override void SetValue(AngularInertialCompensator equipment, float value) {
            equipment.dumpener = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useAngularCompensator;
        }
    }
}