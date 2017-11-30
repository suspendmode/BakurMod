using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_ZeroSidewaysAction : UIControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_ZeroSidewaysAction() : base("LinearGenerator_ZeroSidewaysAction", "Zero Sideways Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.sideways = 0;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.sideways * equipment.maxLinearAcceleration;
            builder.Append("Zero Side");
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useLinearGenerator;
        }
    }
}