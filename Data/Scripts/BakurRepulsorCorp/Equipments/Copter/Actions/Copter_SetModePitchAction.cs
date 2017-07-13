using Sandbox.ModAPI;
using System;
using System.Globalization;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Copter_SetModePitchAction : BaseControlAction<Copter> {

        public Copter_SetModePitchAction() : base("Copter_SetModePitchAction", "Set Mode Pitch") {
        }

        public override void Action(IMyTerminalBlock block) {
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.mode = Copter.PITCH_MODE;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(ModeString(Copter.PITCH_MODE));
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