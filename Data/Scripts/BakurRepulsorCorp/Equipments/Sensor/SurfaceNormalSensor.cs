using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class SurfaceNormalSensor : EquipmentBase {

        public Vector3D surfaceNormal = Vector3D.Zero;
        public Vector3D surfacePoint = Vector3D.Zero;
        public bool hasSurface = false;
        public double altitude = 0;
        public double maxDistance = 10000;

        public SensorHitInfo frontLeftHit = new SensorHitInfo();
        public SensorHitInfo frontRightHit = new SensorHitInfo();
        public SensorHitInfo rearLeftHit = new SensorHitInfo();
        public SensorHitInfo rearRightHit = new SensorHitInfo();

        public SurfaceNormalSensor(BakurBlock block) : base(block) {
        }

        #region lifecycle

        public override void Initialize() {

        }

        public override void Destroy() {

        }

        #endregion

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Surface Normal Sensor ==");
            customInfo.AppendLine("Has Surface : " + hasSurface);
            customInfo.AppendLine("Altitude : " + Math.Round(altitude, 1));
            customInfo.AppendLine("Front Left Hit : " + frontLeftHit.hit);
            customInfo.AppendLine("Front Right Hit : " + frontRightHit.hit);
            customInfo.AppendLine("Rear Left Hit : " + rearLeftHit.hit);
            customInfo.AppendLine("Rear Right Hit : " + rearRightHit.hit);
        }

        public void UpdateSensor() {

            Vector3D forward = block.WorldMatrix.Forward;
            Vector3D right = block.WorldMatrix.Right;
            IMyCubeGrid grid = block.CubeGrid;

            Vector3D center = grid.WorldAABB.Center;
            Vector3D down = grid.WorldMatrix.Down;
            Vector3D Size = grid.LocalAABB.Size;

            double scale = 0.5f;

            double forwardSpeed = Vector3.Dot(grid.Physics.LinearVelocity, forward);
            // Vector3D predicedPostition = BakurMathHelper.ProjectOnPlane(grid.Physics.LinearVelocity, -down) * (deltaTime * 2);
            //  center += predicedPostition;

            double forwardSize = (Size.Z * scale) + (forwardSpeed * scale);
            double sideSize = Size.X * scale;

            frontLeftHit.from = center + (forward * forwardSize + -right * sideSize);
            frontLeftHit.to = frontLeftHit.from + down * maxDistance;

            frontRightHit.from = center + (forward * forwardSize + right * sideSize);
            frontRightHit.to = frontRightHit.from + down * maxDistance;

            rearLeftHit.from = center + (-forward * forwardSize + -right * sideSize);
            rearLeftHit.to = rearLeftHit.from + down * maxDistance;

            rearRightHit.from = center + (-forward * forwardSize + right * sideSize);
            rearRightHit.to = rearRightHit.from + down * maxDistance;

            BakurCastRayHelper.CastRayVoxelAndGrids(frontLeftHit, block);
            BakurCastRayHelper.CastRayVoxelAndGrids(frontRightHit, block);
            BakurCastRayHelper.CastRayVoxelAndGrids(rearLeftHit, block);
            BakurCastRayHelper.CastRayVoxelAndGrids(rearRightHit, block);

            int count = 0;
            if (frontLeftHit.hit) {
                count++;
            }
            if (frontRightHit.hit) {
                count++;
            }
            if (rearLeftHit.hit) {
                count++;
            }
            if (rearRightHit.hit) {
                count++;
            }

            hasSurface = count >= 3;

            if (hasSurface) {

                surfaceNormal = BakurMathHelper.CalculateSurfaceNormal(
                    new Vector3[] {
                        frontLeftHit.hitPoint,
                        frontRightHit.hitPoint,
                        rearRightHit.hitPoint,
                        rearLeftHit.hitPoint,
                });
                surfaceNormal.Normalize();

                //average altitude
                int c = 0;
                altitude = 0;
                if (frontLeftHit.hit) {
                    altitude += frontLeftHit.distance;
                    c++;
                }
                if (frontRightHit.hit) {
                    altitude += frontRightHit.distance;
                    c++;
                }
                if (rearLeftHit.hit) {
                    altitude += rearLeftHit.distance;
                    c++;
                }
                if (rearRightHit.hit) {
                    altitude += rearRightHit.distance;
                    c++;
                }
                if (c > 1) {
                    altitude /= (double)c;
                }
            } else {
                altitude = 0;
                surfaceNormal = Vector3D.Zero;
            }
        }

        void Clear() {

            hasSurface = false;
            altitude = 0;
            surfacePoint = Vector3.Zero;

            surfaceNormal = Vector3D.Zero;

            frontLeftHit.Clear();
            frontRightHit.Clear();
            rearLeftHit.Clear();
            rearRightHit.Clear();
        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }
        }
    }
}