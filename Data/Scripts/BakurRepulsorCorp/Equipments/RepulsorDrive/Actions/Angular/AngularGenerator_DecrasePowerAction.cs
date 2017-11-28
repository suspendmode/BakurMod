using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class AngularGenerator_DecrasePowerAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_DecrasePowerAction() : base("AngularGenerator_DecrasePowerAction", "Decrase Power") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double step = 0.01f;
            equipment.power -= step;
            equipment.power = MathHelper.Clamp(equipment.power, 0, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
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
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.enabled && equipment.useAngularGenerator;
        }
    }
}