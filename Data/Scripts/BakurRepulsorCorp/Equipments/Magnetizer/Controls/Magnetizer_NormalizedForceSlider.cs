using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Magnetizer_NormalizedForceSlider : Slider<Magnetizer> {

        public Magnetizer_NormalizedForceSlider() : base("Magnetizer_NormalizedForceSlider", "Force", "-1..1", -1f, 1f) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.normalizedForce * equipment.maxForce, 1) + " N");
        }

        protected override float GetValue(Magnetizer equipment) {
            return (float)equipment.normalizedForce;
        }

        protected override void SetValue(Magnetizer equipment, float value) {
            equipment.normalizedForce = value;
        }
    }
}