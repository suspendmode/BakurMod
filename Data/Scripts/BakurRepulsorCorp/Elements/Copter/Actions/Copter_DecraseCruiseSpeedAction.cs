using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class Copter_DecraseCruiseSpeedAction : UIControlAction<Copter>
    {

        public Copter_DecraseCruiseSpeedAction() : base("Copter_DecraseCruiseSpeedAction", "Decrase Cruise Speed")
        {
        }

        public override void Action(IMyTerminalBlock block)
        {
            Copter equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            double step = MathHelper.Lerp(Copter.cruiseSpeedSlider.min, Copter.cruiseSpeedSlider.max, 0.01);
            equipment.cruiseSpeed -= step;
            equipment.cruiseSpeed = MathHelper.Clamp(equipment.cruiseSpeed, Copter.cruiseSpeedSlider.min, Copter.cruiseSpeedSlider.max);
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder)
        {
            builder.Clear();
            Copter equipment = GetEquipment(block);
            if (equipment == null)
            {
                return;
            }
            double cruiseSpeed = equipment.cruiseSpeed;
            builder.Append("[-] " + Math.Round(cruiseSpeed, 1) + "m/s");
        }

        protected override bool Visible(IMyTerminalBlock block)
        {
            if (!base.Visible(block))
            {
                return false;
            }
            Copter equipment = GetEquipment(block);
            if (equipment == null)
            {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useCopter;
        }
    }
}
