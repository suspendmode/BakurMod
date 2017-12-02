using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorPointLift : LogicElement
    {
        public double maxLinearAcceleration = 10;

        public RepulsorPointLift(LogicComponent component) : base(component)
        {
        }

        #region use point lift

        public readonly string USE_POINT_LIFT_PROPERTY_NAME = "PointLift_UsePointLift";

        public bool defaultUsePointLift = true;

        public bool usePointLift
        {
            set
            {
                string id = GeneratePropertyId(USE_POINT_LIFT_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_POINT_LIFT_PROPERTY_NAME);
                bool result = defaultUsePointLift;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUsePointLift;
            }
        }

        #endregion

        public double altitude = double.NaN;
        public double desiredAltitude = double.NaN;

        #region normalized altitude

        public readonly string NORMALIZED_DISTANCE_PROPERTY_NAME = "RepulsorPointLift_NormalizedDistance";

        public double defaultNormalizedDistance = 1;

        public double normalizedAltitude
        {
            set
            {
                string id = GeneratePropertyId(NORMALIZED_DISTANCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(NORMALIZED_DISTANCE_PROPERTY_NAME);
                double result = defaultNormalizedDistance;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultNormalizedDistance;
            }

        }

        #endregion                   

        DoublePID altitudePID = new DoublePID();

        #region Kp

        public readonly string PROPORTIONAL_COEFFICIENT_PROPERTY_NAME = "RepulsorPointLift_ProportionalCoefficient";

        public double defaultProportionalCoefficient = 0.5;

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

        public readonly string INTEGRAL_COEFFICIENT_PROPERTY_NAME = "RepulsorPointLift_IntegralCoefficient";

        public double defaultIntegralCoefficient = 0.0005;

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

        public readonly string DERIVATIVE_COEFFICIENT_PROPERTY_NAME = "RepulsorPointLift_DerivativeCoefficient";

        public double defaultDerivativeCoefficient = 0.3;

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

        void Clear()
        {
            desiredLinearAcceleration = Vector3D.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Point Lift");
            customInfo.AppendLine("Altitude: " + Math.Round(altitude, 1) + " m");
            customInfo.AppendLine("Max Altitude: " + Math.Round(maxAltitude, 1) + " m");
            customInfo.AppendLine("Desired Altitude: " + Math.Round(desiredAltitude, 1) + " m");
            customInfo.AppendLine("Normalized Desired Altitude: " + Math.Round(normalizedAltitude * 100, 1) + " %");
        }

        #endregion

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
            if (!double.IsNaN(altitude) && !double.IsNaN(maxAltitude) && double.IsNaN(desiredAltitude))
            {
                desiredAltitude = BakurMathHelper.InverseLerp(0, maxAltitude, altitude);
                //desiredAltitude = MathHelper.Clamp(desiredAltitude, 0, 1);
            }
        }

        void UpdateDesiredAltitudeFromInput()
        {
            if (double.IsNaN(desiredAltitude))
            {
                return;
            }

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null)
            {
                double step = 0.0001f;
                //MyAPIGateway.Utilities.ShowMessage("lift", "step: " + step + ", altitudeNormalized: " + altitudeNormalized + ", altitude: " + altitude + ", maxAltitude: " + maxAltitude);
                desiredAltitude += step * GetShipControllerLinearInput(shipController);
            }
        }

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

            double distanceError = (desiredAltitude * maxAltitude) - altitude;

            double output = altitudePID.UpdateValue(distanceError, physicsDeltaTime);
            //MyAPIGateway.Utilities.ShowMessage("pid:", "desiredDistance: " + desiredDistance + ", distance: " + distance + ", distanceError: " + distanceError + ", output: " + output);

            return output;
        }

        Vector3D desiredLinearAcceleration;
        public Vector3D desiredUp;

        public Vector3D GetDesiredLinearAcceleration(double physicsDeltaTime, Vector3D desiredUp, double measuredAltitude)
        {
            this.desiredUp = desiredUp;
            desiredLinearAcceleration = Vector3D.Zero;

            if (!logicComponent.enabled || !usePointLift || !logicComponent.rigidbody.IsInGravity)
            {
                Clear();
                return desiredLinearAcceleration;
            }

            UpdateAltitude(measuredAltitude);

            //MyAPIGateway.Utilities.ShowMessage("Repulsor Lift", Math.Round(altitude, 1) + "m");

            UpdateDesiredAltitude();

            //UpdatePrecisionMode(precisionMode);

            UpdateDesiredAltitudeFromInput();

            if (!double.IsNaN(desiredAltitude) && !double.IsNaN(altitude))
            {
                double pid = UpdatePID(physicsDeltaTime);

                desiredLinearAcceleration = pid * desiredUp * maxLinearAcceleration;
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
