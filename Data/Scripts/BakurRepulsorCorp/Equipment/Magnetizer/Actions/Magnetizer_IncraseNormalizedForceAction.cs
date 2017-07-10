using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Magnetizer_IncraseNormalizedForceAction : BaseControlAction<Magnetizer> {

        public Magnetizer_IncraseNormalizedForceAction() : base("Magnetizer_IncraseNormalizedForceAction", "Incrase Normalized Force") {
        }

        public override void Action(IMyTerminalBlock block) {
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            float step = 0.1f;
            equipment.normalizedForce += step;
            equipment.normalizedForce = MathHelper.Clamp(equipment.normalizedForce, -1, 1);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.normalizedForce * equipment.maxForce;
            builder.Append(Math.Round(value, 1) + "N");
        }
    }
}