using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_PowerSlider : Slider<RepulsorAngularGenerator> {

        public AngularGenerator_PowerSlider() : base("AngularGenerator_PowerSlider", "Power", "0..1", 0, 1) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double percentageValue = equipment.power * 100;
            builder.Append(Math.Round(percentageValue, 1) + " %");
        }

        protected override float GetValue(RepulsorAngularGenerator equipment) {
            return (float)equipment.power;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, float value) {
            equipment.power = value;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.enabled && equipment.useAngularGenerator;
        }
    }
}