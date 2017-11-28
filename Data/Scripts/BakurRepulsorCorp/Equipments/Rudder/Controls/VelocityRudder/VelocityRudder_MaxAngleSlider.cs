using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class VelocityRudder_MaxAngleSlider : Slider<VelocityRudder> {

        public static float minAngle = 1;
        public static float maxAngle = 45;

        public VelocityRudder_MaxAngleSlider() : base("VelocityRudder_MaxAngleSlider", "Max Angle", Math.Round(minAngle, 1) + ".." + Math.Round(maxAngle, 1), (float)minAngle, (float)maxAngle) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();

            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.maxAngle, 1) + " degrees");
        }

        protected override float GetValue(VelocityRudder equipment) {
            return (float)equipment.maxAngle;
        }

        protected override void SetValue(VelocityRudder equipment, float value) {
            equipment.maxAngle = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            VelocityRudder equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useVelocityRudder;
        }
    }

}