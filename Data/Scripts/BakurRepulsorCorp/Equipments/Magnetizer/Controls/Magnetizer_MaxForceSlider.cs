using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Magnetizer_MaxForceSlider : Slider<Magnetizer> {

        public static double minForce = 0;
        public static double maxForce = 100;

        public Magnetizer_MaxForceSlider() : base("Magnetizer_MaxForceSlider", "Maximum Force", Math.Round(minForce, 1) + ".." + Math.Round(maxForce, 1), (float)minForce, (float)maxForce) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.maxForce, 1) + " N");
        }

        protected override float GetValue(Magnetizer equipment) {
            return (float)equipment.maxForce;
        }

        protected override void SetValue(Magnetizer equipment, float value) {
            equipment.maxForce = value;
        }
    }
}