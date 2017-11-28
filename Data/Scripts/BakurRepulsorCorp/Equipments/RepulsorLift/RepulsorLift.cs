using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorLift : BakurBlockEquipment
    {

        public RepulsorLift(BakurBlock component) : base(component)
        {
        }

        static Separator<RepulsorLift> repulsorLiftSeparator;
        static Label<RepulsorLift> repulsorLiftLabel;

        public double altitude = double.NaN;

        #region use repulsor lift

        static Lift_UseRepulsorLiftSwitch useRepulsorLiftSwitch;
        static Lift_UseRepulsorLiftToggleAction useRepulsorLiftToggleAction;
        static Lift_UseRepulsorLiftEnableAction useRepulsorLiftEnableAction;
        static Lift_UseRepulsorLiftDisableAction useRepulsorLiftDisableAction;

        public static string USE_REPULSOR_LIFT_PROPERTY_NAME = "RepulsorLift_UseRepulsorLift";

        public bool defaultUseRepulsorLift = true;

        public bool useLift
        {
            set
            {
                string id = GeneratatePropertyId(USE_REPULSOR_LIFT_PROPERTY_NAME);
                SetVariable<bool>(id, value);
                RefreshControls();
            }
            get
            {
                string id = GeneratatePropertyId(USE_REPULSOR_LIFT_PROPERTY_NAME);
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

        static Lift_DesiredAltitudeSlider desiredAltitudeSlider;
        static Lift_IncraseDesiredAltitudeAction incraseDesiredAltitudeAction;
        static Lift_DecraseDesiredAltitudeAction decraseDesiredAltitudeAction;
        static Lift_SetDesiredAltitudeFromCurrentAction setDesiredAltitudeFromCurrentAction;

        public static string DESIRED_ALTITUDE_PROPERTY_NAME = "RepulsorLift_DesiredAltitude";

        public double defaultDesiredAltitude = double.NaN;

        public double desiredAltitude
        {
            set
            {
                string id = GeneratatePropertyId(DESIRED_ALTITUDE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(DESIRED_ALTITUDE_PROPERTY_NAME);
                double result = defaultDesiredAltitude;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultDesiredAltitude;
            }

        }

        #endregion        

        static Separator<RepulsorLift> feedbackControllerSeparator;
        static Label<RepulsorLift> feedbackControllerLabel;

        DoublePID altitudePID = new DoublePID();

        #region Kp

        static Lift_ProportionalCoefficientSlider proportionalCoefficientSlider;

        public static string PROPORTIONAL_COEFFICIENT_PROPERTY_NAME = "RepulsorLift_ProportionalCoefficient";

        public double defaultProportionalCoefficient = 0.5;

        public double proportionalCoefficient
        {
            set
            {
                string id = GeneratatePropertyId(PROPORTIONAL_COEFFICIENT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(PROPORTIONAL_COEFFICIENT_PROPERTY_NAME);
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

        static Lift_IntegralCoefficientSlider integralCoefficientSlider;

        public static string INTEGRAL_COEFFICIENT_PROPERTY_NAME = "RepulsorLift_IntegralCoefficient";

        public double defaultIntegralCoefficient = 0.00001;

        public double integralCoefficient
        {
            set
            {
                string id = GeneratatePropertyId(INTEGRAL_COEFFICIENT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(INTEGRAL_COEFFICIENT_PROPERTY_NAME);
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

        static Lift_DerivativeCoefficientSlider derivativeCoefficientSlider;

        public static string DERIVATIVE_COEFFICIENT_PROPERTY_NAME = "RepulsorLift_DerivativeCoefficient";

        public double defaultDerivativeCoefficient = 0.3;

        public double derivativeCoefficient
        {
            set
            {
                string id = GeneratatePropertyId(DERIVATIVE_COEFFICIENT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(DERIVATIVE_COEFFICIENT_PROPERTY_NAME);
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

        protected override void RefreshControls()
        {
            if (!isInitialized)
            {
                return;
            }

            base.RefreshControls();
            repulsorLiftSeparator.RefreshControl();
            repulsorLiftLabel.RefreshControl();
            useRepulsorLiftSwitch.RefreshControl();
            desiredAltitudeSlider.RefreshControl();
            feedbackControllerSeparator.RefreshControl();
            feedbackControllerLabel.RefreshControl();
            proportionalCoefficientSlider.RefreshControl();
            integralCoefficientSlider.RefreshControl();
            derivativeCoefficientSlider.RefreshControl();
        }

        public override void Initialize()
        {

            // repulsor

            if (repulsorLiftSeparator == null)
            {
                repulsorLiftSeparator = new Separator<RepulsorLift>("RepulsorLift_RepulsorLiftSeparator");
                repulsorLiftSeparator.Initialize();
            }

            if (repulsorLiftLabel == null)
            {
                repulsorLiftLabel = new Label<RepulsorLift>("RepulsorLift_RepulsorLiftLabel", "Repulsor Lift");
                repulsorLiftLabel.Initialize();
            }

            // use repulsor lift

            if (useRepulsorLiftSwitch == null)
            {
                useRepulsorLiftSwitch = new Lift_UseRepulsorLiftSwitch();
                useRepulsorLiftSwitch.Initialize();
            }

            if (useRepulsorLiftToggleAction == null)
            {
                useRepulsorLiftToggleAction = new Lift_UseRepulsorLiftToggleAction();
                useRepulsorLiftToggleAction.Initialize();
            }

            if (useRepulsorLiftEnableAction == null)
            {
                useRepulsorLiftEnableAction = new Lift_UseRepulsorLiftEnableAction();
                useRepulsorLiftEnableAction.Initialize();
            }

            if (useRepulsorLiftDisableAction == null)
            {
                useRepulsorLiftDisableAction = new Lift_UseRepulsorLiftDisableAction();
                useRepulsorLiftDisableAction.Initialize();
            }

            // desired altitude

            if (setDesiredAltitudeFromCurrentAction == null)
            {
                setDesiredAltitudeFromCurrentAction = new Lift_SetDesiredAltitudeFromCurrentAction();
                setDesiredAltitudeFromCurrentAction.Initialize();
            }

            if (desiredAltitudeSlider == null)
            {
                desiredAltitudeSlider = new Lift_DesiredAltitudeSlider();
                desiredAltitudeSlider.Initialize();
            }

            if (incraseDesiredAltitudeAction == null)
            {
                incraseDesiredAltitudeAction = new Lift_IncraseDesiredAltitudeAction();
                incraseDesiredAltitudeAction.Initialize();
            }

            if (decraseDesiredAltitudeAction == null)
            {
                decraseDesiredAltitudeAction = new Lift_DecraseDesiredAltitudeAction();
                decraseDesiredAltitudeAction.Initialize();
            }

            // Feedback controller

            if (feedbackControllerSeparator == null)
            {
                feedbackControllerSeparator = new Separator<RepulsorLift>("RepulsorLift_FeedbackControllerSeparator");
                feedbackControllerSeparator.Initialize();
            }

            if (feedbackControllerLabel == null)
            {
                feedbackControllerLabel = new Label<RepulsorLift>("RepulsorLift_FeedbackControllerLabel", "Control Loop Feedback Controller");
                feedbackControllerLabel.Initialize();
            }

            if (proportionalCoefficientSlider == null)
            {
                proportionalCoefficientSlider = new Lift_ProportionalCoefficientSlider();
                proportionalCoefficientSlider.Initialize();
            }

            if (integralCoefficientSlider == null)
            {
                integralCoefficientSlider = new Lift_IntegralCoefficientSlider();
                integralCoefficientSlider.Initialize();
            }

            if (derivativeCoefficientSlider == null)
            {
                derivativeCoefficientSlider = new Lift_DerivativeCoefficientSlider();
                derivativeCoefficientSlider.Initialize();
            }
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
            customInfo.AppendLine("== Repulsor Lift ==");
            customInfo.AppendLine("Desired Linear Velocity : " + Math.Round(desiredLinearAcceleration.Length(), 1) + " m/s");
            customInfo.AppendLine("Altitude : " + Math.Round(altitude, 1) + " m");
            customInfo.AppendLine("Max Altitude : " + Math.Round(maxAltitude, 1) + " m");
            customInfo.AppendLine("Desired Altitude : " + Math.Round(desiredAltitude * maxAltitude, 1) + " m");
        }

        #endregion

        void Clear()
        {
            desiredAltitude = double.NaN;
            altitude = double.NaN;
        }

        public double maxAltitude
        {
            get
            {
                return 10 * coilsCount;
            }
        }

        public int coilsCount
        {
            get
            {
                if (!RepulsorCoil.repulsorCoils.ContainsKey(block.CubeGrid.EntityId))
                {
                    return 0;
                }
                else
                {
                    return RepulsorCoil.repulsorCoils[block.CubeGrid.EntityId].Count;
                }
            }
        }

        void UpdateDesiredAltitude(double altitude)
        {

            if (double.IsNaN(altitude) || double.IsNaN(maxAltitude))
            {
                Clear();
                return;
            }

            this.altitude = altitude;

            if (double.IsNaN(desiredAltitude) && altitude <= maxAltitude)
            {
                desiredAltitude = BakurMathHelper.InverseLerp(0, maxAltitude, altitude);
                desiredAltitude = MathHelper.Clamp(desiredAltitude, 0, 1);
            }
        }

        #region input

        double GetShipControllerLinearInput(double physicsDeltaTime, IMyShipController shipController)
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

        void UpdateDesiredAltitudeFromInput(double physicsDeltaTime)
        {
            IMyCubeGrid grid = block.CubeGrid;

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null)
            {
                double step = 0.01f;
                //MyAPIGateway.Utilities.ShowMessage("lift", "step: " + step + ", altitudeNormalized: " + altitudeNormalized + ", altitude: " + altitude + ", maxAltitude: " + maxAltitude);
                desiredAltitude += step * GetShipControllerLinearInput(physicsDeltaTime, shipController);
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

            double distanceError = desiredAltitude - altitude;

            double output = altitudePID.UpdateValue(distanceError, physicsDeltaTime);
            //MyAPIGateway.Utilities.ShowMessage("pid:", "desiredDistance: " + desiredDistance + ", distance: " + distance + ", distanceError: " + distanceError + ", output: " + output);

            return output;
        }

        Vector3D desiredLinearAcceleration;

        public Vector3D GetLinearAcceleration(double physicsDeltaTime, double altitude)
        {

            desiredLinearAcceleration = Vector3D.Zero;

            if (!enabled || !component.enabled || !useLift || !component.rigidbody.IsInGravity)
            {
                Clear();
                return desiredLinearAcceleration;
            }

            //MyAPIGateway.Utilities.ShowMessage("Repulsor Lift", Math.Round(altitude, 1) + "m");
            UpdateDesiredAltitude(altitude);

            //UpdatePrecisionMode(precisionMode);

            UpdateDesiredAltitudeFromInput(physicsDeltaTime);

            double pid = UpdatePID(physicsDeltaTime);
            desiredLinearAcceleration = pid * -component.rigidbody.gravity * 2;

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

    }
}
