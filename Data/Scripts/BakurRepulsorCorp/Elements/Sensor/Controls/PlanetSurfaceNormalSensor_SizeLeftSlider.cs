using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class PlanetSurfaceNormalSensor_SizeLeftSlider : Slider<PlanetSurfaceNormalSensor> {

        public static double minValue = 0.5;
        public static double maxValue = 2.5;

        public PlanetSurfaceNormalSensor_SizeLeftSlider() : base("PlanetSurfaceNormalSensor_SizeLeftSlider", "Size Left", Math.Round(minValue, 1) + ".." + Math.Round(maxValue, 1), (float)minValue, (float)maxValue) {
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            PlanetSurfaceNormalSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(Math.Round(equipment.sizeLeft, 1));
        }

        protected override float GetValue(PlanetSurfaceNormalSensor equipment) {
            return (float)equipment.sizeLeft;
        }

        protected override void SetValue(PlanetSurfaceNormalSensor equipment, float value) {
            equipment.sizeLeft = value;
        }
        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            PlanetSurfaceNormalSensor equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled;
        }
    }
}