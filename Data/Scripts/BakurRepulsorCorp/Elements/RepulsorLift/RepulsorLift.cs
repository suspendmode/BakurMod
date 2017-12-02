using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorLift : LogicElement
    {

        public RepulsorLift(LogicComponent component) : base(component)
        {
        }

        public double altitude = double.NaN;

        #region use repulsor lift

        public readonly string USE_REPULSOR_LIFT_PROPERTY_NAME = "RepulsorLift_UseRepulsorLift";

        public bool defaultUseRepulsorLift = true;

        public bool useLift
        {
            set
            {
                string id = GeneratePropertyId(USE_REPULSOR_LIFT_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_REPULSOR_LIFT_PROPERTY_NAME);
                bool result = defaultUseRepulsorLift;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseRepulsorLift;
            }
        }

        #endregion

        #region desired altitude

        public readonly string NORMALIZED_ALTITUDE_PROPERTY_NAME = "RepulsorLift_NormalizedAltitude";

        public double defaultNormalizedAltitude = double.NaN;

        public double normalizedAltitude
        {
            set
            {
                string id = GeneratePropertyId(NORMALIZED_ALTITUDE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(NORMALIZED_ALTITUDE_PROPERTY_NAME);
                double result = defaultNormalizedAltitude;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultNormalizedAltitude;
            }

        }

        #endregion


        DoublePID altitudePID = new DoublePID();

        #region Kp

        public readonly string PROPORTIONAL_COEFFICIENT_PROPERTY_NAME = "RepulsorLift_ProportionalCoefficient";

        public double defaultProportionalCoefficient = 0.6;

        public double proportionalCoefficient
        {
            set
            {
                string id = GeneratePropertyId(PROPORTIONAL_COEFFICIENT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(PROPORTIONAL_COEFFICIENT_PROPERTY_NAME);
                double result = defaultProportionalCoefficient;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultProportionalCoefficient;
            }
        }

        #endregion

        #region Ki

        public readonly string INTEGRAL_COEFFICIENT_PROPERTY_NAME = "RepulsorLift_IntegralCoefficient";

        public double defaultIntegralCoefficient = 0.001;

        public double integralCoefficient
        {
            set
            {
                string id = GeneratePropertyId(INTEGRAL_COEFFICIENT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(INTEGRAL_COEFFICIENT_PROPERTY_NAME);
                double result = defaultIntegralCoefficient;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultIntegralCoefficient;
            }
        }

        #endregion

        #region Kd

        public readonly string DERIVATIVE_COEFFICIENT_PROPERTY_NAME = "RepulsorLift_DerivativeCoefficient";

        public double defaultDerivativeCoefficient = 0.4;

        public double derivativeCoefficient
        {
            set
            {
                string id = GeneratePropertyId(DERIVATIVE_COEFFICIENT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(DERIVATIVE_COEFFICIENT_PROPERTY_NAME);
                double result = defaultDerivativeCoefficient;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultDerivativeCoefficient;
            }
        }

        #endregion

        #region lifecycle

        public override void Initialize()
        {

        }

        public override void Destroy()
        {
            Clear();
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Lift");
            customInfo.AppendLine("Desired Linear Velocity: " + Math.Round(desiredLinearAcceleration.Length(), 1) + " m/s");
            customInfo.AppendLine("Altitude: " + Math.Round(altitude, 1) + " m");
            customInfo.AppendLine("Max Altitude: " + Math.Round(maxAltitude, 1) + " m");
            customInfo.AppendLine("Desired Altitude: " + Math.Round(normalizedAltitude * maxAltitude, 1) + " m");
        }

        #endregion

        void Clear()
        {
            normalizedAltitude = double.NaN;
            altitude = double.NaN;
        }

        public double maxAltitude
        {
            get
            {
                return RepulsorCoil.GetMaxAltitude(block.CubeGrid);
            }
        }

        void UpdateAltitude(double newAltitude)
        {
            altitude = newAltitude;
        }

        void UpdateDesiredAltitude()
        {
            if (!double.IsNaN(altitude) && !double.IsNaN(maxAltitude) && double.IsNaN(normalizedAltitude))
            {
                normalizedAltitude = BakurMathHelper.InverseLerp(0, maxAltitude, altitude);
                //desiredAltitude = MathHelper.Clamp(desiredAltitude, 0, 1);
            }
        }

        #region input

        double GetShipControllerLinearInput(IMyShipController shipController)
        {

            double desired = 0;

            if (!shipController.CanControlShip)
            {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP))
                {
                    desired = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH))
                {
                    desired = -1;
                }
            }
            else
            {
                desired = MathHelper.Clamp(shipController.MoveIndicator.Y, -1, 1);
            }
            //desired *= (physicsDeltaTime * physicsDeltaTime * 0.3);
            return desired;
        }

        #endregion

        void UpdateDesiredAltitudeFromInput()
        {
            if (double.IsNaN(normalizedAltitude))
            {
                return;
            }

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null)
            {
                double step = 0.0001f;
                //MyAPIGateway.Utilities.ShowMessage("lift", "step: " + step + ", altitudeNormalized: " + altitudeNormalized + ", altitude: " + altitude + ", maxAltitude: " + maxAltitude);
                normalizedAltitude += step * GetShipControllerLinearInput(shipController);
            }
        }

        double UpdatePID(double physicsDeltaTime)
        {
            // update pid

            altitudePID.Kp = proportionalCoefficient;
            altitudePID.Ki = integralCoefficient;
            altitudePID.Kd = derivativeCoefficient;

            altitudePID.clampOutput = true;
            altitudePID.minOutput = -1;
            altitudePID.maxOutput = 1;

            // update desired force

            double distanceError = (normalizedAltitude * maxAltitude) - altitude;

            double output = altitudePID.UpdateValue(distanceError, physicsDeltaTime);
            //MyAPIGateway.Utilities.ShowMessage("pid:", "desiredDistance: " + desiredDistance + ", distance: " + distance + ", distanceError: " + distanceError + ", output: " + output);

            return output;
        }

        Vector3D desiredLinearAcceleration;

        public Vector3D GetLinearAcceleration(double physicsDeltaTime, double measuredAltitude)
        {
            desiredLinearAcceleration = Vector3D.Zero;

            if (!logicComponent.enabled || !useLift || !logicComponent.rigidbody.IsInGravity)
            {
                Clear();
                return desiredLinearAcceleration;
            }

            UpdateAltitude(measuredAltitude);

            //MyAPIGateway.Utilities.ShowMessage("Repulsor Lift", Math.Round(altitude, 1) + "m");

            UpdateDesiredAltitude();

            //UpdatePrecisionMode(precisionMode);

            UpdateDesiredAltitudeFromInput();

            if (!double.IsNaN(normalizedAltitude) && !double.IsNaN(altitude))
            {
                double pid = UpdatePID(physicsDeltaTime);
                double acceleration = logicComponent.rigidbody.gravity.Length() * 10;
                desiredLinearAcceleration = pid * logicComponent.rigidbody.gravityUp * acceleration;
            }

            //MyAPIGateway.Utilities.ShowMessage("Repulsor Lift", desiredLinearAcceleration + " N");

            /*
            double aa = PlanetsSession.GetNearestPlanet(block.CubeGrid).AtmosphereAltitude;
            double ar = PlanetsSession.GetNearestPlanet(block.CubeGrid).AtmosphereRadius;
            double min = PlanetsSession.GetNearestPlanet(block.CubeGrid).MinimumRadius;
            double max = PlanetsSession.GetNearestPlanet(block.CubeGrid).MaximumRadius;
            double av = PlanetsSession.GetNearestPlanet(block.CubeGrid).AverageRadius;

            MyAPIGateway.Utilities.ShowMessage("lift:", "AtmosphereAltitude: " + aa + ", AtmosphereRadius: " + ar + ", MinimumRadius: " + min + ", MaximumRadius: " + max + ", AverageRadius: " + av);
            */

            // MyAPIGateway.Utilities.ShowMessage("lift:", "desiredDistance: " + desiredDistance + ", distance: " + distance + ", desiredLinearAcceleration: " + desiredLinearAcceleration);

            return desiredLinearAcceleration;
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
