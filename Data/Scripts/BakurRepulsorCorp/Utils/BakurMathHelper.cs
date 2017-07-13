using System;
using VRageMath;

namespace BakurRepulsorCorp {

    public static class BakurMathHelper {

        public const double Deg2Rad = 0.0174532924f;
        public const double Rad2Deg = 57.29578f;
        public static double HalfPI = 1.57079632679f;

        public static double Angle(Vector3D from, Vector3D to) {
            from.Normalize();
            to.Normalize();
            return Math.Acos(MathHelper.Clamp(Vector3D.Dot(from, to), -1f, 1f)) * BakurMathHelper.Rad2Deg;
        }

        public static Vector3 ComputePredictionOffset(Vector3D linearVelocity) {
            // since we scan every 10 frames + some speedup compensation
            return linearVelocity * 0.166f * 2f;
        }

        public static double SmoothDamp(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed, double deltaTime) {
            smoothTime = Math.Max(0.0001f, (float)smoothTime);
            double num = 2f / smoothTime;
            double num2 = num * deltaTime;
            double num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
            double num4 = current - target;
            double num5 = target;
            double num6 = maxSpeed * smoothTime;
            num4 = MathHelper.Clamp(num4, -num6, num6);
            target = current - num4;
            double num7 = (currentVelocity + num * num4) * deltaTime;
            currentVelocity = (currentVelocity - num * num7) * num3;
            double num8 = target + (num4 + num7) * num3;
            if (num5 - current > 0f == num8 > num5) {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }
            return num8;
        }

        public static string RandomDigits(int length) {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public static Quaternion RotateTowards(Quaternion from, Quaternion to, double maxDegreesDelta) {
            double num = Angle(from, to);
            Quaternion result;
            if (num == 0) {
                result = to;
            } else {
                float t = (float)Math.Min(1, maxDegreesDelta / num);
                result = Quaternion.Slerp(from, to, t);
            }
            return result;
        }

        public static double Angle(Quaternion a, Quaternion b) {
            double f = Quaternion.Dot(a, b);
            return Math.Acos(Math.Min(Math.Abs(f), 1)) * 2 * Rad2Deg;
        }

        public static float InverseLerp(float a, float b, float value) {
            float result;
            if (a != b) {
                result = Clamp01((value - a) / (b - a));
            } else {
                result = 0f;
            }
            return result;
        }

        public static double InverseLerp(double a, double b, double value) {
            double result;
            if (a != b) {
                result = Clamp01((value - a) / (b - a));
            } else {
                result = 0f;
            }
            return result;
        }

        public static double Clamp01(double value) {
            double result;
            if (value < 0f) {
                result = 0f;
            } else if (value > 1f) {
                result = 1f;
            } else {
                result = value;
            }
            return result;
        }

        public static float Clamp01(float value) {
            float result;
            if (value < 0f) {
                result = 0f;
            } else if (value > 1f) {
                result = 1f;
            } else {
                result = value;
            }
            return result;
        }

        public static Vector3D Project(Vector3D vector, Vector3D onNormal) {
            float num = Vector3.Dot(onNormal, onNormal);
            Vector3D result;

            if (num < 0.00000001f) {
                result = Vector3.Zero;
            } else {
                result = onNormal * Vector3.Dot(vector, onNormal) / num;
            }
            return result;
        }

        public static Vector3D ProjectOnPlane(Vector3D vector, Vector3D planeNormal) {
            return vector - Project(vector, planeNormal);
        }

        public static Vector3D GetNormal(Vector3D a, Vector3D b, Vector3D c) {

            Vector3D side1 = b - a;
            Vector3D side2 = c - a;

            Vector3D perp = Vector3.Cross(side1, side2);

            var perpLength = perp.Length();
            perp /= perpLength;
            return perp;
        }

        public static Vector3 CalculateSurfaceNormal(Vector3[] Polygon) {

            Vector3 Normal = Vector3.Zero;

            for (int i = 0; i < Polygon.Length; i++) {

                Vector3 Current = Polygon[i];
                Vector3 Next = Polygon[(i + 1) % Polygon.Length];

                Normal.X += (Current.Y - Next.Y) * (Current.Z + Next.Z);
                Normal.Y += (Current.Z - Next.Z) * (Current.X + Next.X);
                Normal.Z += (Current.X - Next.X) * (Current.Y + Next.Y);

            }

            Normal.Normalize();
            return Normal;
        }

        public static Vector3D GetPolygonNormal(Vector3D[] vertices) {
            Vector3D normal = Vector3D.Zero;
            Vector3D currVert, nextVert;

            for (int i = 0; i < vertices.Length; i++) {
                currVert = vertices[i];
                nextVert = vertices[((i + 1) % vertices.Length)];

                normal.X += (currVert.Y - nextVert.Y) * (currVert.Z + nextVert.Z);
                normal.Y += (currVert.Z - nextVert.Z) * (currVert.X + nextVert.X);
                normal.Z += (currVert.X - nextVert.X) * (currVert.Y + nextVert.Y);
            }
            normal.Normalize();
            return normal;
        }

        public static Quaternion AngleAxis(double degress, Vector3D axis) {
            if (axis.LengthSquared() == 0.0f)
                return Quaternion.Identity;

            Quaternion result = Quaternion.Identity;
            var radians = degress * BakurMathHelper.Deg2Rad;
            radians *= 0.5f;
            axis.Normalize();
            axis = axis * (float)Math.Sin(radians);
            result.X = (float)axis.X;
            result.Y = (float)axis.Y;
            result.Z = (float)axis.Z;
            result.W = (float)System.Math.Cos(radians);

            return Quaternion.Normalize(result);
        }

        public static Quaternion AxisAngle(Vector3 axis, float angle) {

            float radians = (float)(Deg2Rad * angle);

            float d = axis.Length();
            if (d == 0f)
                return Quaternion.Identity;
            d = 1f / d;
            float l_ang = radians < 0 ? ((float)Math.PI * 2) - (-radians % ((float)Math.PI * 2)) : radians % ((float)Math.PI * 2);
            float l_sin = (float)Math.Sin(l_ang / 2);
            float l_cos = (float)Math.Cos(l_ang / 2);

            Quaternion q = Quaternion.Identity;
            q.X = d * axis.X * l_sin;
            q.Y = d * axis.Y * l_sin;
            q.Z = d * axis.Z * l_sin;
            q.W = l_cos;
            q.Normalize();
            return q;
        }

        public static QuaternionD AxisAngle(Vector3D axis, double angle) {

            double radians = Deg2Rad * angle;

            double d = axis.Length();
            if (d == 0f)
                return QuaternionD.Identity;
            d = 1f / d;
            double l_ang = radians < 0 ? (Math.PI * 2) - (-radians % (Math.PI * 2)) : radians % (Math.PI * 2);
            double l_sin = (float)Math.Sin(l_ang / 2);
            double l_cos = (float)Math.Cos(l_ang / 2);

            QuaternionD q = QuaternionD.Identity;
            q.X = d * axis.X * l_sin;
            q.Y = d * axis.Y * l_sin;
            q.Z = d * axis.Z * l_sin;
            q.W = l_cos;
            q.Normalize();
            return q;
        }

        public static double GetGimbalPole(this Quaternion q) {
            double t = q.Y * q.X + q.Z * q.W;
            return t > 0.499 ? 1 : (t < -0.499 ? -1 : 0);
        }

        public static double GetRoll(this Quaternion q) {
            double pole = q.GetGimbalPole();
            double roll = pole == 0 ? Math.Atan2(2 * (q.W * q.Z + q.Y * q.X), 1f - 2f * (q.X * q.X + q.Z * q.Z)) : (float)pole * 2f * Math.Atan2(q.Y, q.W);
            return Rad2Deg * roll;
        }

        public static double GetPitch(this Quaternion q) {
            double pole = q.GetGimbalPole();
            double pitch = pole == 0 ? Math.Asin(MathHelper.Clamp(2f * (q.W * q.X - q.Z * q.Y), -1f, 1f)) : pole * Math.PI * 0.5;
            return Rad2Deg * pitch;
        }

        public static double GetYaw(this Quaternion q) {
            double pole = q.GetGimbalPole();
            double yaw = pole == 0 ? Math.Atan2(2f * (q.Y * q.W + q.X * q.Z), 1f - 2f * (q.Y * q.Y + q.X * q.X)) : 0f;
            return Rad2Deg * yaw;
        }
    }
}