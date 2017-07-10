using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_PitchSlider : Slider<RepulsorAngularGenerator> {

        public static double minimum = -1;
        public static double maximum = 1;

        public AngularGenerator_PitchSlider() : base("AngularGenerator_PitchSlider", "Pitch", Math.Round(minimum, 1) + ".." + Math.Round(maximum, 1), (float)minimum, (float)maximum) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.pitch * equipment.maxAngularAcceleration, 1) + " °/s");
        }

        protected override float GetValue(RepulsorAngularGenerator equipment) {
            return (float)equipment.pitch;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, float value) {
            equipment.pitch = value;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useAngularGenerator;
        }
    }
}