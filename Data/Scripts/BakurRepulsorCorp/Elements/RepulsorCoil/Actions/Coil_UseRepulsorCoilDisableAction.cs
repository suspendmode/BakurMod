﻿using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Coil_UseCoilDisableAction : UIControlAction<RepulsorCoil> {

        public Coil_UseCoilDisableAction() : base("Coil_UseCoilDisableAction", "Disable Use Coil") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useCoil = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorCoil component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useCoil ? "Coil On" : "Coil Off");
        }
    }
}