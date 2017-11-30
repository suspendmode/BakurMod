using Sandbox.ModAPI;
using System;
using System.Globalization;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Copter_SetModeHoverAction : UIControlAction<Copter> {

        public Copter_SetModeHoverAction() : base("Copter_SetModeHoverAction", "Set Mode Hover") {
        }

        public override void Action(IMyTerminalBlock block) {
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.mode = Copter.HOVER_MODE;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(ModeString(Copter.HOVER_MODE));
        }

        string ModeString(string mode) {
            return char.ToUpper(mode[0]) + mode.Substring(1);
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useCopter;
        }
    }
}