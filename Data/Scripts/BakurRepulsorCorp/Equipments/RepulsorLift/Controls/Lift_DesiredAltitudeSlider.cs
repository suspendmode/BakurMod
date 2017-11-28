using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp
{

    public class Lift_DesiredAltitudeSlider : Slider<RepulsorLift>
    {

        public Lift_DesiredAltitudeSlider() : base("Lift_DesiredAltitudeSlider", "Altitude", "0..Max", 0f, 1)
        {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            builder.Append(Math.Round(equipment.desiredAltitude * equipment.maxAltitude, 1) + " m");
        }

        protected override float GetValue(RepulsorLift equipment)
        {

            return (float)equipment.desiredAltitude;
        }

        protected override void SetValue(RepulsorLift equipment, float value)
        {
            equipment.desiredAltitude = value;
        }

        protected override bool Visible(IMyTerminalBlock block)
        {
            if (!base.Visible(block))
            {
                return false;
            }

            RepulsorLift equipment = GetEquipment(block);
            if (equipment == null)
            {
                return false;
            }
            return equipment.component.enabled && equipment.useLift;
        }
    }
}
