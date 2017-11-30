using Sandbox.ModAPI;
using System;
using System.Globalization;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Copter_SetModeFreeGlideAction : UIControlAction<Copter> {

        public Copter_SetModeFreeGlideAction() : base("Copter_SetModeFreeGlideAction", "Set Mode Free Glide") {
        }

        public override void Action(IMyTerminalBlock block) {
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.mode = Copter.FREE_GLIDE_MODE;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(ModeString(Copter.FREE_GLIDE_MODE));
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