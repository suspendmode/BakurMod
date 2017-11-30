using Sandbox.ModAPI;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class PlanetSurfaceNormalSensor : PlanetSensor
    {

        public Vector3D surfaceNormal = Vector3D.Zero;

        static Separator<PlanetSurfaceNormalSensor> separator;
        static Label<PlanetSurfaceNormalSensor> label;

        public PlanetSurfaceNormalSensor(LogicComponent component) : base(component)
        {
        }

        #region size front

        static PlanetSurfaceNormalSensor_SizeFrontSlider sizeFrontSlider;

        public static string SIZE_FRONT_PROPERTY_NAME = "PlanetSurfaceNormalSensor_SizeFront";

        public double defaultSizeFront = PlanetSurfaceNormalSensor_SizeFrontSlider.maxValue;

        public double sizeFront
        {
            set
            {
                string id = GeneratatePropertyId(SIZE_FRONT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(SIZE_FRONT_PROPERTY_NAME);
                double result = defaultSizeFront;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultSizeFront;
            }
        }

        #endregion

        #region size back

        static PlanetSurfaceNormalSensor_SizeBackSlider sizeBackSlider;

        public static string SIZE_BACK_PROPERTY_NAME = "PlanetSurfaceNormalSensor_SizeBack";

        public double defaultSizeBack = PlanetSurfaceNormalSensor_SizeBackSlider.minValue;

        public double sizeBack
        {
            set
            {
                string id = GeneratatePropertyId(SIZE_BACK_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(SIZE_BACK_PROPERTY_NAME);
                double result = defaultSizeBack;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultSizeBack;
            }
        }

        #endregion

        #region size left

        static PlanetSurfaceNormalSensor_SizeLeftSlider sizeLeftSlider;

        public static string SIZE_LEFT_PROPERTY_NAME = "PlanetSurfaceNormalSensor_SizeLeft";

        public double defaultSizeLeft = 1;

        public double sizeLeft
        {
            set
            {
                string id = GeneratatePropertyId(SIZE_LEFT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(SIZE_LEFT_PROPERTY_NAME);
                double result = defaultSizeLeft;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultSizeLeft;
            }
        }

        #endregion

        #region size right

        static PlanetSurfaceNormalSensor_SizeRightSlider sizeRightSlider;

        public static string SIZE_RIGHT_PROPERTY_NAME = "PlanetSurfaceNormalSensor_SizeRight";

        public double defaultSizeRight = 1;

        public double sizeRight
        {
            set
            {
                string id = GeneratatePropertyId(SIZE_RIGHT_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(SIZE_RIGHT_PROPERTY_NAME);
                double result = defaultSizeRight;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultSizeRight;
            }
        }

        #endregion

        public override void UpdateSensor(double physicsDeltaTime)
        {

            base.UpdateSensor(physicsDeltaTime);

            if (!logicComponent.rigidbody.IsInGravity || nearestPlanet == null)
            {
                surfaceNormal = Vector3D.Zero;
                return;
            }


            Vector3D center = grid.WorldAABB.Center;
            Vector3D blockUp = block.WorldMatrix.Up;
            Vector3D blockDown = block.WorldMatrix.Down;
            Vector3D blockForward = block.WorldMatrix.Forward;
            Vector3D blockRight = block.WorldMatrix.Right;

            double gridSideFront = grid.LocalAABB.Size.Z * sizeFront;
            double gridSideBack = grid.LocalAABB.Size.Z * sizeBack;
            double gridSideLeft = grid.LocalAABB.Size.X * sizeLeft;
            double gridSideRight = grid.LocalAABB.Size.X * sizeRight;

            double gridSideHeight = grid.LocalAABB.Size.Y;

            Vector3D upOffset = logicComponent.rigidbody.gravityUp * gridSideHeight;

            double forwardSpeed = Vector3D.Dot(blockForward, grid.Physics.LinearVelocity);
            //Vector3D front = center + upOffset + (blockForward * forwardSize) + (blockForward * forwardSpeed);
            //Vector3D back = center + upOffset - (blockForward * forwardSize) - (blockForward * forwardSpeed);
            Vector3D front = center + upOffset + (blockForward * gridSideFront);
            Vector3D back = center + upOffset - (blockForward * gridSideBack);
            Vector3D right = blockRight * gridSideRight;
            Vector3D left = -blockRight * gridSideLeft;

            //DebugDraw.DrawBox(center, blockForward, blockUp, grid.LocalAABB.Size, Color.Red, 0.05f);

            Vector3D frontLeft = front + left;
            Vector3D frontLeftSurfacePoint = nearestPlanet.GetClosestSurfacePointGlobal(ref frontLeft);

            if (logicComponent.debugEnabled)
            {
                DebugDraw.DrawLine(frontLeft, frontLeftSurfacePoint, Color.Blue, 0.1f);
            }

            Vector3D frontRight = front + right;
            Vector3D frontRightSurfacePoint = nearestPlanet.GetClosestSurfacePointGlobal(ref frontRight);
            if (logicComponent.debugEnabled)
            {
                DebugDraw.DrawLine(frontRight, frontRightSurfacePoint, Color.Blue, 0.1f);
            }

            Vector3D rearLeft = back + left;
            Vector3D rearLeftSurfacePoint = nearestPlanet.GetClosestSurfacePointGlobal(ref rearLeft);
            if (logicComponent.debugEnabled)
            {
                DebugDraw.DrawLine(rearLeft, rearLeftSurfacePoint, Color.Blue, 0.1f);
            }

            Vector3D rearRight = back + right;
            Vector3D rearRightSurfacePoint = nearestPlanet.GetClosestSurfacePointGlobal(ref rearRight);
            if (logicComponent.debugEnabled)
            {
                DebugDraw.DrawLine(rearRight, rearRightSurfacePoint, Color.Blue, 0.1f);
            }

            Vector3D centerSurfacePoint = Vector3D.Zero;
            int pointsCount = 0;
            if (frontLeftSurfacePoint != Vector3D.Zero)
            {
                pointsCount++;
                centerSurfacePoint += frontLeftSurfacePoint;
            }
            if (frontRightSurfacePoint != Vector3D.Zero)
            {
                pointsCount++;
                centerSurfacePoint += frontRightSurfacePoint;
            }
            if (rearLeftSurfacePoint != Vector3D.Zero)
            {
                pointsCount++;
                centerSurfacePoint += rearLeftSurfacePoint;
            }
            if (rearRightSurfacePoint != Vector3D.Zero)
            {
                pointsCount++;
                centerSurfacePoint += rearRightSurfacePoint;
            }

            if (pointsCount > 2)
            {
                centerSurfacePoint /= pointsCount;
                surfaceNormal = BakurMathHelper.CalculateSurfaceNormal(
                    new Vector3[] {
                        rearLeftSurfacePoint,
                        rearRightSurfacePoint,
                        frontRightSurfacePoint,
                        frontLeftSurfacePoint,
                });
                surfaceNormal.Normalize();
            }
            else
            {
                surfaceNormal = logicComponent.rigidbody.gravityUp;
            }

            bool outOfAngle = (BakurMathHelper.Angle(logicComponent.rigidbody.gravityUp, surfaceNormal)) > 89;

            if (outOfAngle)
            {
                surfaceNormal = logicComponent.rigidbody.gravityUp;
            }

            //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "angle: " + Angle(gravityUp, surfaceNormal));

        }



        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Planet Surface Normal Sensor");
            customInfo.AppendLine("IsInGravity: " + logicComponent.rigidbody.IsInGravity);
            customInfo.AppendLine("Planets: " + PlanetsList.planets.Count);
            customInfo.AppendLine("Nearest Planet: " + (nearestPlanet != null ? nearestPlanet.Name : "None"));
            customInfo.AppendLine("Normal: " + surfaceNormal);
        }

        #region lifecycle

        public override void Initialize()
        {

            if (separator == null)
            {
                separator = new Separator<PlanetSurfaceNormalSensor>("PlanetSurfaceNormalSensor_Separator");
                separator.Initialize();
            }

            if (label == null)
            {
                label = new Label<PlanetSurfaceNormalSensor>("PlanetSurfaceNormalSensor_Label", "Planet Surface Normal");
                label.Initialize();
            }

            if (sizeFrontSlider == null)
            {
                sizeFrontSlider = new PlanetSurfaceNormalSensor_SizeFrontSlider();
                sizeFrontSlider.Initialize();
            }
            if (sizeBackSlider == null)
            {
                sizeBackSlider = new PlanetSurfaceNormalSensor_SizeBackSlider();
                sizeBackSlider.Initialize();
            }
            if (sizeLeftSlider == null)
            {
                sizeLeftSlider = new PlanetSurfaceNormalSensor_SizeLeftSlider();
                sizeLeftSlider.Initialize();
            }
            if (sizeRightSlider == null)
            {
                sizeRightSlider = new PlanetSurfaceNormalSensor_SizeRightSlider();
                sizeRightSlider.Initialize();
            }
        }

        public override void Destroy()
        {
            Clear();
        }

        protected override void Clear()
        {
            base.Clear();
            surfaceNormal = Vector3D.Zero;
        }

        #endregion

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }

            Vector3D center = grid.WorldAABB.Center;
            bool outOfAngle = BakurMathHelper.Angle(logicComponent.rigidbody.gravityUp, surfaceNormal) > 89;
            if (outOfAngle)
            {
                if (logicComponent.debugEnabled)
                {
                    DebugDraw.DrawLine(center, center + surfaceNormal * 50, Color.Red, 0.2f);
                }
            }
            else
            {
                if (logicComponent.debugEnabled)
                {
                    DebugDraw.DrawLine(center, center + surfaceNormal * 50, Color.Cyan, 0.05f);
                }
            }
        }
    }
}
