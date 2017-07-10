using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class VelocityRudder : BakurBlockEquipment {

        #region max angle

        public static VelocityRudder_MaxAngleSlider maxAngleSlider;
        static VelocityRudder_IncraseMaxAngleAction incraseMaxAngleAction;
        static VelocityRudder_DecraseMaxAngleAction decraseMaxAngleAction;

        public static string MAX_ANGLE_PROPERTY_NAME = "VelocityRudder_MaxAngle";

        public double defaultMaxDegrees = 1;

        public VelocityRudder(BakurBlock block) : base(block) {
        }

        public double maxAngle
        {
            set
            {
                string id = GeneratatePropertyId(MAX_ANGLE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MAX_ANGLE_PROPERTY_NAME);
                double result = defaultMaxDegrees;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultMaxDegrees;
            }

        }

        #endregion

        static Separator<VelocityRudder> velocityRudderSeparator;
        static Label<VelocityRudder> velocityRudderLabel;


        #region use velocity rudder

        static VelocityRudder_UseVelocityRudderSwitch useVelocityRudderSwitch;
        static VelocityRudder_UseVelocityRudderToggleAction useVelocityRudderToggleAction;
        static VelocityRudder_UseVelocityRudderEnableAction useVelocityRudderEnableAction;
        static VelocityRudder_UseVelocityRudderDisableAction useVelocityRudderDisableAction;

        public static string USE_VELOCITY_RUDDER_COUPLER_PROPERTY_NAME = "VelocityRudder_UseVelocityRudder";

        public bool defaultUseVelocityRudder = true;

        public bool useVelocityRudder
        {
            set
            {
                string id = GeneratatePropertyId(USE_VELOCITY_RUDDER_COUPLER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_VELOCITY_RUDDER_COUPLER_PROPERTY_NAME);
                bool result = defaultUseVelocityRudder;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseVelocityRudder;
            }

        }



        #endregion

        #region lifecycle

        public override void Initialize() {

            // etheric rudder

            if (velocityRudderSeparator == null) {
                velocityRudderSeparator = new Separator<VelocityRudder>("VelocityRudder_VelocityRudderSeparator");
                velocityRudderSeparator.Initialize();
            }

            if (velocityRudderLabel == null) {
                velocityRudderLabel = new Label<VelocityRudder>("VelocityRudder_VelocityRudderLabel", "Velocity Rudder");
                velocityRudderLabel.Initialize();
            }

            // use velocity rudder

            if (useVelocityRudderSwitch == null) {
                useVelocityRudderSwitch = new VelocityRudder_UseVelocityRudderSwitch();
                useVelocityRudderSwitch.Initialize();
            }

            if (useVelocityRudderToggleAction == null) {
                useVelocityRudderToggleAction = new VelocityRudder_UseVelocityRudderToggleAction();
                useVelocityRudderToggleAction.Initialize();
            }

            if (useVelocityRudderEnableAction == null) {
                useVelocityRudderEnableAction = new VelocityRudder_UseVelocityRudderEnableAction();
                useVelocityRudderEnableAction.Initialize();
            }

            if (useVelocityRudderDisableAction == null) {
                useVelocityRudderDisableAction = new VelocityRudder_UseVelocityRudderDisableAction();
                useVelocityRudderDisableAction.Initialize();
            }

            // max angle

            if (maxAngleSlider == null) {
                maxAngleSlider = new VelocityRudder_MaxAngleSlider();
                maxAngleSlider.Initialize();
            }

            if (incraseMaxAngleAction == null) {
                incraseMaxAngleAction = new VelocityRudder_IncraseMaxAngleAction();
                incraseMaxAngleAction.Initialize();
            }

            if (decraseMaxAngleAction == null) {
                decraseMaxAngleAction = new VelocityRudder_DecraseMaxAngleAction();
                decraseMaxAngleAction.Initialize();
            }
        }

        public override void Destroy() {

        }

        #endregion      

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Velocity Rudder ==");
            customInfo.AppendLine("Enabled: " + useVelocityRudder);
            customInfo.AppendLine("Max Degrees : " + maxAngle);
        }

        public override void Debug() {

            if (!component.debugEnabled) {
                return;
            }

            Vector3 blockPosition = block.GetPosition();
            IMyCubeGrid grid = block.CubeGrid;
            Vector3D velocity = grid.Physics.LinearVelocity;
            Vector3D velocityDirection = velocity;
            velocityDirection.Normalize();

            Quaternion from = Quaternion.CreateFromForwardUp(velocityDirection, block.WorldMatrix.Up);
            Quaternion to = Quaternion.CreateFromForwardUp(block.WorldMatrix.Forward, block.WorldMatrix.Up);

            DebugDraw.DrawLine(blockPosition, blockPosition + from.Forward * 100, Color.Red, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + to.Forward * 100, Color.Green, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + velocity * 100, Color.Orange, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + rotatedVelocity * 100, Color.Blue, 0.05f);
        }

        #endregion

        #region update

        public void UpdateBeforeSimulation(double physicsDeltaTime, double updateDeltaTime) {

            if (useVelocityRudder) {
                UpdateVelocityRudder(physicsDeltaTime);
            }
        }

        void UpdateVelocityRudder(double physicsDeltaTime) {

            IMyCubeGrid grid = block.CubeGrid;
            Vector3 velocity = grid.Physics.LinearVelocity;
            Vector3 velocityDirection = velocity;
            velocityDirection.Normalize();

            //Matrix current = grid.WorldMatrix.GetOrientation();


            //Quaternion from = Quaternion.CreateFromForwardUp(velocityDirection, component.gravityUp);
            //Quaternion to = Quaternion.CreateFromForwardUp(block.WorldMatrix.Forward, component.gravityUp);
            Quaternion from = Quaternion.CreateFromForwardUp(velocityDirection, Vector3.Up);
            Quaternion to = Quaternion.CreateFromRotationMatrix(block.WorldMatrix);

            //Vector3 blockPosition = block.GetPosition();

            //Quaternion velocityForward = Quaternion.CreateFromForwardUp(velocity, up);

            //Quaternion step = from - RotateTowards(from, to, maxDegrees);                        
            //float angle = BakurMathHelper.Angle(from, to);
            Quaternion step = BakurMathHelper.RotateTowards(from, to, 0) - from;
            //Quaternion step = Quaternion.Slerp(from, to, maxRotationAngle) - from;

            //MyAPIGateway.Utilities.ShowMessage("Flight", "maxSpeed: " + maxSpeed + ", normalized: " + normalizedVelocity);
            //Matrix rotationStep = Matrix.CreateFromQuaternion(step);
            rotatedVelocity = Vector3D.Transform(velocity, step);

            grid.Physics.LinearVelocity = rotatedVelocity;

            //Vector3 rotatedVelocity = velocity;
            //Vector3.RotateAndScale(ref velocity, ref rotationStep, out rotatedVelocity);

        }

        Vector3D rotatedVelocity;

        #endregion
    }
}

/*
        Vector3D predictedVelocity = Vector3D.Zero;
        predictedVelocity += grid.WorldMatrix.Right * velocity.X;
        predictedVelocity += grid.WorldMatrix.Up * velocity.Y;
        predictedVelocity += grid.WorldMatrix.Forward * velocity.Z;

        float num = Angle(from, to);
        if (num == 0f) {
            grid.Physics.LinearVelocity = velocity;
        } else {
            float t = Math.Min(1f, maxDegrees / num);
            grid.Physics.LinearVelocity = Vector3.Lerp(velocity, predictedVelocity, t);
        }

Quaternion currentForward = attachedRigidbody.rotation;
Quaternion velocityForward = velocity.sqrMagnitude > 0.001f ? Quaternion.LookRotation(velocity.normalized, up) : Quaternion.identity;
Quaternion step = Quaternion.RotateTowards(currentForward, velocityForward, velocityRotationRate * deltaTime);
velocity = Vector3.Lerp(velocity, step * Vector3.forward, velocityStep * deltaTime);

attachedRigidbody.velocity = velocity;
*/
