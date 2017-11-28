using Sandbox.ModAPI;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class VelocityKiller : BakurBlockEquipment
    {

        #region max angle

        public static VelocityRudder_MaxAngleSlider maxAngleSlider;
        static VelocityRudder_IncraseMaxAngleAction incraseMaxAngleAction;
        static VelocityRudder_DecraseMaxAngleAction decraseMaxAngleAction;

        public static string MAX_ANGLE_PROPERTY_NAME = "EthericRudder_MaxAngle";

        public double defaultMaxDegrees = 1;

        public VelocityKiller(BakurBlock block) : base(block)
        {
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
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxDegrees;
            }

        }

        #endregion

        static Separator<VelocityKiller> velocityKillerSeparator;
        static Label<VelocityKiller> velocityKillerLabel;

        #region use eheric rudder

        static VelocityRudder_UseVelocityRudderSwitch useEthericRudderSwitch;
        static VelocityRudder_UseVelocityRudderToggleAction useEthericRudderToggleAction;
        static VelocityRudder_UseVelocityRudderEnableAction useEthericRudderEnableAction;
        static VelocityRudder_UseVelocityRudderDisableAction useEthericRudderDisableAction;

        public static string USE_ETHERIC_RUDDER_COUPLER_PROPERTY_NAME = "Rudder_UseEthericRudder";

        public bool defaultUseEthericRudder = true;

        public bool useEthericRudder
        {
            set
            {
                string id = GeneratatePropertyId(USE_ETHERIC_RUDDER_COUPLER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_ETHERIC_RUDDER_COUPLER_PROPERTY_NAME);
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

        static VelocityKiller_KillForwardSwitch killForwardSwitch;
        static VelocityKiller_KillForwardToggleAction killForwardToggleAction;
        static VelocityKiller_KillForwardEnableAction killForwardEnableAction;
        static VelocityKiller_KillForwardDisableAction killForwardDisableAction;

        public static string KILL_FORWARD_PROPERTY_NAME = "Rudder_KillForward";

        public bool defaultKillForward = false;

        public bool killForward
        {
            set
            {
                string id = GeneratatePropertyId(KILL_FORWARD_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(KILL_FORWARD_PROPERTY_NAME);
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

        static VelocityKiller_KillSidewaysSwitch killSidewaysSwitch;
        static VelocityKiller_KillSidewaysToggleAction killSidewaysToggleAction;
        static VelocityKiller_KillSidewaysEnableAction killSidewaysEnableAction;
        static VelocityKiller_KillSidewaysDisableAction killSidewaysDisableAction;

        public static string KILL_SIDEWAYS_PROPERTY_NAME = "VelocityKiller_KillSideways";

        public bool defaultKillSideways = false;

        public bool killSideways
        {
            set
            {
                string id = GeneratatePropertyId(KILL_SIDEWAYS_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(KILL_SIDEWAYS_PROPERTY_NAME);
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

        static VelocityKiller_KillUpSwitch killUpSwitch;
        static VelocityKiller_KillUpToggleAction killUpToggleAction;
        static VelocityKiller_KillUpEnableAction killUpEnableAction;
        static VelocityKiller_KillUpDisableAction killUpDisableAction;

        public static string KILL_UP_PROPERTY_NAME = "VelocityKiller_KillUp";

        public bool defaultKillUp = false;

        public bool killUp
        {
            set
            {
                string id = GeneratatePropertyId(KILL_UP_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(KILL_UP_PROPERTY_NAME);
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

        static VelocityKiller_KillPitchSwitch killPitchSwitch;
        static VelocityKiller_KillPitchToggleAction killPitchToggleAction;
        static VelocityKiller_KillPitchEnableAction killPitchEnableAction;
        static VelocityKiller_KillPitchDisableAction killPitchDisableAction;

        public static string KILL_PITCH_PROPERTY_NAME = "VelocityKiller_KillPitch";

        public bool defaultKillPitch = false;

        public bool killPitch
        {
            set
            {
                string id = GeneratatePropertyId(KILL_PITCH_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(KILL_PITCH_PROPERTY_NAME);
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

        static VelocityKiller_KillYawSwitch killYawSwitch;
        static VelocityKiller_KillYawToggleAction killYawToggleAction;
        static VelocityKiller_KillYawEnableAction killYawEnableAction;
        static VelocityKiller_KillYawDisableAction killYawDisableAction;

        public static string KILL_YAW_PROPERTY_NAME = "VelocityKiller_KillYaw";

        public bool defaultKillYaw = false;

        public bool killYaw
        {
            set
            {
                string id = GeneratatePropertyId(KILL_YAW_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(KILL_YAW_PROPERTY_NAME);
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

        static VelocityKiller_KillRollSwitch killRollSwitch;
        static VelocityKiller_KillRollToggleAction killRollToggleAction;
        static VelocityKiller_KillRollEnableAction killRollEnableAction;
        static VelocityKiller_KillRollDisableAction killRollDisableAction;

        public static string KILL_ROLL_PROPERTY_NAME = "VelocityKiller_KillRoll";

        public bool defaultKillRoll = false;

        public bool killRoll
        {
            set
            {
                string id = GeneratatePropertyId(KILL_ROLL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(KILL_ROLL_PROPERTY_NAME);
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

        static VelocityKiller_UseVelocityKillerSwitch useVelocityKillerSwitch;
        static VelocityKiller_UseVelocityKillerToggleAction useVelocityKillerToggleAction;
        static VelocityKiller_UseVelocityKillerEnableAction useVelocityKillerEnableAction;
        static VelocityKiller_UseVelocityKillerDisableAction useVelocityKillerDisableAction;

        public static string USE_VELOCITY_KILLER_PROPERTY_NAME = "VelocityKiller_UseVelocityKiller";

        public bool defaultUseVelocityKiller = false;

        public bool useVelocityKiller
        {
            set
            {
                string id = GeneratatePropertyId(USE_VELOCITY_KILLER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_VELOCITY_KILLER_PROPERTY_NAME);
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

            // velocity killer

            if (velocityKillerSeparator == null)
            {
                velocityKillerSeparator = new Separator<VelocityKiller>("VelocityKiller_VelocityKillerSeparator");
                velocityKillerSeparator.Initialize();
            }

            if (velocityKillerLabel == null)
            {
                velocityKillerLabel = new Label<VelocityKiller>("VelocityKiller_VelocityKillerLabel", "Velocity Killer");
                velocityKillerLabel.Initialize();
            }

            if (useVelocityKillerSwitch == null)
            {
                useVelocityKillerSwitch = new VelocityKiller_UseVelocityKillerSwitch();
                useVelocityKillerSwitch.Initialize();
            }

            if (useVelocityKillerToggleAction == null)
            {
                useVelocityKillerToggleAction = new VelocityKiller_UseVelocityKillerToggleAction();
                useVelocityKillerToggleAction.Initialize();
            }

            if (useVelocityKillerEnableAction == null)
            {
                useVelocityKillerEnableAction = new VelocityKiller_UseVelocityKillerEnableAction();
                useVelocityKillerEnableAction.Initialize();
            }

            if (useVelocityKillerDisableAction == null)
            {
                useVelocityKillerDisableAction = new VelocityKiller_UseVelocityKillerDisableAction();
                useVelocityKillerDisableAction.Initialize();
            }

            // foward

            if (killForwardSwitch == null)
            {
                killForwardSwitch = new VelocityKiller_KillForwardSwitch();
                killForwardSwitch.Initialize();
            }

            if (killForwardToggleAction == null)
            {
                killForwardToggleAction = new VelocityKiller_KillForwardToggleAction();
                killForwardToggleAction.Initialize();
            }

            if (killForwardEnableAction == null)
            {
                killForwardEnableAction = new VelocityKiller_KillForwardEnableAction();
                killForwardEnableAction.Initialize();
            }

            if (killForwardDisableAction == null)
            {
                killForwardDisableAction = new VelocityKiller_KillForwardDisableAction();
                killForwardDisableAction.Initialize();

            }

            // sideways

            if (killSidewaysSwitch == null)
            {
                killSidewaysSwitch = new VelocityKiller_KillSidewaysSwitch();
                killSidewaysSwitch.Initialize();
            }

            if (killSidewaysToggleAction == null)
            {
                killSidewaysToggleAction = new VelocityKiller_KillSidewaysToggleAction();
                killSidewaysToggleAction.Initialize();
            }

            if (killSidewaysEnableAction == null)
            {
                killSidewaysEnableAction = new VelocityKiller_KillSidewaysEnableAction();
                killSidewaysEnableAction.Initialize();
            }

            if (killSidewaysDisableAction == null)
            {
                killSidewaysDisableAction = new VelocityKiller_KillSidewaysDisableAction();
                killSidewaysDisableAction.Initialize();
            }

            // up

            if (killUpSwitch == null)
            {
                killUpSwitch = new VelocityKiller_KillUpSwitch();
                killUpSwitch.Initialize();
            }

            if (killUpToggleAction == null)
            {
                killUpToggleAction = new VelocityKiller_KillUpToggleAction();
                killUpToggleAction.Initialize();
            }

            if (killUpEnableAction == null)
            {
                killUpEnableAction = new VelocityKiller_KillUpEnableAction();
                killUpEnableAction.Initialize();
            }

            if (killUpDisableAction == null)
            {
                killUpDisableAction = new VelocityKiller_KillUpDisableAction();
                killUpDisableAction.Initialize();
            }

            // pitch

            if (killPitchSwitch == null)
            {
                killPitchSwitch = new VelocityKiller_KillPitchSwitch();
                killPitchSwitch.Initialize();
            }

            if (killPitchToggleAction == null)
            {
                killPitchToggleAction = new VelocityKiller_KillPitchToggleAction();
                killPitchToggleAction.Initialize();
            }

            if (killPitchEnableAction == null)
            {
                killPitchEnableAction = new VelocityKiller_KillPitchEnableAction();
                killPitchEnableAction.Initialize();
            }

            if (killPitchDisableAction == null)
            {
                killPitchDisableAction = new VelocityKiller_KillPitchDisableAction();
                killPitchDisableAction.Initialize();
            }

            // yaw

            if (killYawSwitch == null)
            {
                killYawSwitch = new VelocityKiller_KillYawSwitch();
                killYawSwitch.Initialize();
            }

            if (killYawToggleAction == null)
            {
                killYawToggleAction = new VelocityKiller_KillYawToggleAction();
                killYawToggleAction.Initialize();
            }

            if (killYawEnableAction == null)
            {
                killYawEnableAction = new VelocityKiller_KillYawEnableAction();
                killYawEnableAction.Initialize();
            }

            if (killYawDisableAction == null)
            {
                killYawDisableAction = new VelocityKiller_KillYawDisableAction();
                killYawDisableAction.Initialize();
            }

            // roll

            if (killRollSwitch == null)
            {
                killRollSwitch = new VelocityKiller_KillRollSwitch();
                killRollSwitch.Initialize();
            }

            if (killRollToggleAction == null)
            {
                killRollToggleAction = new VelocityKiller_KillRollToggleAction();
                killRollToggleAction.Initialize();
            }

            if (killRollEnableAction == null)
            {
                killRollEnableAction = new VelocityKiller_KillRollEnableAction();
                killRollEnableAction.Initialize();
            }

            if (killRollDisableAction == null)
            {
                killRollDisableAction = new VelocityKiller_KillRollDisableAction();
                killRollDisableAction.Initialize();
            }
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
            customInfo.AppendLine("== Velocity Killer ==");
            customInfo.AppendLine("Enabled: " + useVelocityKiller);
            customInfo.AppendLine("Kill Forward : " + killForward);
            customInfo.AppendLine("Kill Up : " + killUp);
            customInfo.AppendLine("Kill Right : " + killSideways);
            customInfo.AppendLine("Kill Pitch : " + killPitch);
            customInfo.AppendLine("Kill Yaw : " + killYaw);
            customInfo.AppendLine("Kill Roll : " + killRoll);
        }

        public override void Debug()
        {
            if (!component.debugEnabled)
            {
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
