using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class LinearGenerator_ZeroForwardAction : UIControlAction<RepulsorLinearGenerator> {

        public LinearGenerator_ZeroForwardAction() : base("LinearGenerator_ZeroForwardAction", "Zero Forward Speed") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.forward = 0;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorLinearGenerator equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.forward * equipment.maxLinearAcceleration;
            builder.Append("Zero Fwd");
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