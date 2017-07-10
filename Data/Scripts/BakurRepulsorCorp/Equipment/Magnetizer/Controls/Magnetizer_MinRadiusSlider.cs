﻿using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class Magnetizer_MinRadiusSlider : Slider<Magnetizer> {

        public static double minRadius = 0;
        public static double maxRadius = 1000;

        public Magnetizer_MinRadiusSlider() : base("Magnetizer_MinRadiusSlider", "Min Radius", Math.Round(minRadius, 1) + ".." + Math.Round(maxRadius, 1), (float)minRadius, (float)maxRadius) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.minRadius, 1) + " m");
        }

        protected override float GetValue(Magnetizer equipment) {
            return (float)equipment.minRadius;
        }

        protected override void SetValue(Magnetizer equipment, float value) {
            equipment.minRadius = value;
        }
    }
}