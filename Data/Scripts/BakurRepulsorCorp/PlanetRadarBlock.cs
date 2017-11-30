using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallPlanetRadar", "LargePlanetRadar" })]
    public class PlanetRadarBlock : BakurBlock
    {

        PlanetAltitudeSensor planetAltitudeSensor;
        PlanetRadarSensor planetRadarSensor;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddEquipment(planetAltitudeSensor);

            planetRadarSensor = new PlanetRadarSensor(this);
            AddEquipment(planetRadarSensor);
        }


        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(planetAltitudeSensor);
            planetAltitudeSensor = null;

            RemoveEquipment(planetRadarSensor);
            planetRadarSensor = null;

        }

        #endregion

        #region update
        public Color backgroundColor = Color.LightSkyBlue;
        public Color poiColor = Color.Orange;
        public Color axisColor = Color.BlueViolet;

        public void DrawEmissive2()
        {
            if (!block.IsWorking)
            {
                block.SetEmissiveParts("Emissive2", new Color(255, 0, 0), 1);
            }
            else
            {
                block.SetEmissiveParts("Emissive2", new Color(0, 255, 0), 1);
            }
        }

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);
            if (planetAltitudeSensor.nearestPlanet == null)
            {
                block.SetEmissiveParts("Emissive2", Color.Black, 1);
                return;
            }

            block.SetEmissiveParts("Emissive2", backgroundColor, 1);

            Vector3D direction = planetAltitudeSensor.nearestPlanet.WorldMatrix.Translation - block.WorldMatrix.Translation;
            direction.Normalize();

            //blockPower.UpdatePower(physicsDeltaTime, updateDeltaTime);
            //MyAPIGateway.Utilities.ShowMessage("powerRatio", blockPower.powerRatio.ToString());
            float size = 0.5f;
            if (block.BlockDefinition.SubtypeId == "LargePlanetRadar")
            {
                size = 2.5f;
            }
            float radius = size / 4;
            Vector3D start = block.WorldMatrix.Translation + block.WorldMatrix.Up * size / 9;
            Vector3D startLine = start + (direction * radius);
            Vector3D endLine = startLine + direction * (radius);

            DebugDraw.DrawBox(startLine, block.WorldMatrix.Forward, block.WorldMatrix.Up, Vector3D.One * 0.02f, Color.Red, 0.025f);
            DebugDraw.DrawBox(endLine, block.WorldMatrix.Forward, block.WorldMatrix.Up, Vector3D.One * 0.02f, Color.Red, 0.025f);
            DebugDraw.DrawLine(startLine, endLine, poiColor, 0.025f);
            DebugDraw.PlanetRadarSphere(start, planetAltitudeSensor.nearestPlanet.WorldMatrix.Forward, planetAltitudeSensor.nearestPlanet.WorldMatrix.Up, radius, backgroundColor);

            // planet axles

            Vector3D planetUp = planetAltitudeSensor.nearestPlanet.WorldMatrix.Up;
            Vector3D planetRight = planetAltitudeSensor.nearestPlanet.WorldMatrix.Right;
            Vector3D planetForward = planetAltitudeSensor.nearestPlanet.WorldMatrix.Forward;

            // x

            Vector3D planetAxisRightStart = start + (planetRight * radius);
            Vector3D planetAxisRightEnd = planetAxisRightStart + (planetRight * radius);
            Vector3D planetAxisLeftStart = start + (-planetRight * radius);
            Vector3D planetAxisLeftEnd = planetAxisLeftStart + (-planetRight * radius);

            // y

            Vector3D planetAxisUpStart = start + (planetUp * radius);
            Vector3D planetAxisUpEnd = planetAxisUpStart + (planetUp * radius);
            Vector3D planetAxisDownStart = start + (-planetUp * radius);
            Vector3D planetAxisDownEnd = planetAxisUpStart + (-planetUp * radius);

            // z

            Vector3D planetAxisForwardStart = start + (planetForward * radius);
            Vector3D planetAxisForwardEnd = planetAxisForwardStart + (planetForward * radius);
            Vector3D planetAxisBackwardStart = start + (-planetForward * radius);
            Vector3D planetAxisBackwardEnd = planetAxisBackwardStart + (-planetForward * radius);

            DebugDraw.DrawLine(planetAxisRightStart, planetAxisRightEnd, axisColor, 0.045f);
            DebugDraw.DrawLine(planetAxisLeftStart, planetAxisLeftEnd, axisColor, 0.045f);

            DebugDraw.DrawLine(planetAxisUpStart, planetAxisUpEnd, axisColor, 0.045f);
            DebugDraw.DrawLine(planetAxisDownStart, planetAxisDownEnd, axisColor, 0.045f);

            DebugDraw.DrawLine(planetAxisForwardStart, planetAxisForwardEnd, axisColor, 0.045f);
            DebugDraw.DrawLine(planetAxisBackwardStart, planetAxisBackwardEnd, axisColor, 0.045f);

        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
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

        protected override Guid blockGUID()
        {
            return new Guid("e89e8ef9-418a-414b-8198-b8743a4bc275");
        }
    }

}
