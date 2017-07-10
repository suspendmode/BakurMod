using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {
   
    public class Copter_ResponsivitySlider : Slider<Copter> {

        public Copter_ResponsivitySlider() : base("Copter_ResponsivitySlider", "Responsivity", "(0..10)", 0, 10) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.responsivity, 1) + " degrees");
        }

        protected override float GetValue(Copter equipment) {
            return (float)equipment.responsivity;
        }

        protected override void SetValue(Copter equipment, float value) {
            equipment.responsivity = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.component.enabled;
        }
    }
   
}