using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;
using System;

namespace BakurRepulsorCorp {


    public class PowerCoupling : EquipmentBase {

        public PowerCoupling(BakurBlock block) : base(block) {
        }

        public static List<PowerCoupling> powerCouplingTransceivers = new List<PowerCoupling>();

        Vector3D output;

        public static double maxPower = 100000;        

        #region distance

        public static PowerCoupler_DesiredDistanceSlider desiredDistanceSlider;
        static PowerCoupler_DecraseDesiredDistanceAction decraseDesiredDistanceAction;
        static PowerCoupler_IncraseDesiredDistanceAction incraseDesiredDistanceAction;

        public static string DESIRED_DISTANCE_PROPERTY_NAME = "PowerCoupling_DesiredDistance";

        public double defaultDesiredDistance = double.NaN;

        public double desiredDistance
        {
            set
            {
                string id = GeneratatePropertyId(DESIRED_DISTANCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(DESIRED_DISTANCE_PROPERTY_NAME);
                double result = defaultDesiredDistance;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultDesiredDistance;
            }

        }

        #endregion

        #region power

        public static PowerCoupler_PowerSlider powerSlider;
        static PowerCoupler_DecrasePowerAction decrasePowerAction;
        static PowerCoupler_IncrasePowerAction incrasePowerAction;

        public static string POWER_PROPERTY_NAME = "PowerCoupler_Power";

        public double defaultPower = 1;

        public double power
        {
            set
            {
                string id = GeneratatePropertyId(POWER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(POWER_PROPERTY_NAME);
                double result = defaultPower;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultPower;
            }

        }

        #endregion

        #region max force

        public static PowerCoupler_MaxForceSlider maxForceSlider;
        static PowerCoupler_DecraseMaxForceAction decraseMaxForceAction;
        static PowerCoupler_IncraseMaxForceAction incraseMaxForceAction;

        public static string MAX_FORCE_PROPERTY_NAME = "PowerCoupler_MaxForce";

        public double defaultMaxForce = 1;

        public double maxForce
        {
            set
            {
                string id = GeneratatePropertyId(MAX_FORCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MAX_FORCE_PROPERTY_NAME);
                double result = defaultMaxForce;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultMaxForce;
            }

        }

        #endregion

        #region frequency

        public static PowerCoupler_FrequencyTextBox frequencyTextBox;

        public static string FREQUENCY_PROPERTY_NAME = "PowerCoupling_Frequency";

        public string defaultFrequency = "";

        public string frequency
        {
            set
            {
                string id = GeneratatePropertyId(FREQUENCY_PROPERTY_NAME);
                SetVariable<string>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(FREQUENCY_PROPERTY_NAME);
                string result = defaultFrequency;
                if (GetVariable<string>(id, out result)) {
                    return result;
                }
                return defaultFrequency;
            }

        }

        #endregion        


        #region lifecycle

        public override void Initialize() {

            // distance

            if (desiredDistanceSlider == null) {
                desiredDistanceSlider = new PowerCoupler_DesiredDistanceSlider();
                desiredDistanceSlider.Initialize();
            }

            if (incraseDesiredDistanceAction == null) {
                incraseDesiredDistanceAction = new PowerCoupler_IncraseDesiredDistanceAction();
                incraseDesiredDistanceAction.Initialize();
            }

            if (decraseDesiredDistanceAction == null) {
                decraseDesiredDistanceAction = new PowerCoupler_DecraseDesiredDistanceAction();
                decraseDesiredDistanceAction.Initialize();
            }


            // power

            if (powerSlider == null) {
                powerSlider = new PowerCoupler_PowerSlider();
                powerSlider.Initialize();
            }

            if (incrasePowerAction == null) {
                incrasePowerAction = new PowerCoupler_IncrasePowerAction();
                incrasePowerAction.Initialize();
            }

            if (decrasePowerAction == null) {
                decrasePowerAction = new PowerCoupler_DecrasePowerAction();
                decrasePowerAction.Initialize();
            }

            // max force

            if (maxForceSlider == null) {
                maxForceSlider = new PowerCoupler_MaxForceSlider();
                maxForceSlider.Initialize();
            }

            if (decraseMaxForceAction == null) {
                decraseMaxForceAction = new PowerCoupler_DecraseMaxForceAction();
                decraseMaxForceAction.Initialize();
            }

            if (incraseMaxForceAction == null) {
                incraseMaxForceAction = new PowerCoupler_IncraseMaxForceAction();
                incraseMaxForceAction.Initialize();
            }

            // channel

            if (frequencyTextBox == null) {
                frequencyTextBox = new PowerCoupler_FrequencyTextBox();
                frequencyTextBox.Initialize();
            }

            powerCouplingTransceivers.Add(this);
        }

        public override void Destroy() {

            if (powerCouplingTransceivers.Contains(this)) {
                powerCouplingTransceivers.Remove(this);
            }
            Clear();
        }

        #endregion

        void Clear() {
            output = Vector3D.Zero;
        }

        Vector3D GetOutputPosition(PowerCoupling powerCoupling) {
            Vector3D outputPosition = powerCoupling.block.WorldMatrix.Translation;
            outputPosition += powerCoupling.block.WorldMatrix.Forward * (powerCoupling.block.CubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Large ? 1.25 : 0.25);
            return outputPosition;
        }

        Vector3D[] targetOutputPositions;
        Vector3D sourceOutputPosition;
        Vector3D targetPosition;
        double previousDistance;

        public Vector3 GetOutput(double physicsDeltaTime, out Vector3D direction) {

            IMyCubeGrid grid = block.CubeGrid;

            output = Vector3D.Zero;
            direction = Vector3D.Zero;

            if (string.IsNullOrEmpty(frequency)) {
                desiredDistance = double.NaN;
                return output;
            }

            // distancement

            targetOutputPositions = GetPowerCouplingOutputPositions(frequency);

            if (targetOutputPositions.Length < 1) {
                desiredDistance = double.NaN;
                return output;
            }

            sourceOutputPosition = GetOutputPosition(this);

            targetPosition = GetCentroid(sourceOutputPosition, targetOutputPositions);

            // distance
            double currentDistance = Vector3.Distance(sourceOutputPosition, targetPosition);

            //MyAPIGateway.Utilities.ShowMessage("PowerCoupling", "currentDistance:" + currentDistance);

            direction = targetPosition - sourceOutputPosition;

            if (double.IsNaN(desiredDistance)) {
                desiredDistance = currentDistance;
                previousDistance = currentDistance;
                desiredDistance = MathHelper.Clamp(desiredDistance, desiredDistanceSlider.min, desiredDistanceSlider.max);
            }

            direction.Normalize();


            // pid
            double stiffness = 8;
            //double impulse = 1;
            double damping = 100;

            double distanceError = desiredDistance - currentDistance;

            output = direction * stiffness * (distanceError + damping) * (previousDistance - currentDistance);
            previousDistance = currentDistance;

            output = Vector3D.ClampToSphere(output * maxForce, maxForce);

            int iterations = (int)Math.Ceiling(desiredDistance * 5);
            double range = 0.075f;
            float radius = 0.5f;

            if (!component.debugEnabled) {
                DebugDraw.PowerLine(sourceOutputPosition, targetPosition, iterations, range, Color.LightSteelBlue, radius);
                DebugDraw.PowerLine(sourceOutputPosition, targetPosition, iterations, range, Color.White, radius);
                DebugDraw.PowerLine(sourceOutputPosition, targetPosition, iterations, range, Color.LightBlue, radius);
                DebugDraw.PowerLine(sourceOutputPosition, targetPosition, iterations, range, Color.Violet, radius);
            }

            return output;
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Power Coupling ==");
            customInfo.AppendLine("Others On Channel : " + GetPowerCouplingsOnChannel(frequency).Length);
            customInfo.AppendLine("All : " + powerCouplingTransceivers.Count);
            customInfo.AppendLine("ID : " + block.EntityId);
            customInfo.AppendLine("Distance : " + desiredDistance);
            customInfo.AppendLine("Power : " + power);
            customInfo.AppendLine("Max Power : " + maxForce);
            customInfo.AppendLine("Output : " + output);
            customInfo.AppendLine("Channel : " + frequency);
        }

        Vector3D GetCentroid(Vector3D center, Vector3D[] points) {
            foreach (Vector3D point in points) {
                center += point;
            }
            if (points.Length > 0) {
                center /= (points.Length + 1);
            }
            return center;
        }

        Vector3D GetMiddlePoint(PowerCoupling[] powerCouplings) {

            Vector3D centroid = Vector3.Zero;

            foreach (PowerCoupling powerCoupling in powerCouplings) {
                //DebugDraw.Line(block.GetPosition(), powerCoupling.block.GetPosition(), Color.Violet, 0.1f);                
                centroid += powerCoupling.block.WorldMatrix.Translation;
            }

            if (powerCouplings.Length > 1) {
                centroid /= powerCouplings.Length;
            }

            return centroid;

        }

        Vector3D GetDisplacement(PowerCoupling[] powerCouplings) {

            Vector3D displacement = Vector3D.Zero;
            foreach (PowerCoupling powerCoupling in powerCouplings) {
                if (powerCoupling == null) {
                    continue;
                }
                //DebugDraw.Line(block.GetPosition(), powerCoupling.block.GetPosition(), Color.Violet, 0.1f);
                displacement -= powerCoupling.block.GetPosition() - block.GetPosition();
            }
            return displacement;
        }


        Vector3D[] GetPowerCouplingOutputPositions(string channel) {

            List<Vector3D> outputPositions = new List<Vector3D>();

            if (string.IsNullOrEmpty(channel)) {
                return outputPositions.ToArray();
            }

            foreach (PowerCoupling powerCouplingTransceiver in powerCouplingTransceivers) {
                if (powerCouplingTransceiver == this) {
                    continue;
                }
                if (powerCouplingTransceiver.frequency.Equals(channel)) {
                    outputPositions.Add(GetOutputPosition(powerCouplingTransceiver));
                }
            }
            return outputPositions.ToArray();

        }

        PowerCoupling[] GetPowerCouplingsOnChannel(string channel) {

            List<PowerCoupling> onChannel = new List<PowerCoupling>();

            if (string.IsNullOrEmpty(channel)) {
                return onChannel.ToArray();
            }

            foreach (PowerCoupling powerCouplingTransceiver in powerCouplingTransceivers) {
                if (powerCouplingTransceiver == this) {
                    continue;
                }
                if (powerCouplingTransceiver.frequency.Equals(channel)) {
                    onChannel.Add(powerCouplingTransceiver);
                }
            }
            return onChannel.ToArray();
        }

        public override void Debug() {

            if (!component.debugEnabled) {
                return;
            }

            DebugDraw.DrawLine(sourceOutputPosition, sourceOutputPosition + output, Color.Black, 0.025f);
            DebugDraw.GizmoDrawCross(sourceOutputPosition, 3, 0.025f);
            DebugDraw.DrawLine(sourceOutputPosition, targetPosition, Color.Pink, 0.05f);
            DebugDraw.GizmoDrawCross(targetPosition, 3, 0.025f);

        }
    }

}