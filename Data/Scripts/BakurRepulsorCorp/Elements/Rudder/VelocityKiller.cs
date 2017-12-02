using Sandbox.ModAPI;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class VelocityKiller : LogicElement
    {

        #region max angle

        public readonly string MAX_ANGLE_PROPERTY_NAME = "EthericRudder_MaxAngle";

        public double defaultMaxDegrees = 1;

        public VelocityKiller(LogicComponent block) : base(block)
        {
        }

        public double maxAngle
        {
            set
            {
                string id = GeneratePropertyId(MAX_ANGLE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MAX_ANGLE_PROPERTY_NAME);
                double result = defaultMaxDegrees;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxDegrees;
            }

        }

        #endregion

        #region use eheric rudder

        public readonly string USE_ETHERIC_RUDDER_COUPLER_PROPERTY_NAME = "Rudder_UseEthericRudder";

        public bool defaultUseEthericRudder = true;

        public bool useEthericRudder
        {
            set
            {
                string id = GeneratePropertyId(USE_ETHERIC_RUDDER_COUPLER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_ETHERIC_RUDDER_COUPLER_PROPERTY_NAME);
                bool result = defaultUseEthericRudder;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseEthericRudder;
            }

        }

        #endregion

        #region kill forward

        public readonly string KILL_FORWARD_PROPERTY_NAME = "Rudder_KillForward";

        public bool defaultKillForward = false;

        public bool killForward
        {
            set
            {
                string id = GeneratePropertyId(KILL_FORWARD_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(KILL_FORWARD_PROPERTY_NAME);
                bool result = defaultKillForward;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultKillForward;
            }

        }

        #endregion

        #region kill sideways

        public readonly string KILL_SIDEWAYS_PROPERTY_NAME = "VelocityKiller_KillSideways";

        public bool defaultKillSideways = false;

        public bool killSideways
        {
            set
            {
                string id = GeneratePropertyId(KILL_SIDEWAYS_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(KILL_SIDEWAYS_PROPERTY_NAME);
                bool result = defaultKillSideways;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultKillSideways;
            }

        }

        #endregion

        #region kill up

        public readonly string KILL_UP_PROPERTY_NAME = "VelocityKiller_KillUp";

        public bool defaultKillUp = false;

        public bool killUp
        {
            set
            {
                string id = GeneratePropertyId(KILL_UP_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(KILL_UP_PROPERTY_NAME);
                bool result = defaultKillUp;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultKillUp;
            }

        }

        #endregion

        #region kill pitch

        public readonly string KILL_PITCH_PROPERTY_NAME = "VelocityKiller_KillPitch";

        public bool defaultKillPitch = false;

        public bool killPitch
        {
            set
            {
                string id = GeneratePropertyId(KILL_PITCH_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(KILL_PITCH_PROPERTY_NAME);
                bool result = defaultKillPitch;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultKillPitch;
            }

        }

        #endregion

        #region kill yaw

        public readonly string KILL_YAW_PROPERTY_NAME = "VelocityKiller_KillYaw";

        public bool defaultKillYaw = false;

        public bool killYaw
        {
            set
            {
                string id = GeneratePropertyId(KILL_YAW_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(KILL_YAW_PROPERTY_NAME);
                bool result = defaultKillYaw;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultKillYaw;
            }

        }

        #endregion

        #region kill roll

        public readonly string KILL_ROLL_PROPERTY_NAME = "VelocityKiller_KillRoll";

        public bool defaultKillRoll = false;

        public bool killRoll
        {
            set
            {
                string id = GeneratePropertyId(KILL_ROLL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(KILL_ROLL_PROPERTY_NAME);
                bool result = defaultKillRoll;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultKillRoll;
            }

        }

        #endregion

        #region use velocity killer

        public readonly string USE_VELOCITY_KILLER_PROPERTY_NAME = "VelocityKiller_UseVelocityKiller";

        public bool defaultUseVelocityKiller = false;

        public bool useVelocityKiller
        {
            set
            {
                string id = GeneratePropertyId(USE_VELOCITY_KILLER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_VELOCITY_KILLER_PROPERTY_NAME);
                bool result = defaultUseVelocityKiller;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseVelocityKiller;
            }

        }

        #endregion

        #region lifecycle

        public override void Initialize()
        {

        }

        public override void Destroy()
        {

        }

        #endregion      

        #region update

        public void UpdateAfterSimulation(double physicsDeltaTime)
        {

            if (useVelocityKiller)
            {
                UpdateVelocityKiller();
            }
        }


        void UpdateVelocityKiller()
        {

            MatrixD invWorldRot = block.CubeGrid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            MatrixD worldRot = block.CubeGrid.WorldMatrix.GetOrientation();

            if (killSideways || killForward || killSideways)
            {

                Vector3D localLinearVelocity = Vector3D.Transform(block.CubeGrid.Physics.LinearVelocity, invWorldRot);

                localLinearVelocity.X = killSideways ? 0 : localLinearVelocity.X;
                localLinearVelocity.Y = killUp ? 0 : localLinearVelocity.Y;
                localLinearVelocity.Z = killForward ? 0 : localLinearVelocity.Z;

                block.CubeGrid.Physics.LinearVelocity = Vector3.Transform(localLinearVelocity, worldRot);
            }

            if (killPitch || killYaw || killRoll)
            {

                Vector3D localAngularVelocity = Vector3D.Transform(block.CubeGrid.Physics.AngularVelocity, invWorldRot);

                localAngularVelocity.X = killPitch ? 0 : localAngularVelocity.X;
                localAngularVelocity.Y = killYaw ? 0 : localAngularVelocity.Y;
                localAngularVelocity.Z = killRoll ? 0 : localAngularVelocity.Z;

                block.CubeGrid.Physics.AngularVelocity = Vector3.Transform(localAngularVelocity, worldRot);
            }

        }

        #endregion

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Velocity Killer");
            customInfo.AppendLine("Enabled: " + useVelocityKiller);
            customInfo.AppendLine("Kill Forward: " + killForward);
            customInfo.AppendLine("Kill Up: " + killUp);
            customInfo.AppendLine("Kill Right: " + killSideways);
            customInfo.AppendLine("Kill Pitch: " + killPitch);
            customInfo.AppendLine("Kill Yaw: " + killYaw);
            customInfo.AppendLine("Kill Roll: " + killRoll);
            customInfo.AppendLine("MaxAngle: " + maxAngle);
        }

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }

            Vector3 blockPosition = block.GetPosition();

            Vector3D velocity = grid.Physics.LinearVelocity;
            Vector3D velocityDirection = velocity;
            velocityDirection.Normalize();

            Quaternion from = Quaternion.CreateFromForwardUp(velocityDirection, block.WorldMatrix.Up);
            Quaternion to = Quaternion.CreateFromForwardUp(block.WorldMatrix.Forward, block.WorldMatrix.Up);

            DebugDraw.DrawLine(blockPosition, blockPosition + from.Forward * 100, Color.Red, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + to.Forward * 100, Color.Green, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + velocity * 100, Color.Orange, 0.05f);
        }

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
