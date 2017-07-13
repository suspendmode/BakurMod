using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class RepulsorLift : BakurBlockEquipment {

        public double maxDistance;
        //internal int enabledLiftsCount;

        public RepulsorLift(BakurBlock component) : base(component) {
        }

        static Separator<RepulsorLift> repulsorLiftSeparator;
        static Label<RepulsorLift> repulsorLiftLabel;

        public double distance = double.NaN;
        public double desiredDistance = double.NaN;

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
            }
            get
            {
                string id = GeneratatePropertyId(USE_REPULSOR_LIFT_PROPERTY_NAME);
                bool result = defaultUseRepulsorLift;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseRepulsorLift;
            }
        }


        #endregion

        #region normalized distance

        static Lift_NormalizedDistanceSlider normalizedDistanceSlider;
        static Lift_IncraseNormalizedDistanceAction incraseNormalizedDistanceAction;
        static Lift_DecraseNormalizedDistanceAction decraseNormalizedDistanceAction;
        static Lift_SetDesiredDistanceToCurrentAction setCurrentAsDesiredDistanceAction;

        public static string NORMALIZED_DISTANCE_PROPERTY_NAME = "RepulsorLift_NormalizedDistance";

        public double defaultNormalizedDistance = 1;

        public double normalizedDistance
        {
            set
            {
                string id = GeneratatePropertyId(NORMALIZED_DISTANCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(NORMALIZED_DISTANCE_PROPERTY_NAME);
                double result = defaultNormalizedDistance;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultNormalizedDistance;
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
                if (GetVariable<double>(id, out result)) {
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
                if (GetVariable<double>(id, out result)) {
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
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultDerivativeCoefficient;
            }
        }

        #endregion

        #region lifecycle

        public override void Initialize() {

            // repulsor

            if (repulsorLiftSeparator == null) {
                repulsorLiftSeparator = new Separator<RepulsorLift>("RepulsorLift_RepulsorLiftSeparator");
                repulsorLiftSeparator.Initialize();
            }

            if (repulsorLiftLabel == null) {
                repulsorLiftLabel = new Label<RepulsorLift>("RepulsorLift_RepulsorLiftLabel", "Repulsor Lift");
                repulsorLiftLabel.Initialize();
            }

            // use repulsor lift

            if (useRepulsorLiftSwitch == null) {
                useRepulsorLiftSwitch = new Lift_UseRepulsorLiftSwitch();
                useRepulsorLiftSwitch.Initialize();
            }

            if (useRepulsorLiftToggleAction == null) {
                useRepulsorLiftToggleAction = new Lift_UseRepulsorLiftToggleAction();
                useRepulsorLiftToggleAction.Initialize();
            }

            if (useRepulsorLiftEnableAction == null) {
                useRepulsorLiftEnableAction = new Lift_UseRepulsorLiftEnableAction();
                useRepulsorLiftEnableAction.Initialize();
            }

            if (useRepulsorLiftDisableAction == null) {
                useRepulsorLiftDisableAction = new Lift_UseRepulsorLiftDisableAction();
                useRepulsorLiftDisableAction.Initialize();
            }

            // desired altitude



            if (setCurrentAsDesiredDistanceAction == null) {
                setCurrentAsDesiredDistanceAction = new Lift_SetDesiredDistanceToCurrentAction();
                setCurrentAsDesiredDistanceAction.Initialize();
            }

            if (normalizedDistanceSlider == null) {
                normalizedDistanceSlider = new Lift_NormalizedDistanceSlider();
                normalizedDistanceSlider.Initialize();
            }

            if (incraseNormalizedDistanceAction == null) {
                incraseNormalizedDistanceAction = new Lift_IncraseNormalizedDistanceAction();
                incraseNormalizedDistanceAction.Initialize();
            }

            if (decraseNormalizedDistanceAction == null) {
                decraseNormalizedDistanceAction = new Lift_DecraseNormalizedDistanceAction();
                decraseNormalizedDistanceAction.Initialize();
            }

            // Feedback controller

            if (feedbackControllerSeparator == null) {
                feedbackControllerSeparator = new Separator<RepulsorLift>("RepulsorLift_FeedbackControllerSeparator");
                feedbackControllerSeparator.Initialize();
            }

            if (feedbackControllerLabel == null) {
                feedbackControllerLabel = new Label<RepulsorLift>("RepulsorLift_FeedbackControllerLabel", "Control Loop Feedback Controller");
                feedbackControllerLabel.Initialize();
            }

            if (proportionalCoefficientSlider == null) {
                proportionalCoefficientSlider = new Lift_ProportionalCoefficientSlider();
                proportionalCoefficientSlider.Initialize();
            }

            if (integralCoefficientSlider == null) {
                integralCoefficientSlider = new Lift_IntegralCoefficientSlider();
                integralCoefficientSlider.Initialize();
            }

            if (derivativeCoefficientSlider == null) {
                derivativeCoefficientSlider = new Lift_DerivativeCoefficientSlider();
                derivativeCoefficientSlider.Initialize();
            }


        }

        public override void Destroy() {
            Clear();
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Lift ==");
            customInfo.AppendLine("Desired Linear Velocity : " + Math.Round(desiredLinearAcceleration.Length(), 1) + " m/s");
            customInfo.AppendLine("Altitude : " + Math.Round(distance, 1) + " m");
            customInfo.AppendLine("Desired Altitude : " + Math.Round(desiredDistance, 1) + " m");
            customInfo.AppendLine("Normalized Desired Distance : " + Math.Round(normalizedDistance * 100, 1) + " %");
            customInfo.AppendLine("Max Desired Distance : " + Math.Round(maxDistance, 1) + " m");
        }

        #endregion

        /*
        public void Update() {
            // precision mode

            if (precisionMode != lastPrecisionMode) {
                desiredAltitude = distance;
                normalizedAltitude = distance / maxAltitude;
                lastPrecisionMode = precisionMode;
                normalizedAltitudeSlider.Update();
            }
        }
        */

        void Clear() {
            desiredDistance = double.NaN;
            distance = double.NaN;
        }

        void UpdateDesiredDistance(double distance, double maxAltitude) {

            if (double.IsNaN(desiredDistance) && !double.IsNaN(distance)) {
                normalizedDistance = BakurMathHelper.InverseLerp(0, maxAltitude, distance);
                normalizedDistance = BakurMathHelper.Clamp01(normalizedDistance);
                desiredDistance = distance;
            } else if (!double.IsNaN(desiredDistance)) {
                normalizedDistance = BakurMathHelper.Clamp01(normalizedDistance);
                desiredDistance = normalizedDistance * maxAltitude;
                desiredDistance = MathHelper.Clamp(desiredDistance, 0, maxAltitude);
            } else if (double.IsNaN(distance)) {
                Clear();
            } else {

            }
        }

        #region input

        /*
        void UpdatePrecisionMode(bool precisionMode) {
            if (!precisionModeSet) {
                lastPrecisionMode = precisionMode;
                precisionModeSet = true;
            }
        }
        */

        double GetShipControllerLinearInput(double physicsDeltaTime, IMyShipController shipController) {

            double desired = 0;

            if (!shipController.CanControlShip) {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP)) {
                    desired = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH)) {
                    desired = -1;
                }
            } else {
                desired = MathHelper.Clamp(shipController.MoveIndicator.Y, -1, 1);
            }
            desired *= (physicsDeltaTime * physicsDeltaTime * 0.3);
            return desired;
        }

        #endregion


        void UpdateNormalizedDistanceFromInput(double physicsDeltaTime) {
            IMyCubeGrid grid = block.CubeGrid;

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null) {
                normalizedDistance += GetShipControllerLinearInput(physicsDeltaTime, shipController) * physicsDeltaTime;
            }

            normalizedDistance = BakurMathHelper.Clamp01(normalizedDistance);
        }

        double UpdatePID(double physicsDeltaTime) {
            // update pid

            altitudePID.Kp = proportionalCoefficient;
            altitudePID.Ki = integralCoefficient;
            altitudePID.Kd = derivativeCoefficient;

            altitudePID.clampOutput = true;
            altitudePID.minOutput = -1;
            altitudePID.maxOutput = 1;

            // update desired force

            double distanceError = desiredDistance - distance;

            double output = altitudePID.UpdateValue(distanceError, physicsDeltaTime);
            //MyAPIGateway.Utilities.ShowMessage("pid:", "desiredDistance: " + desiredDistance + ", distance: " + distance + ", distanceError: " + distanceError + ", output: " + output);

            return output;
        }

        Vector3D desiredLinearAcceleration;

        public Vector3D GetLinearAcceleration(double physicsDeltaTime, double distance, double maxDistance) {

            this.maxDistance = maxDistance;
            this.distance = distance;

            desiredLinearAcceleration = Vector3D.Zero;

            UpdateDesiredDistance(distance, maxDistance);

            if (!useLift || !component.IsInGravity) {
                Clear();
                return desiredLinearAcceleration;
            }

            //UpdatePrecisionMode(precisionMode);e
            
            UpdateNormalizedDistanceFromInput(physicsDeltaTime);

            desiredDistance = normalizedDistance * maxDistance;

            double pid = UpdatePID(physicsDeltaTime);
            desiredLinearAcceleration = pid * -component.gravity * 2;

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