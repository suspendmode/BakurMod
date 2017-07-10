﻿using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class AngularGenerator_DecrasePitchAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_DecrasePitchAction() : base("AngularGenerator_DecrasePitchAction", "Decrase Pitch Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.05f;
            equipment.pitch -= step;
            equipment.pitch = MathHelper.Clamp(equipment.pitch, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.pitch * equipment.maxAngularAcceleration;
            builder.Append("-Pitch " + Math.Round(value, 1) + "°/s");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useAngularGenerator;
        }
    }
}