using VRageMath;

namespace BakurRepulsorCorp {

    public class SensorHitInfo {

        public Vector3D from;
        public Vector3D to;
        public bool hit;
        public Vector3D hitPoint;
        public double distance;

        public void Clear() {
            from = Vector3D.Zero;
            to = Vector3D.Zero;
            hit = false;
            hitPoint = Vector3D.Zero;
            distance = 0;
        }
    }
}