using Sandbox.ModAPI;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;
using System;
using VRage.Utils;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallPlanetRadar", "LargePlanetRadar" })]
    public class PlanetRadarBlock : BakurBlock {

        PlanetAltitudeSensor planetAltitudeSensor;
        PlanetRadarSensor planetRadarSensor;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            Add(planetAltitudeSensor);

            planetRadarSensor = new PlanetRadarSensor(this);
            Add(planetRadarSensor);
        }


        protected override void Destroy() {

            base.Destroy();

            Remove(planetAltitudeSensor);
            planetAltitudeSensor = null;

            Remove(planetRadarSensor);
            planetRadarSensor = null;

        }

        #endregion

        #region update
        public Color backgroundColor = Color.LightSkyBlue;
        public Color poiColor = Color.DeepSkyBlue;

        public void DrawEmissive2() {
            if (!block.IsWorking) {
                block.SetEmissiveParts("Emissive2", new Color(255, 0, 0), 1);
            } else {
                block.SetEmissiveParts("Emissive2", new Color(0, 255, 0), 1);
            }
        }

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);
            if (planetAltitudeSensor.nearestPlanet == null) {
                block.SetEmissiveParts("Emissive2", Color.Black, 1);
                return;
            }

            block.SetEmissiveParts("Emissive2", backgroundColor, 1);

            Vector3D direction = planetAltitudeSensor.nearestPlanet.WorldMatrix.Translation - block.WorldMatrix.Translation;
            direction.Normalize();

            //blockPower.UpdatePower(physicsDeltaTime, updateDeltaTime);
            //MyAPIGateway.Utilities.ShowMessage("powerRatio", blockPower.powerRatio.ToString());
            float size = 0.5f;
            if (block.BlockDefinition.SubtypeId == "LargePlanetRadar") {
                size = 2.5f;
            }
            float radius = size / 3;
            Vector3D start = block.WorldMatrix.Translation + block.WorldMatrix.Up * size / 9;
            Vector3D startLine = start + (direction * radius / 2);
            Vector3D endLine = startLine + direction * (radius / 2);

            DebugDraw.DrawLine(startLine, endLine, poiColor, 0.025f);
            DebugDraw.PlanetRadarSphere(start, planetAltitudeSensor.nearestPlanet.WorldMatrix.Forward, planetAltitudeSensor.nearestPlanet.WorldMatrix.Up, radius, backgroundColor);
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Planet Radar ==");

            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "planet_radar_working_start", "planet_radar_working_loop", "planet_radar_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("e89e8ef9-418a-414b-8198-b8743a4bc275");
        }
    }

}
