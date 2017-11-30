using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class AngularGenerator_ZeroPitchAction : UIControlAction<RepulsorAngularGenerator> {

        public AngularGenerator_ZeroPitchAction() : base("AngularGenerator_ZeroPitchAction", "Zero Pitch Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.pitch = 0;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorAngularGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.pitch;
            builder.Append("Zero Pitch");
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