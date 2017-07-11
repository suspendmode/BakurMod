using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class AngularGenerator_IncraseRollAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_IncraseRollAction() : base("AngularGenerator_IncraseRollAction", "Incrase Roll Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.05f;
            equipment.roll += step;
            equipment.roll = MathHelper.Clamp(equipment.roll, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.roll;
            builder.Append("+Roll " + Math.Round(value, 1) + "°/s");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useAngularGenerator;
        }
    }
}