using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PowerCoupler_FrequencyTextBox : Textbox<PowerCoupling> {

        public PowerCoupler_FrequencyTextBox() : base("PowerCoupler_FrequencyTextBox", "Frequency", "Beam Frequency") {

        }
        
        protected override StringBuilder GetValue(PowerCoupling equipment) {
            return new StringBuilder(equipment.frequency);
        }

        protected override void SetValue(PowerCoupling equipment, StringBuilder value) {
            
            equipment.frequency = value.ToString();
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PowerCoupling equipment = GetEquipment(block);
            return equipment.logicComponent.enabled;
        }
    }
}