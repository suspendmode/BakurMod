using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class PlanetAltitudeSensor : PlanetSensor
    {

        public double altitude = double.NaN;

        #region precision mode

        public readonly string PRECISION_MODE_PROPERTY_NAME = "PlanetAltitudeSensor_PrecisionMode";

        public bool defaultPrecisionMode = true;

        public bool precisionMode
        {
            set
            {
                string id = GeneratePropertyId(PRECISION_MODE_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(PRECISION_MODE_PROPERTY_NAME);
                bool result = defaultPrecisionMode;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultPrecisionMode;
            }
        }

        #endregion

        #region use block position

        public readonly string USE_BLOCK_POSITION_PROPERTY_NAME = "PlanetAltitudeSensor_UseBlockPosition";

        public bool defaultUseBlockPosition = false;

        public bool useBlockPosition
        {
            set
            {
                string id = GeneratePropertyId(USE_BLOCK_POSITION_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_BLOCK_POSITION_PROPERTY_NAME);
                bool result = defaultUseBlockPosition;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseBlockPosition;
            }
        }

        #endregion       

        public PlanetAltitudeSensor(LogicComponent component) : base(component)
        {
        }

        #region lifecycle

        public override void Initialize()
        {


        }

        public override void Destroy()
        {
        }

        #endregion

        public override void UpdateSensor(double physicsDeltaTime)
        {
            base.UpdateSensor(physicsDeltaTime);

            if (!logicComponent.rigidbody.IsInGravity || nearestPlanet == null)
            {
                altitude = double.NaN;
                return;
            }

            double newAltitude = 0;

            if (precisionMode)
            {
                newAltitude = GetPrecisionModeAltitude(physicsDeltaTime);
            }
            else
            {
                newAltitude = GetAltitude(physicsDeltaTime);
            }

            altitude = newAltitude;
        }

        double GetAltitude(double physicsDeltaTime)
        {
            IMyCubeGrid grid = block.CubeGrid;
            Vector3D center = useBlockPosition ? block.WorldAABB.Center : grid.WorldAABB.Center;
            double distance = Vector3D.Distance(center, nearestPlanet.PositionComp.GetPosition());
            return distance - nearestPlanet.MinimumRadius;
        }

        double GetPrecisionModeAltitude(double physicsDeltaTime)
        {

            IMyCubeGrid grid = block.CubeGrid;
            Vector3D center = useBlockPosition ? block.WorldAABB.Center : grid.WorldAABB.Center;
            Vector3D surfacePoint = nearestPlanet.GetClosestSurfacePointGlobal(ref center);

            return Vector3D.Distance(surfacePoint, center);
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine("");
            customInfo.AppendLine("Type: Planet Altitude Sensor");
            customInfo.AppendLine("IsInGravity: " + logicComponent.rigidbody.IsInGravity);
            customInfo.AppendLine("Planets: " + PlanetsList.planets.Count);
            customInfo.AppendLine("Nearest Planet: " + (nearestPlanet != null ? nearestPlanet.Name.Substring(0, 10) : "None"));
            customInfo.AppendLine("Altitude: " + Math.Round(altitude, 1));
            //customInfo.AppendLine("MAx Altitude Smooth Speed: " + Math.Round(maxAltitudeSmoothSpeed, 1) + " m/s");
            customInfo.AppendLine("Use Block Position : " + useBlockPosition);
        }

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }
        }
    }
}
