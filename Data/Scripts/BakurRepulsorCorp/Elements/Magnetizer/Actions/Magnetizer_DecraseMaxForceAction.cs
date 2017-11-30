using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp {

    public class Magnetizer_DecraseMaxForceAction : UIControlAction<Magnetizer> {

        public Magnetizer_DecraseMaxForceAction() : base("Magnetizer_DecraseMaxForceAction", "Decrase Max Force") {
        }

        public override void Action(IMyTerminalBlock block) {
            Magnetizer equipment = GetEquipment(block);           
            if (equipment == null) {
                return;
            }
            double step = MathHelper.Lerp(Magnetizer_MaxForceSlider.minForce, Magnetizer_MaxForceSlider.maxForce, 0.05);
            equipment.maxForce -= step;
            equipment.maxForce = MathHelper.Clamp(equipment.maxForce, Magnetizer_MaxForceSlider.minForce, Magnetizer_MaxForceSlider.maxForce);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            Magnetizer equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            double value = equipment.maxForce;
            builder.Append(Math.Round(value, 1) + "N");
        }
    }
}