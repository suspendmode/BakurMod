using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {
   
    public class Copter_MaxRollAngleSlider : Slider<Copter> {

        public Copter_MaxRollAngleSlider() : base("Copter_MaxRollAngleSlider", "Max Roll Angle", "(0..90)", 0, 90) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.maxRoll, 1) + " degrees");
        }

        protected override float GetValue(Copter equipment) {
            return (float)equipment.maxRoll;
        }

        protected override void SetValue(Copter equipment, float value) {
            equipment.maxRoll = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled && equipment.useCopter;
        }
    }
   
}