using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class AngularGenerator_DecraseYawAction : UIControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_DecraseYawAction() : base("AngularGenerator_DecraseYawAction", "Decrase Yaw Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.05f;
            equipment.yaw -= step;
            equipment.yaw = MathHelper.Clamp(equipment.yaw, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.yaw;
            builder.Append("-Yaw " + Math.Round(value, 1) + "°/s");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useAngularGenerator;
        }
    }
}