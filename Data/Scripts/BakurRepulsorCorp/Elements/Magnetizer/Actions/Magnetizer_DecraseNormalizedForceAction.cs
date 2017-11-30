﻿using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Magnetizer_DecraseNormalizedForceAction : UIControlAction<Magnetizer> {

        public Magnetizer_DecraseNormalizedForceAction() : base("Magnetizer_DecraseNormalizedForceAction", "Decrase Normalized Force") {
        }

        public override void Action(IMyTerminalBlock block) {
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.1f;
            equipment.normalizedForce -= step;
            equipment.normalizedForce = MathHelper.Clamp(equipment.normalizedForce, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.normalizedForce * equipment.maxForce;
            builder.Append(Math.Round(value, 1) + "N");
        }
    }
}