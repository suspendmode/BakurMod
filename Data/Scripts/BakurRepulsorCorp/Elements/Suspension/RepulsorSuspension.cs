using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class RepulsorSuspension : LogicElement {

        public RepulsorSuspension(LogicComponent component) : base(component) {
        }

        static Separator<RepulsorSuspension> repulsorSuspensionSeparator;
        static Label<RepulsorSuspension> repulsorSuspensionLabel;

        #region damping

        static Suspension_DampingSlider dampingSlider;
        static Suspension_IncraseDampingAction incraseDampingAction;
        static Suspension_DecraseDampingAction decraseDampingAction;

        public static string DAMPING_PROPERTY_NAME = "RepulsorSuspension_Damping";

        public double defaultDamping = Suspension_DampingSlider.maximum;

        public double damping
        {
            set
            {
                string id = GeneratatePropertyId(DAMPING_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(DAMPING_PROPERTY_NAME);
                double result = defaultDamping;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultDamping;
            }

        }

        #endregion

        #region rest length

        static Suspension_RestLengthSlider restLengthSlider;
        static Suspension_IncraseRestLengthAction incraseRestLengthAction;
        static Suspension_DecraseRestLengthAction decraseRestLengthAction;

        public static string REST_LENGTH_PROPERTY_NAME = "RepulsorSuspension_RestLength";

        public double defaultRestLength = double.NaN;

        public double restLength
        {
            set
            {
                string id = GeneratatePropertyId(REST_LENGTH_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(REST_LENGTH_PROPERTY_NAME);
                double result = defaultRestLength;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultRestLength;
            }
        }

        #endregion

        #region stiffness

        static Suspension_StiffnessSlider stiffnessSlider;
        static Suspension_IncraseStiffnessAction incraseStiffnessAction;
        static Suspension_DecraseStiffnessAction decraseStiffnessAction;

        public static string STIFFNES_PROPERTY_NAME = "RepulsorSuspension_Stiffness";

        public double defaultStiffness = 8;

        public double stiffness
        {
            set
            {
                string id = GeneratatePropertyId(STIFFNES_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(STIFFNES_PROPERTY_NAME);
                double result = defaultStiffness;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultStiffness;
            }
        }

        #endregion

        #region impulse

        static Suspension_ImpulseSlider impulseSlider;

        public static string IMPULSE_PROPERTY_NAME = "RepulsorSuspension_Impulse";

        public double defaultImpulse = Suspension_ImpulseSlider.maxImpulse;

        public double impulse
        {
            set
            {
                string id = GeneratatePropertyId(IMPULSE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(IMPULSE_PROPERTY_NAME);
                double result = defaultImpulse;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultImpulse;
            }
        }

        #endregion

        #region lifecycle

        public override void Initialize() {

            if (repulsorSuspensionSeparator == null) {
                repulsorSuspensionSeparator = new Separator<RepulsorSuspension>("RepulsorSuspension_RepulsorSuspensionSeparator");
                repulsorSuspensionSeparator.Initialize();
            }

            if (repulsorSuspensionLabel == null) {
                repulsorSuspensionLabel = new Label<RepulsorSuspension>("RepulsorSuspension_RepulsorSuspensionLabel", "Repulsor Suspension");
                repulsorSuspensionLabel.Initialize();
            }

            if (dampingSlider == null) {
                dampingSlider = new Suspension_DampingSlider();
                dampingSlider.Initialize();
            }

            if (impulseSlider == null) {
                impulseSlider = new Suspension_ImpulseSlider();
                impulseSlider.Initialize();
            }

            if (incraseDampingAction == null) {
                incraseDampingAction = new Suspension_IncraseDampingAction();
                incraseDampingAction.Initialize();
            }

            if (decraseDampingAction == null) {
                decraseDampingAction = new Suspension_DecraseDampingAction();
                decraseDampingAction.Initialize();
            }

            if (restLengthSlider == null) {
                restLengthSlider = new Suspension_RestLengthSlider();
                restLengthSlider.Initialize();
            }

            if (incraseRestLengthAction == null) {
                incraseRestLengthAction = new Suspension_IncraseRestLengthAction();
                incraseRestLengthAction.Initialize();
            }

            if (decraseRestLengthAction == null) {
                decraseRestLengthAction = new Suspension_DecraseRestLengthAction();
                decraseRestLengthAction.Initialize();
            }

            if (stiffnessSlider == null) {
                stiffnessSlider = new Suspension_StiffnessSlider();
                stiffnessSlider.Initialize();
            }

            if (incraseStiffnessAction == null) {
                incraseStiffnessAction = new Suspension_IncraseStiffnessAction();
                incraseStiffnessAction.Initialize();
            }

            if (decraseStiffnessAction == null) {
                decraseStiffnessAction = new Suspension_DecraseStiffnessAction();
                decraseStiffnessAction.Initialize();
            }
        }

        public override void Destroy() {
            Clear();
        }


        void Clear() {
            desiredForce = Vector3D.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Suspension ==");
            customInfo.AppendLine("Stiffness : " + Math.Round(stiffness, 1));
            customInfo.AppendLine("Rest Length : " + Math.Round(restLength, 1) + " m");
            customInfo.AppendLine("Damping : " + Math.Round(damping, 1));
            customInfo.AppendLine("Ratio : " + Math.Round(ratio, 3));
            customInfo.AppendLine("Impulse : " + Math.Round(impulse, 1));
            customInfo.AppendLine("Altitude : " + Math.Round(altitude, 1) + " m");
            customInfo.AppendLine("Previous Altitude : " + Math.Round(previousAltitude, 1) + " m");
            customInfo.AppendLine("Desired Force : " + Math.Round(desiredForce.Length() / 1000, 3) + " kN");
        }

        #endregion

        public Vector3D desiredForce;
        public double altitude;
        double previousAltitude;
        public Vector3D desiredUp;
        double ratio;

        public Vector3D GetForce(double physicsDeltaTime, Vector3D desiredUp, double altitude) {

            this.altitude = altitude;
            this.desiredUp = desiredUp;

            desiredForce = Vector3.Zero;

            // auto distance if (double.IsNaN(restLength) && !double.IsNaN(this.distance)) {

            if (altitude >= Suspension_RestLengthSlider.maximum) {
                return desiredForce;
            }

            IMyCubeGrid grid = block.CubeGrid;

            if (double.IsNaN(restLength)) {
                restLength = (grid.LocalAABB.Size.Length() * 0.5f) + 0.5f;
                restLength = MathHelper.Clamp(restLength * 0.5f, Suspension_RestLengthSlider.minimum, Suspension_RestLengthSlider.maximum);
            }

            // desired altitude
            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null) {
                restLength += GetShipControllerLinearInput(physicsDeltaTime, shipController);
            }

            restLength = MathHelper.Clamp(restLength, Suspension_RestLengthSlider.minimum, Suspension_RestLengthSlider.maximum);

            ratio = stiffness * (restLength - this.altitude) + damping * (previousAltitude - this.altitude);
            previousAltitude = this.altitude;

            desiredForce = desiredUp * MathHelper.Clamp(ratio * impulse * logicComponent.rigidbody.gridMass * logicComponent.rigidbody.gravity.Length(), 0, double.MaxValue);

            return desiredForce;
        }

        double GetShipControllerLinearInput(double physicsDeltaTime, IMyShipController shipController) {

            double desired = 0;

            if (!shipController.CanControlShip) {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP)) {
                    desired = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH)) {
                    desired = -1;
                }
            } else {
                desired = MathHelper.Clamp(shipController.MoveIndicator.Y, -1, 1);
            }
            desired *= (physicsDeltaTime * physicsDeltaTime * 0.3);
            return desired;

        }
        public override void Debug() {
            if (!logicComponent.debugEnabled) {
                return;
            }
        }
    }
}
