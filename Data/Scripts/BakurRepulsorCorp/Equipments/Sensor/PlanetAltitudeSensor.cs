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

        static Separator<PlanetAltitudeSensor> separator;
        static Label<PlanetAltitudeSensor> planetAltitudeLabel;

        #region precision mode

        static Separator<PlanetAltitudeSensor> precisionModeSeparator;
        static PlanetAltitudeSensor_PrecisionModeSwitch precisionModeSwitch;
        static PlanetAltitudeSensor_PrecisionModeDisableAction precisionModeDisableAction;
        static PlanetAltitudeSensor_PrecisionModeEnableAction precisionModeEnableAction;
        static PlanetAltitudeSensor_PrecisionModeToggleAction precisionModeToggleAction;

        public static string PRECISION_MODE_PROPERTY_NAME = "PlanetAltitudeSensor_PrecisionMode";

        public bool defaultPrecisionMode = true;

        public bool precisionMode
        {
            set
            {
                string id = GeneratatePropertyId(PRECISION_MODE_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(PRECISION_MODE_PROPERTY_NAME);
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

        static PlanetAltitudeSensor_UseBlockPositionSwitch useBlockPositionSwitch;

        public static string USE_BLOCK_POSITION_PROPERTY_NAME = "PlanetAltitudeSensor_UseBlockPosition";

        public bool defaultUseBlockPosition = false;

        public bool useBlockPosition
        {
            set
            {
                string id = GeneratatePropertyId(USE_BLOCK_POSITION_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_BLOCK_POSITION_PROPERTY_NAME);
                bool result = defaultUseBlockPosition;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseBlockPosition;
            }
        }

        #endregion       

        public PlanetAltitudeSensor(BakurBlock component) : base(component)
        {
        }

        #region lifecycle

        public override void Initialize()
        {

            if (separator == null)
            {
                separator = new Separator<PlanetAltitudeSensor>("PlanetAltitudeSensor_Separator");
                separator.Initialize();
            }

            if (planetAltitudeLabel == null)
            {
                planetAltitudeLabel = new Label<PlanetAltitudeSensor>("PlanetAltitudeSensor_PlanetAltitudeLabel", "Planet Altitude Sensor");
                planetAltitudeLabel.Initialize();
            }


            if (useBlockPositionSwitch == null)
            {
                useBlockPositionSwitch = new PlanetAltitudeSensor_UseBlockPositionSwitch();
                useBlockPositionSwitch.Initialize();
            }

            if (precisionModeSeparator == null)
            {
                precisionModeSeparator = new Separator<PlanetAltitudeSensor>("PlanetAltitudeSensor_PrecisionModeSeparator");
                precisionModeSeparator.Initialize();
            }

            if (precisionModeSwitch == null)
            {
                precisionModeSwitch = new PlanetAltitudeSensor_PrecisionModeSwitch();
                precisionModeSwitch.Initialize();
            }

            if (precisionModeDisableAction == null)
            {
                precisionModeDisableAction = new PlanetAltitudeSensor_PrecisionModeDisableAction();
                precisionModeDisableAction.Initialize();
            }

            if (precisionModeEnableAction == null)
            {
                precisionModeEnableAction = new PlanetAltitudeSensor_PrecisionModeEnableAction();
                precisionModeEnableAction.Initialize();
            }

            if (precisionModeToggleAction == null)
            {
                precisionModeToggleAction = new PlanetAltitudeSensor_PrecisionModeToggleAction();
                precisionModeToggleAction.Initialize();
            }
        }

        public override void Destroy()
        {
        }

        #endregion

        public override void UpdateSensor(double physicsDeltaTime)
        {
            base.UpdateSensor(physicsDeltaTime);

            if (!component.rigidbody.IsInGravity || nearestPlanet == null)
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
            customInfo.AppendLine("IsInGravity: " + component.rigidbody.IsInGravity);
            customInfo.AppendLine("Planets: " + PlanetsList.planets.Count);
            customInfo.AppendLine("Nearest Planet: " + (nearestPlanet != null ? nearestPlanet.Name.Substring(0, 10) : "None"));
            customInfo.AppendLine("Altitude: " + Math.Round(altitude, 1));
            //customInfo.AppendLine("MAx Altitude Smooth Speed: " + Math.Round(maxAltitudeSmoothSpeed, 1) + " m/s");
            customInfo.AppendLine("Use Block Position : " + useBlockPosition);
        }

        public override void Debug()
        {
            if (!component.debugEnabled)
            {
                return;
            }
        }
    }
}
