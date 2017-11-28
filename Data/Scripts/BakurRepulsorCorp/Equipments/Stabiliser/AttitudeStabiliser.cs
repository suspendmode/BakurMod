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

            customInfo.AppendLine("maxAcceleration : " + Math.Round(maxAcceleration, 1));
            customInfo.AppendLine("desiredAcceleration : " + Math.Round(desiredAcceleration.Length(), 1));
            customInfo.AppendLine("slowdownAngle : " + Math.Round(slowdownAngle, 1));
        }

        #endregion

        public double maxAcceleration = 180;
        public Vector3D desiredAcceleration;
        public Vector3D targetForward;
        public Vector3D targetUp;
        double slowdownAngle = 10;

        public Vector3D GetAngularAcceleration(double physicsDeltaTime, Vector3D currentForward, Vector3D currentUp, Vector3D targetForward, Vector3D targetUp) {

            this.targetForward = targetForward;
            this.targetUp = targetUp;            
            desiredAcceleration = Vector3D.Zero;

            if (!component.rigidbody.IsInGravity) {
                return desiredAcceleration;
            }

            if (targetForward == Vector3D.Zero) {
                return desiredAcceleration;
            }

            IMyCubeGrid grid = block.CubeGrid;

            double thetaZ = 0;
            double thetaY = 0;
            double angle = 0;
            double minAngle = 0.5f;
            double rotationTime = 0;

            float anglePredictionTime = 2 * (float)physicsDeltaTime;

            Vector3D currentAngularVelocity = grid.Physics.AngularVelocity;

            Vector3D predictedForward = Vector3D.Transform(currentForward, BakurMathHelper.AngleAxis(
               currentAngularVelocity.Length() * BakurMathHelper.Rad2Deg * anglePredictionTime,
               currentAngularVelocity
           ));
            predictedForward.Normalize();

            Vector3D predictedUp = Vector3D.Transform(currentUp, BakurMathHelper.AngleAxis(
               currentAngularVelocity.Length() * BakurMathHelper.Rad2Deg * anglePredictionTime,
               currentAngularVelocity 
           ));
            predictedUp.Normalize();

            angle = Math.Max(BakurMathHelper.Angle(predictedForward, targetForward), BakurMathHelper.Angle(predictedUp, targetUp));

            //MyAPIGateway.Utilities.ShowMessage("angle: ", angle + ", predictedForward: " + predictedForward + ", predictedUp: " + predictedUp);

            if (angle < minAngle) {
                desiredAcceleration = Vector3D.Zero;
                return desiredAcceleration;
            }

            Vector3D z = Vector3D.Cross(predictedForward, targetForward);
            Vector3D y = Vector3D.Cross(predictedUp, targetUp);
            //Debug.Log("z.magnitude: " + Mathf.Asin(z.magnitude));
            thetaZ = Math.Asin(z.Length());
            thetaY = Math.Asin(y.Length());

            Vector3D wZ = z * thetaZ;
            Vector3D wY = y * thetaY;

            Vector3D desiredAngularVelocity = wZ + wY;

           // MyAPIGateway.Utilities.ShowMessage("wZ: ", wZ + ", wY: " + wY + ", desiredAngularVelocity: " + desiredAngularVelocity);
            //Vector3 x = Vector3.Cross(predictedForward, targetForward);

            //theta = Mathf.Asin(x.magnitude) * Mathf.Rad2Deg;
            //theta = x.magnitude * Mathf.Rad2Deg;

            //if (theta < minAngle) {
            //Debug.Log("theta < minAngle");
            //  return Vector3.zero;
            //        }

            //      angularVelocity = x.normalized * theta;

            if (angle < slowdownAngle) {
                //Debug.Log("slowdownAngle");
                desiredAngularVelocity *= (angle / slowdownAngle);
            } else {
                //Debug.Log("normalAngle");

            }

            if (rotationTime != 0) {
                //Debug.Log("rotationTime!=0");
                desiredAngularVelocity *= (1 / rotationTime);
            }

            desiredAcceleration = desiredAngularVelocity / physicsDeltaTime;
            desiredAcceleration = Vector3D.ClampToSphere(desiredAcceleration * maxAcceleration, maxAcceleration);
            return desiredAcceleration;
            /*
            if (grid.Physics.AngularVelocity == Vector3D.Zero) {
                desiredAngularVelocity = Vector3D.Zero;
                return desiredAngularVelocity;
            }
            */
            /*
            Vector3 angularVelocity = grid.Physics.AngularVelocity;

            double angle = (angularVelocity.Length() * BakurMathHelper.Rad2Deg * stability / speed);

            Quaternion predictedAxis = BakurMathHelper.AngleAxis(angle, angularVelocity * (float)BakurMathHelper.Rad2Deg);
            Vector3D predictedUp = Vector3D.Transform(current, predictedAxis);

            desiredAcceleration = Vector3D.Cross(predictedUp, desired) * maxAcceleration;

            return desiredAcceleration;
            */
        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }
        }
    }
}