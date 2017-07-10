﻿
using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityKiller_KillPitchEnableAction : BaseControlAction<VelocityKiller> {

        public VelocityKiller_KillPitchEnableAction() : base("VelocityKiller_KillPitchEnableAction", "Enable Kill Pitch") {
        }

        public override void Action(IMyTerminalBlock block) {
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.killPitch = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            VelocityKiller component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append("Ptch " + (component.killPitch ? "On" : "Off"));
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            VelocityKiller equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useVelocityKiller;
        }
    }
}