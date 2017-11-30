using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Utils;

namespace BakurRepulsorCorp {

    public class Copter_CruiseSpeedSlider : Slider<Copter> {

        public Copter_CruiseSpeedSlider() : base("Copter_CruiseSpeedSlider", "Cruise Speed", "(0..100)", 0, 100) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.cruiseSpeed, 1) + " m/s");
        }

        protected override float GetValue(Copter equipment) {
            return (float)equipment.cruiseSpeed;
        }

        protected override void SetValue(Copter equipment, float value) {
            equipment.cruiseSpeed = value;
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useCopter;
        }
    }

}