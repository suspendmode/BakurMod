using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;


namespace BakurRepulsorCorp {

    public class BakurCastRayHelper {

        public static bool CastRayVoxelAndGrids(SensorHitInfo sensorHitInfo, IMyCubeBlock block) {
           // int voxelCollisionLayer = MyAPIGateway.Physics.GetCollisionLayer("VoxelCollisionLayer");
            int collisionLayerWithoutCharacter = MyAPIGateway.Physics.GetCollisionLayer("CollisionLayerWithoutCharacter");
            //int raycastCollisionFilter = voxelCollisionLayer | collisionLayerWithoutCharacter;
            return CastRayMultilayer(sensorHitInfo, block, collisionLayerWithoutCharacter);
        }

        public static bool CastRayVoxel(SensorHitInfo sensorHitInfo, IMyCubeBlock block) {
            int voxelCollisionLayer = MyAPIGateway.Physics.GetCollisionLayer("VoxelCollisionLayer");
            return CastRayMultilayer(sensorHitInfo, block, voxelCollisionLayer);
        }

        public static bool CastRayMultilayer(SensorHitInfo sensorHitInfo, IMyCubeBlock block, int raycastCollisionFilter) {

            List<IHitInfo> hitInfos = new List<IHitInfo>();            

            MyAPIGateway.Physics.CastRay(sensorHitInfo.from, sensorHitInfo.to, hitInfos, raycastCollisionFilter);
            sensorHitInfo.hit = false;

            List<IHitInfo> targets = new List<IHitInfo>();
            foreach (IHitInfo info in hitInfos) {

                if (info.HitEntity.EntityId == block.EntityId) {
                    continue;
                }
                if (info.HitEntity.EntityId == block.CubeGrid.EntityId) {
                    continue;
                }
                if (info.HitEntity.Parent != null && info.HitEntity.Parent.EntityId == block.CubeGrid.EntityId) {
                    continue;
                }
                if (info.HitEntity.Parent != null && info.HitEntity.Parent.EntityId == block.EntityId) {
                    continue;
                }
                targets.Add(info);
            }

            if (targets.Count > 0) {
                sensorHitInfo.hit = true;
                sensorHitInfo.hitPoint = targets[0].Position;
                sensorHitInfo.distance = Vector3D.Distance(sensorHitInfo.from, sensorHitInfo.hitPoint);
                return true;
            } else {
                sensorHitInfo.hit = false;
                sensorHitInfo.hitPoint = Vector3.Zero;
                sensorHitInfo.distance = 0;
                return false;
            }
        }
    }
}