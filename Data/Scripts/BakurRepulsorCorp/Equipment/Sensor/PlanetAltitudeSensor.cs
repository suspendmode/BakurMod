﻿using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class PlanetAltitudeSensor : PlanetSensor {

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
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultPrecisionMode;
            }
        }

        #endregion

        #region altitude smooth time

        static PlanetAltitudeSensor_AltitudeSmoothTimeSlider altitudeSmoothTimeSlider;

        public static string ALTITUDE_SMOOTH_TIME_PROPERTY_NAME = "PlanetAltitudeSensor_AltitudeSmoothTime";

        public double defaultAltitudeSmoothTime = PlanetAltitudeSensor_AltitudeSmoothTimeSlider.minSmoothTime;

        public double altitudeSmoothTime
        {
            set
            {
                string id = GeneratatePropertyId(ALTITUDE_SMOOTH_TIME_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(ALTITUDE_SMOOTH_TIME_PROPERTY_NAME);
                double result = defaultAltitudeSmoothTime;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultAltitudeSmoothTime;
            }
        }

        #endregion

        #region max altitude round

        static PlanetAltitudeSensor_AltitudeRoundSlider altitudeRoundSlider;

        public static string ALTITUDE_ROUND_PROPERTY_NAME = "PlanetAltitudeSensor_AltitudeRound";

        public double defaultAltitudeRound = PlanetAltitudeSensor_AltitudeRoundSlider.minValue;

        public double altitudeRound
        {
            set
            {
                string id = GeneratatePropertyId(ALTITUDE_ROUND_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(ALTITUDE_ROUND_PROPERTY_NAME);
                double result = defaultAltitudeRound;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultAltitudeRound;
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
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseBlockPosition;
            }
        }

        #endregion       

        public PlanetAltitudeSensor(BakurBlock component) : base(component) {
        }

        #region lifecycle

        public override void Initialize() {

            if (separator == null) {
                separator = new Separator<PlanetAltitudeSensor>("PlanetAltitudeSensor_Separator");
                separator.Initialize();
            }

            if (planetAltitudeLabel == null) {
                planetAltitudeLabel = new Label<PlanetAltitudeSensor>("PlanetAltitudeSensor_PlanetAltitudeLabel", "Planet Altitude Sensor");
                planetAltitudeLabel.Initialize();
            }


            if (useBlockPositionSwitch == null) {
                useBlockPositionSwitch = new PlanetAltitudeSensor_UseBlockPositionSwitch();
                useBlockPositionSwitch.Initialize();
            }

            if (altitudeSmoothTimeSlider == null) {
                altitudeSmoothTimeSlider = new PlanetAltitudeSensor_AltitudeSmoothTimeSlider();
                altitudeSmoothTimeSlider.Initialize();
            }

            if (altitudeRoundSlider == null) {
                altitudeRoundSlider = new PlanetAltitudeSensor_AltitudeRoundSlider();
                altitudeRoundSlider.Initialize();
            }

            if (precisionModeSeparator == null) {
                precisionModeSeparator = new Separator<PlanetAltitudeSensor>("PlanetAltitudeSensor_PrecisionModeSeparator");
                precisionModeSeparator.Initialize();
            }

            if (precisionModeSwitch == null) {
                precisionModeSwitch = new PlanetAltitudeSensor_PrecisionModeSwitch();
                precisionModeSwitch.Initialize();
            }

            if (precisionModeDisableAction == null) {
                precisionModeDisableAction = new PlanetAltitudeSensor_PrecisionModeDisableAction();
                precisionModeDisableAction.Initialize();
            }

            if (precisionModeEnableAction == null) {
                precisionModeEnableAction = new PlanetAltitudeSensor_PrecisionModeEnableAction();
                precisionModeEnableAction.Initialize();
            }

            if (precisionModeToggleAction == null) {
                precisionModeToggleAction = new PlanetAltitudeSensor_PrecisionModeToggleAction();
                precisionModeToggleAction.Initialize();
            }
        }

        public override void Destroy() {
        }

        #endregion

        public override void UpdateSensor(double physicsDeltaTime) {

            base.UpdateSensor(physicsDeltaTime);

            if (!component.IsInGravity || nearestPlanet == null ) {
                altitude = double.NaN;
                return;
            }

            double newAltitude = 0;

            if (precisionMode) {
                newAltitude = GetPrecisionModeAltitude(physicsDeltaTime);
            } else {
                newAltitude = GetAltitude(physicsDeltaTime);
            }

            if (altitudeRound != 0) {
                newAltitude = ((int)Math.Floor(newAltitude / altitudeRound)) * altitudeRound;
            }

            if (altitudeSmoothTime > 0) {
                altitude = MathHelper.Lerp(altitude, newAltitude, altitudeSmoothTime / physicsDeltaTime);
            } else {
                altitude = newAltitude;
            }
        }

        double GetAltitude(double physicsDeltaTime) {
            IMyCubeGrid grid = block.CubeGrid;
            Vector3D center = useBlockPosition ? block.WorldAABB.Center : grid.WorldAABB.Center;
            double distance = Vector3D.Distance(center, nearestPlanet.PositionComp.GetPosition());
            return distance - nearestPlanet.MinimumRadius;
        }

        double GetPrecisionModeAltitude(double physicsDeltaTime) {

            IMyCubeGrid grid = block.CubeGrid;
            Vector3D center = useBlockPosition ? block.WorldAABB.Center : grid.WorldAABB.Center;
            Vector3D surfacePoint = nearestPlanet.GetClosestSurfacePointGlobal(ref center);

            return Vector3D.Distance(surfacePoint, center);
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {

            customInfo.AppendLine();
            customInfo.AppendLine("== Planet Altitude Sensor ==");
            customInfo.AppendLine("IsInGravity : " + component.IsInGravity);
            customInfo.AppendLine("Planets : " + planets.Count);
            customInfo.AppendLine("Nearest Planet : " + (nearestPlanet != null ? nearestPlanet.Name : "None"));
            customInfo.AppendLine("Atmosphere : " + Math.Round(atmosphere, 1));
            customInfo.AppendLine("Atmospheres : " + atmospheres);
            customInfo.AppendLine("Altitude : " + Math.Round(altitude, 1));
            customInfo.AppendLine("Altitude Smooth Time: " + Math.Round(altitudeSmoothTime * 100, 1) + " %");
            //customInfo.AppendLine("MAx Altitude Smooth Speed: " + Math.Round(maxAltitudeSmoothSpeed, 1) + " m/s");
            customInfo.AppendLine("Use Block Position : " + useBlockPosition);
        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }
        }
    }
}
