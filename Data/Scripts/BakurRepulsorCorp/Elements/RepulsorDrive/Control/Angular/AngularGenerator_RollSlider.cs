using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_RollSlider : Slider<RepulsorAngularGenerator> {

        public static double minimum = -1;
        public static double maximum = 1;

        public AngularGenerator_RollSlider() : base("AngularGenerator_RollSlider", "Roll", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);

            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.roll, 1) + " °/s");
        }

        protected override float GetValue(RepulsorAngularGenerator equipment) {
            return (float)equipment.roll;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, float value) {
            equipment.roll = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useAngularGenerator;
        }
    }
}