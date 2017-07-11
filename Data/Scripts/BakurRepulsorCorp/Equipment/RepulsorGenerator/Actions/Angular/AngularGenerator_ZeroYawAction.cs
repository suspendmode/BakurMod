using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_ZeroYawAction : BaseControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_ZeroYawAction() : base("AngularGenerator_ZeroYawAction", "Zero Yaw Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.yaw = 0;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.yaw;
            builder.Append("Zero Yaw");
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