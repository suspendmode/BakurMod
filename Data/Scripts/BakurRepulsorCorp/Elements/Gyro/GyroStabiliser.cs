using Sandbox.ModAPI;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class GyroStabiliser : LogicElement
    {

        public GyroStabiliser(LogicComponent component) : base(component)
        {
        }

        #region use gravity normal

        public readonly string USE_GRAVITY_NORMAL_PROPERTY_NAME = "GyroStabiliser_UseGravityNormal";

        public bool defaultUseGravityNormal = false;

        public bool useGravityNormal
        {
            set
            {
                string id = GeneratePropertyId(USE_GRAVITY_NORMAL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_GRAVITY_NORMAL_PROPERTY_NAME);
                bool result = defaultUseGravityNormal;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseGravityNormal;
            }

        }

        #endregion       

        #region use surface normal

        public readonly string USE_SURFACE_NORMAL_PROPERTY_NAME = "GyroStabiliser_UseSurfaceNormal";

        public bool defaultUseSurfaceNormal = true;

        public bool useSurfaceNormal
        {
            set
            {
                string id = GeneratePropertyId(USE_SURFACE_NORMAL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_SURFACE_NORMAL_PROPERTY_NAME);
                bool result = defaultUseSurfaceNormal;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseSurfaceNormal;
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

        #region visuals

        override public void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
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

        public Vector3 GetDesiredUp(Vector3 surfaceNormal)
        {

            this.surfaceNormal = surfaceNormal;

            if (!logicComponent.rigidbody.IsInGravity)
            {
                desiredUp = block.WorldMatrix.Up;
                return desiredUp;
            }

            //MyLog.Default.WriteLine(block.Name + ", stabilise");

            // up

            if (useGravityNormal && !useSurfaceNormal)
            {
                desiredUp = logicComponent.rigidbody.gravityUp;
                desiredUp.Normalize();
            }
            else if (useSurfaceNormal && !useGravityNormal)
            {
                desiredUp = surfaceNormal;
                desiredUp.Normalize();
            }
            else if (useSurfaceNormal && useGravityNormal)
            {
                desiredUp = (surfaceNormal + logicComponent.rigidbody.gravityUp) / 2;
                desiredUp.Normalize();
            }
            else
            {
                desiredUp = Vector3D.Zero;
            }

            return desiredUp;
        }

        public override void Debug()
        {

            if (!logicComponent.debugEnabled)
            {
                return;
            }

            Vector3D center = block.WorldAABB.Center;
            DebugDraw.DrawLine(center, center + desiredUp * 20, Color.DarkKhaki, 0.03f);
        }

        #endregion

    }

}
