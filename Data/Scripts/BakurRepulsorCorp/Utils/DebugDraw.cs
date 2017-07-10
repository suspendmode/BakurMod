using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace BakurRepulsorCorp {

    public static class DebugDraw {

        public static void DrawLine(Vector3D from, Vector3D to, Color color, float radius) {
            string mat = "WeaponLaserIgnoreDepth";
            Vector4 vector = color.ToVector4();
            MySimpleObjectDraw.DrawLine(from, to, MyStringId.GetOrCompute(mat), ref vector, radius);
        }

        public static void DrawBox(Vector3D position, Vector3D forward, Vector3D up, Vector3D size, Color color, float radius) {
            string mat = "WeaponLaserIgnoreDepth";

            MatrixD worldMatrix = MatrixD.CreateWorld(position, forward, up);
            BoundingBoxD localBox = new BoundingBoxD(-size / 2, size / 2);
            MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localBox, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 24, 0.1f, MyStringId.GetOrCompute(mat), MyStringId.GetOrCompute(mat));
        }

        public static void DrawSphere(Vector3D position, Vector3D forward, Vector3D up, Color color, float radius) {
            string mat = "WeaponLaserIgnoreDepth";
            MatrixD worldMatrix = MatrixD.CreateWorld(position, forward, up);

            MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 24, MyStringId.GetOrCompute(mat), MyStringId.GetOrCompute(mat), 0.1f);
        }

        public static void PlanetRadarSphere(Vector3D point, Vector3D forward, Vector3D up, float radius, Color color) {
            int wireDivideRatio = 24;
            float lineThickness = 0.025f;
            string faceMaterial = "WeaponLaserIgnoreDepth";
            string lineMaterial = "WeaponLaserIgnoreDepth";
            MatrixD worldMatrix = MatrixD.CreateWorld(point, forward, up);
            MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref color, MySimpleObjectRasterizer.SolidAndWireframe, wireDivideRatio, MyStringId.GetOrCompute(faceMaterial), MyStringId.GetOrCompute(lineMaterial), lineThickness);
        }

        public static void PowerLine(Vector3D from, Vector3D to, int iterations, double range, Color color, float radius) {

            if (iterations < 2) {
                //MyAPIGateway.Utilities.ShowMessage("Draw", "it<2 " + iterations);
                DrawLine(from, to, color, radius);

            } else {

                Vector3D end = from;
                range = BakurMathHelper.Clamp01(range);

                for (int i = 1; i <= iterations; i++) {
                    Vector3D start = end;
                    end = Vector3D.Lerp(from, to, ((double)i / (double)iterations));
                    end += MyUtils.GetRandomVector3D() * range;
                    DrawLine(start, end, color, radius);
                }
            }
        }

        public static void DrawCross(Vector3D position, float length, Color color, float radius) {
            DrawLine(position - Vector3D.Up * length, position + Vector3D.Up * length, color, radius);
            DrawLine(position - Vector3D.Right * length, position + Vector3D.Right * length, color, radius);
            DrawLine(position - Vector3D.Forward * length, position + Vector3D.Forward * length, color, radius);
        }

        public static void GizmoDrawCross(Vector3D position, float length, float radius) {
            DrawLine(position - Vector3D.Up * length, position + Vector3D.Up * length, Color.Green, radius);
            DrawLine(position - Vector3D.Right * length, position + Vector3D.Right * length, Color.Red, radius);
            DrawLine(position - Vector3D.Forward * length, position + Vector3D.Forward * length, Color.Blue, radius);
        }
    }
}