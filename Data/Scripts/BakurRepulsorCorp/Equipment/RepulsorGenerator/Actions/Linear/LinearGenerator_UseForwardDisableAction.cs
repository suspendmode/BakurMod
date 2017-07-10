﻿using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_UseForwardDisableAction : BaseControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_UseForwardDisableAction() : base("LinearGenerator_UseForwardDisableAction", "Disable Use Forward") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useForward = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useForward ? "Fwd On" : "Fwd Off");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useLinearGenerator;
        }
    }
}