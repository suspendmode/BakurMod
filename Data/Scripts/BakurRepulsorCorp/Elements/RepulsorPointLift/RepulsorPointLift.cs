using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorPointLift : LogicElement
    {

        public RepulsorPointLift(LogicComponent component) : base(component)
        {
        }

        #region use point lift

        static PointLift_UsePointLiftSwitch usePointLiftSwitch;
        static PointLift_UsePointLiftToggleAction usePointLiftToggleAction;
        static PointLift_UsePointLiftEnableAction usePointLiftEnableAction;
        static PointLift_UsePointLiftDisableAction usePointLiftDisableAction;

        public static string USE_POINT_LIFT_PROPERTY_NAME = "PointLift_UsePointLift";

        public bool defaultUsePointLift = true;

        public bool usePointLift
        {
            set
            {
                string id = GeneratatePropertyId(USE_POINT_LIFT_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_POINT_LIFT_PROPERTY_NAME);
                bool result = defaultUsePointLift;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUsePointLift;
            }
        }

        #endregion

        static Separator<RepulsorPointLift> repulsorPointLiftSeparator;
        static Label<RepulsorPointLift> repulsorPointLiftLabel;

        public double altitude = double.NaN;
        public double desiredAltitude = double.NaN;

        #region normalized altitude

        static PointLift_NormalizedAltitudeSlider normalizedAltitudeSlider;
        static PointLift_IncraseNormalizedAltitudeAction incraseNormalizedAltitudeAction;
        static PointLift_DecraseNormalizedAltitudeAction decraseNormalizedAltitudeAction;
        static PointLift_SetDesiredAltitudeToCurrentAction setCurrentAsDesiredAltitudeAction;

        public static string NORMALIZED_DISTANCE_PROPERTY_NAME = "RepulsorPointLift_NormalizedDistance";

        public double defaultNormalizedDistance = 1;

        public double normalizedAltitude
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
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultNormalizedDistance;
            }

        }

        #endregion                   

        static Separator<RepulsorPointLift> feedbackControllerSeparator;
        static Label<RepulsorPointLift> feedbackControllerLabel;

        DoublePID distancePID = new DoublePID();

        #region Kp

        static PointLift_ProportionalCoefficientSlider proportionalCoefficientSlider;

        public static string PROPORTIONAL_COEFFICIENT_PROPERTY_NAME = "RepulsorPointLift_ProportionalCoefficient";

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

        static PointLift_IntegralCoefficientSlider integralCoefficientSlider;

        public static string INTEGRAL_COEFFICIENT_PROPERTY_NAME = "RepulsorPointLift_IntegralCoefficient";

        public double defaultIntegralCoefficient = 0.0005;

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

        static PointLift_DerivativeCoefficientSlider derivativeCoefficientSlider;

        public static string DERIVATIVE_COEFFICIENT_PROPERTY_NAME = "RepulsorPointLift_DerivativeCoefficient";

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

        public override void Initialize()
        {

            // repulsor

            if (repulsorPointLiftSeparator == null)
            {
                repulsorPointLiftSeparator = new Separator<RepulsorPointLift>("RepulsorPointLift_RepulsorPointLiftSeparator");
                repulsorPointLiftSeparator.Initialize();
            }

            if (repulsorPointLiftLabel == null)
            {
                repulsorPointLiftLabel = new Label<RepulsorPointLift>("RepulsorPointLift_RepulsorPointLiftLabel", "Repulsor Point Lift");
                repulsorPointLiftLabel.Initialize();
            }

            // use linear generator

            if (usePointLiftSwitch == null)
            {
                usePointLiftSwitch = new PointLift_UsePointLiftSwitch();
                usePointLiftSwitch.Initialize();
            }

            if (usePointLiftToggleAction == null)
            {
                usePointLiftToggleAction = new PointLift_UsePointLiftToggleAction();
                usePointLiftToggleAction.Initialize();
            }

            if (usePointLiftEnableAction == null)
            {
                usePointLiftEnableAction = new PointLift_UsePointLiftEnableAction();
                usePointLiftEnableAction.Initialize();
            }

            if (usePointLiftDisableAction == null)
            {
                usePointLiftDisableAction = new PointLift_UsePointLiftDisableAction();
                usePointLiftDisableAction.Initialize();
            }

            // actions

            if (setCurrentAsDesiredAltitudeAction == null)
            {
                setCurrentAsDesiredAltitudeAction = new PointLift_SetDesiredAltitudeToCurrentAction();
                setCurrentAsDesiredAltitudeAction.Initialize();
            }

            if (normalizedAltitudeSlider == null)
            {
                normalizedAltitudeSlider = new PointLift_NormalizedAltitudeSlider();
                normalizedAltitudeSlider.Initialize();
            }

            if (incraseNormalizedAltitudeAction == null)
            {
                incraseNormalizedAltitudeAction = new PointLift_IncraseNormalizedAltitudeAction();
                incraseNormalizedAltitudeAction.Initialize();
            }

            if (decraseNormalizedAltitudeAction == null)
            {
                decraseNormalizedAltitudeAction = new PointLift_DecraseNormalizedAltitudeAction();
                decraseNormalizedAltitudeAction.Initialize();
            }

            // Feedback controller

            if (feedbackControllerSeparator == null)
            {
                feedbackControllerSeparator = new Separator<RepulsorPointLift>("RepulsorPointLift_FeedbackControllerSeparator");
                feedbackControllerSeparator.Initialize();
            }

            if (feedbackControllerLabel == null)
            {
                feedbackControllerLabel = new Label<RepulsorPointLift>("RepulsorPointLift_FeedbackControllerLabel", "Control Loop Feedback Controller");
                feedbackControllerLabel.Initialize();
            }

            if (proportionalCoefficientSlider == null)
            {
                proportionalCoefficientSlider = new PointLift_ProportionalCoefficientSlider();
                proportionalCoefficientSlider.Initialize();
            }

            if (integralCoefficientSlider == null)
            {
                integralCoefficientSlider = new PointLift_IntegralCoefficientSlider();
                integralCoefficientSlider.Initialize();
            }

            if (derivativeCoefficientSlider == null)
            {
                derivativeCoefficientSlider = new PointLift_DerivativeCoefficientSlider();
                derivativeCoefficientSlider.Initialize();
            }
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
            customInfo.AppendLine("== Repulsor Point Lift ==");
            customInfo.AppendLine("Altitude : " + Math.Round(altitude, 1) + " m");
            customInfo.AppendLine("Max Altitude : " + Math.Round(maxAltitude, 1) + " m");
            customInfo.AppendLine("Desired Altitude : " + Math.Round(desiredAltitude, 1) + " m");
            customInfo.AppendLine("Normalized Desired Altitude : " + Math.Round(normalizedAltitude * 100, 1) + " %");
        }

        #endregion

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

            // update desired altitude

            if (double.IsNaN(desiredAltitude) && !double.IsNaN(altitude))
            {
                normalizedAltitude = altitude / maxAltitude;
                normalizedAltitude = BakurMathHelper.Clamp01(normalizedAltitude);
                desiredAltitude = altitude;
            }
            else if (!double.IsNaN(desiredAltitude))
            {
                normalizedAltitude = BakurMathHelper.Clamp01(normalizedAltitude);
                //maxAltitude = MathHelper.Clamp(desiredAltitude * 2, 0, maxAltitude);

                desiredAltitude = normalizedAltitude * maxAltitude;
                desiredAltitude = MathHelper.Clamp(desiredAltitude, 0, maxAltitude);
            }
            else
            {
                normalizedAltitude = 0;
                desiredAltitude = double.NaN;
            }
        }

        void UpdateNormalizedAltitudeFromInput(double physicsDeltaTime)
        {

            if (double.IsNaN(normalizedAltitude))
            {
                return;
            }
            // desired altitude
            IMyCubeGrid grid = block.CubeGrid;
            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null)
            {
                normalizedAltitude += GetShipControllerLinearInput(physicsDeltaTime, shipController) * physicsDeltaTime;
            }

            normalizedAltitude = BakurMathHelper.Clamp01(normalizedAltitude);

        }

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
            desired = (physicsDeltaTime * physicsDeltaTime * 0.3);
            return desired;
        }

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }
        }

        public Vector3D desiredLinearAcceleration;
        public Vector3D desiredUp;

        public Vector3D GetDesiredLinearAcceleration(double physicsDeltaTime, Vector3D desiredUp, double altitude)
        {

            this.altitude = altitude;
            this.desiredUp = desiredUp;

            desiredLinearAcceleration = Vector3D.Zero;

            UpdateDesiredAltitude(altitude);
            UpdateNormalizedAltitudeFromInput(physicsDeltaTime);

            if (double.IsNaN(desiredAltitude) || double.IsNaN(altitude))
            {
                return desiredLinearAcceleration;
            }

            if (!usePointLift || !logicComponent.rigidbody.IsInGravity)
            {
                Clear();
                return desiredLinearAcceleration;
            }

            // update pid

            distancePID.Kp = proportionalCoefficient;
            distancePID.Ki = integralCoefficient;
            distancePID.Kd = derivativeCoefficient;

            distancePID.clampOutput = true;
            distancePID.minOutput = -1;
            distancePID.maxOutput = 1;

            // update desired force

            double distanceError = desiredAltitude - altitude;

            double pid = distancePID.UpdateValue(distanceError, physicsDeltaTime);

            desiredLinearAcceleration = desiredUp * pid / physicsDeltaTime * logicComponent.rigidbody.gridMass;

            //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "desiredDistance:" + desiredDistance + ", distance:" + distance + ", distanceError:" + distanceError + ", pid: " + pid + ", desiredForce: " + desiredForce + ", gridMass: " + component.gridMass);

            return desiredLinearAcceleration;

            //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "altitude:" + altitude + ", desiredAltitude:" + desiredAltitude + ", output:" + output + ", maxForce: " + maxForce + ", desiredForce: " + desiredForce);
            // MyAPIGateway.Utilities.ShowMessage("RepulosrLift", "distanceError: " + Math.Round(distanceError, 1) + ", output:" + Math.Round(output, 1) + ", outputForce:" + Math.Round(outputForce.Length(), 1));
        }

    }
}
