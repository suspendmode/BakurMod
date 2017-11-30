using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class AltitudeSensor : EquipmentBase
    {

        public bool hasSurface = false;
        public double altitude = double.NaN;

        public Vector3D surfacePoint = Vector3D.Zero;

        public SensorHitInfo centerHit = new SensorHitInfo();

        public AltitudeSensor(BakurBlock component) : base(component)
        {
        }

        static Separator<AltitudeSensor> separator;
        static Label<AltitudeSensor> label;

        #region max distance

        static AltitudeSensor_MaxDistanceSlider maxDistanceSlider;

        public static string MAX_DISTANCE_PROPERTY_NAME = "AltitudeSensor_MaxDistance";

        public double defaultMaxDistance = AltitudeSensor_MaxDistanceSlider.maxValue;

        public double maxDistance
        {
            set
            {
                string id = GeneratatePropertyId(MAX_DISTANCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MAX_DISTANCE_PROPERTY_NAME);
                double result = defaultMaxDistance;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxDistance;
            }
        }

        #endregion

        void Clear()
        {
            hasSurface = false;
            altitude = double.NaN;
            surfacePoint = Vector3.Zero;
            centerHit.Clear();
        }

        #region lifecycle

        public override void Initialize()
        {
            if (separator == null)
            {
                separator = new Separator<AltitudeSensor>("AltitudeSensor_Separator");
                separator.Initialize();
            }

            if (label == null)
            {
                label = new Label<AltitudeSensor>("AltitudeSensor_Label", "Altitude Sensor");
                label.Initialize();
            }

            if (maxDistanceSlider == null)
            {
                maxDistanceSlider = new AltitudeSensor_MaxDistanceSlider();
                maxDistanceSlider.Initialize();
            }
        }

        public override void Destroy()
        {

        }

        #endregion

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Surface Sensor");
            customInfo.AppendLine("Has Surface: " + hasSurface);

            if (!double.IsNaN(altitude))
            {
                customInfo.AppendLine("Altitude: " + Math.Round(altitude, 1));
                customInfo.AppendLine("Hit: " + centerHit.hit);
            }
        }


        public void UpdateSensor()
        {

            double offset = block.LocalAABB.Size.Length();
            Vector3D up = block.WorldMatrix.Up;
            Vector3D down = block.WorldMatrix.Down;

            Vector3D from = block.WorldMatrix.Translation;// + up * offset;
            Vector3D to = from + down * (maxDistance + offset);

            centerHit.from = from;
            centerHit.to = to;

            BakurCastRayHelper.CastRayVoxelAndGrids(centerHit, block);

            hasSurface = centerHit.hit;
            surfacePoint = centerHit.hitPoint;

            if (!hasSurface)
            {
                altitude = double.NaN;
            }
            else
            {
                altitude = centerHit.distance;
            }
        }

        public override void Debug()
        {
            if (!component.debugEnabled)
            {
                return;
            }

            if (centerHit.hit)
            {
                DebugDraw.DrawLine(centerHit.from, centerHit.hitPoint, Color.Green, 0.05f);
            }
            else
            {
                DebugDraw.DrawLine(centerHit.from, centerHit.to, Color.Red, 0.05f);
            }
        }
    }
}
