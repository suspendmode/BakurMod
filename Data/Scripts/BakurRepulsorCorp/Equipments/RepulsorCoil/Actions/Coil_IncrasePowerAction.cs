using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Coil_IncrasePowerAction : BaseControlAction<RepulsorCoil> {

        public Coil_IncrasePowerAction() : base("Coil_IncrasePowerAction", "Incrase Power") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = 0.01f;
            equipment.power += step;
            equipment.power = MathHelper.Clamp(equipment.power, 0, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double percentageValue = equipment.power * 100;
            builder.Append("[+] " + Math.Round(percentageValue, 0) + "%");
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.enabled && equipment.useCoil;
        }
    }
}