﻿using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class VelocityRudder_DecraseMaxAngleAction : UIControlAction<VelocityRudder> {

        public VelocityRudder_DecraseMaxAngleAction() : base("VelocityRudder_DecraseMaxAngleAction", "Decrase Max Angle") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(VelocityRudder_MaxAngleSlider.minAngle, VelocityRudder_MaxAngleSlider.maxAngle, 0.025);
            equipment.maxAngle -= step;
            equipment.maxAngle = MathHelper.Clamp(equipment.maxAngle, VelocityRudder_MaxAngleSlider.minAngle, VelocityRudder_MaxAngleSlider.maxAngle);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.maxAngle;
            builder.Append("-Max " + Math.Round(value * 100, 1) + " %");
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useVelocityRudder;
        }
    }
}