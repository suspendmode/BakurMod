using Sandbox.ModAPI;
using System.Text;
using VRageMath;
using System;

namespace BakurRepulsorCorp {

    public class GyroStabiliser : BakurBlockEquipment {

        public GyroStabiliser(BakurBlock component) : base(component) {
        }

        static Separator<GyroStabiliser> gyroStabiliserSeparator;
        static Label<GyroStabiliser> gyroStabiliserLabel;

        #region use gravity normal

        static Gyro_UseGravityNormalSwitch useGravityNormalSwitch;
        static Gyro_UseGravityNormalToggleAction useGravityNormalToggleAction;
        static Gyro_UseGravityNormalEnableAction useGravityNormalEnableAction;
        static Gyro_UseGravityNormalDisableAction useGravityNormalDisableAction;

        public static string USE_GRAVITY_NORMAL_PROPERTY_NAME = "GyroStabiliser_UseGravityNormal";

        public bool defaultUseGravityNormal = false;

        public bool useGravityNormal
        {
            set
            {
                string id = GeneratatePropertyId(USE_GRAVITY_NORMAL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_GRAVITY_NORMAL_PROPERTY_NAME);
                bool result = defaultUseGravityNormal;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseGravityNormal;
            }

        }

        #endregion       

        #region use surface normal

        static Gyro_UseSurfaceNormalSwitch useSurfaceNormalSwitch;
        static Gyro_UseSurfaceNormalToggleAction useSurfaceNormalToggleAction;
        static Gyro_UseSurfaceNormalEnableAction useSurfaceNormalEnableAction;
        static Gyro_UseSurfaceNormalDisableAction useSurfaceNormalDisableAction;

        public static string USE_SURFACE_NORMAL_PROPERTY_NAME = "GyroStabiliser_UseSurfaceNormal";

        public bool defaultUseSurfaceNormal = true;

        public bool useSurfaceNormal
        {
            set
            {
                string id = GeneratatePropertyId(USE_SURFACE_NORMAL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_SURFACE_NORMAL_PROPERTY_NAME);
                bool result = defaultUseSurfaceNormal;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseSurfaceNormal;
            }

        }

        #endregion

        #region lifecycle

        public override void Initialize() {

            // gravity

            if (gyroStabiliserSeparator == null) {
                gyroStabiliserSeparator = new Separator<GyroStabiliser>("GyroStabiliser_GyroStabiliserSeparator");
                gyroStabiliserSeparator.Initialize();
            }

            if (gyroStabiliserLabel == null) {
                gyroStabiliserLabel = new Label<GyroStabiliser>("GyroStabiliser_GyroStabiliserLabel", "Gyro Stabiliser");
                gyroStabiliserLabel.Initialize();
            }


            if (useGravityNormalSwitch == null) {
                useGravityNormalSwitch = new Gyro_UseGravityNormalSwitch();
                useGravityNormalSwitch.Initialize();
            }

            if (useGravityNormalToggleAction == null) {
                useGravityNormalToggleAction = new Gyro_UseGravityNormalToggleAction();
                useGravityNormalToggleAction.Initialize();
            }

            if (useGravityNormalEnableAction == null) {
                useGravityNormalEnableAction = new Gyro_UseGravityNormalEnableAction();
                useGravityNormalEnableAction.Initialize();
            }

            if (useGravityNormalDisableAction == null) {
                useGravityNormalDisableAction = new Gyro_UseGravityNormalDisableAction();
                useGravityNormalDisableAction.Initialize();
            }


            // surface            

            if (useSurfaceNormalSwitch == null) {
                useSurfaceNormalSwitch = new Gyro_UseSurfaceNormalSwitch();
                useSurfaceNormalSwitch.Initialize();
            }

            if (useSurfaceNormalToggleAction == null) {
                useSurfaceNormalToggleAction = new Gyro_UseSurfaceNormalToggleAction();
                useSurfaceNormalToggleAction.Initialize();
            }

            if (useSurfaceNormalEnableAction == null) {
                useSurfaceNormalEnableAction = new Gyro_UseSurfaceNormalEnableAction();
                useSurfaceNormalEnableAction.Initialize();
            }

            if (useSurfaceNormalDisableAction == null) {
                useSurfaceNormalDisableAction = new Gyro_UseSurfaceNormalDisableAction();
                useSurfaceNormalDisableAction.Initialize();
            }

        }

        public override void Destroy() {

        }

        #endregion

        #region visuals

        override public void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Gyroscopic Stabiliser ==");
            customInfo.AppendLine("Use Gravity Normal : " + useGravityNormal);
            customInfo.AppendLine("Use Surface Normal : " + useSurfaceNormal);
            customInfo.AppendLine("Desired Up : " + desiredUp);
        }

        #endregion

        #region output

        public Vector3 desiredUp;
        public Vector3 surfaceNormal;

        public Vector3 GetDesiredUp(Vector3 surfaceNormal) {

            this.surfaceNormal = surfaceNormal;

            if (!component.IsInGravity) {
                desiredUp = block.WorldMatrix.Up;
                return desiredUp;
            }

            //MyLog.Default.WriteLine(block.Name + ", stabilise");

            // up

            if (useGravityNormal && !useSurfaceNormal) {
                desiredUp = component.gravityUp;
                desiredUp.Normalize();
            } else if (useSurfaceNormal && !useGravityNormal) {
                desiredUp = surfaceNormal;
                desiredUp.Normalize();
            } else if (useSurfaceNormal && useGravityNormal) {
                desiredUp = (surfaceNormal + component.gravityUp) / 2;
                desiredUp.Normalize();
            } else {
                desiredUp = Vector3D.Zero;
            }

            return desiredUp;
        }

        public override void Debug() {

            base.Debug();

            if (!component.debugEnabled) {
                return;
            }

            Vector3D center = block.WorldAABB.Center;
            DebugDraw.DrawLine(center, center + desiredUp * 20, Color.DarkKhaki, 0.03f);
        }

        #endregion

    }
}