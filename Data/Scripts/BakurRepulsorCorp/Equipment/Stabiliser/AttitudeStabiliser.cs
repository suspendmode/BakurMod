using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class AttitudeStabiliser : BakurBlockEquipment {

        public AttitudeStabiliser(BakurBlock component) : base(component) {
        }

        static Separator<AttitudeStabiliser> attitudeStabiliserSeparator;
        static Label<AttitudeStabiliser> attitudeStabiliserLabel;

        #region stability

        public static Stabiliser_StabilitySlider stabilitySlider;
        static Stabiliser_DecraseStabilityAction decraseStabilityAction;
        static Stabiliser_IncraseStabilityAction incraseStabilityAction;

        public static string STABILITY_PROPERTY_NAME = "AttitudeStabiliser_Stability";

        public double defaultStability = 0.3;

        public double stability
        {
            set
            {
                string id = GeneratatePropertyId(STABILITY_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(STABILITY_PROPERTY_NAME);
                double result = defaultStability;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultStability;
            }

        }

        #endregion

        #region speed

        public static Stabiliser_SpeedSlider speedSlider;
        static Stabiliser_DecraseSpeedAction decraseSpeedAction;
        static Stabiliser_IncraseSpeedAction incraseSpeedAction;

        public static string SPEED_PROPERTY_NAME = "AttitudeStabiliser_Speed";

        public double defaultSpeed = 2;

        public double speed
        {
            set
            {
                string id = GeneratatePropertyId(SPEED_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(SPEED_PROPERTY_NAME);
                double result = defaultSpeed;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultSpeed;
            }

        }        

        #endregion

        #region lifecycle

        public override void Initialize() {

            if (attitudeStabiliserSeparator == null) {
                attitudeStabiliserSeparator = new Separator<AttitudeStabiliser>("AttitudeStabiliser_AttitudeStabiliserSeparator");
                attitudeStabiliserSeparator.Initialize();
            }

            if (attitudeStabiliserLabel == null) {
                attitudeStabiliserLabel = new Label<AttitudeStabiliser>("AttitudeStabiliser_AttitudeStabiliserLabel", "Attitude Stabiliser");
                attitudeStabiliserLabel.Initialize();
            }


            if (stabilitySlider == null) {
                stabilitySlider = new Stabiliser_StabilitySlider();
                stabilitySlider.Initialize();
            }

            if (speedSlider == null) {
                speedSlider = new Stabiliser_SpeedSlider();
                speedSlider.Initialize();
            }

            if (decraseSpeedAction == null) {
                decraseSpeedAction = new Stabiliser_DecraseSpeedAction();
                decraseSpeedAction.Initialize();
            }

            if (decraseStabilityAction == null) {
                decraseStabilityAction = new Stabiliser_DecraseStabilityAction();
                decraseStabilityAction.Initialize();
            }

            if (incraseSpeedAction == null) {
                incraseSpeedAction = new Stabiliser_IncraseSpeedAction();
                incraseSpeedAction.Initialize();
            }

            if (incraseStabilityAction == null) {
                incraseStabilityAction = new Stabiliser_IncraseStabilityAction();
                incraseStabilityAction.Initialize();
            }
        }

        public override void Destroy() {
            Clear();
        }

        public void Clear() {
            desiredAcceleration = Vector3.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Attitude Stabiliser ==");
            customInfo.AppendLine("Speed : " + Math.Round(speed, 1));
            customInfo.AppendLine("Stability : " + Math.Round(stability, 1));
            customInfo.AppendLine("Desire Velocity : " + Math.Round(desiredAcceleration.Length(), 1) + " °/s");
        }

        #endregion

        public double maxAcceleration;
        public Vector3D desiredAcceleration;
        public Vector3D desired;

        public Vector3D GetDesiredAngularAcceleration(double maxAcceleration, Vector3D current, Vector3D desired) {

            this.desired = desired;
            this.maxAcceleration = maxAcceleration;
            desiredAcceleration = Vector3D.Zero;

            if (!component.IsInGravity) {
                return desiredAcceleration;
            }

            if (desired == Vector3D.Zero) {
                return desiredAcceleration;
            }

            IMyCubeGrid grid = block.CubeGrid;

            /*
            if (grid.Physics.AngularVelocity == Vector3D.Zero) {
                desiredAngularVelocity = Vector3D.Zero;
                return desiredAngularVelocity;
            }
            */

            Vector3 angularVelocity = grid.Physics.AngularVelocity;

            double angle = (angularVelocity.Length() * BakurMathHelper.Rad2Deg * stability / speed);

            Quaternion predictedAxis = BakurMathHelper.AngleAxis(angle, angularVelocity * (float)BakurMathHelper.Rad2Deg);
            Vector3D predictedUp = Vector3D.Transform(current, predictedAxis);

            desiredAcceleration = Vector3D.Cross(predictedUp, desired) * maxAcceleration;

            return desiredAcceleration;
        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }
        }
    }
}