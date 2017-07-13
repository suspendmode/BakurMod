using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    public class Copter : BakurBlockEquipment {

        static Separator<Copter> copterSeparator;
        static Separator<Copter> copterCruiseSeparator;
        static Label<Copter> copterLabel;

        #region responsicity

        public static Copter_ResponsivitySlider responsivitySlider;

        public static string RESPONSIVITY_PROPERTY_NAME = "Copter_Responsivity";

        public double defaultResponsivity = 3;

        public double responsivity
        {
            set
            {
                string id = GeneratatePropertyId(RESPONSIVITY_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(RESPONSIVITY_PROPERTY_NAME);
                double result = defaultResponsivity;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultResponsivity;
            }
        }

        #endregion

        #region max pitch angle

        public static Copter_MaxPitchAngleSlider maxPitchAngleSlider;

        public static string MAX_PITCH_PROPERTY_NAME = "Copter_MaxPitch";

        public double defaultMaxPitch = 45;

        public double maxPitch
        {
            set
            {
                string id = GeneratatePropertyId(MAX_PITCH_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MAX_PITCH_PROPERTY_NAME);
                double result = defaultMaxPitch;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultMaxPitch;
            }
        }

        #endregion

        #region max roll angle

        public static Copter_MaxRollAngleSlider maxRollAngleSlider;

        public static string MAX_ROLL_PROPERTY_NAME = "Copter_MaxRoll";

        public double defaultMaxRoll = 45;

        public double maxRoll
        {
            set
            {
                string id = GeneratatePropertyId(MAX_ROLL_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MAX_ROLL_PROPERTY_NAME);
                double result = defaultMaxRoll;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultMaxRoll;
            }
        }

        #endregion

        #region cruise speed

        public static Copter_CruiseSpeedSlider cruiseSpeedSlider;

        public static string CRUISE_SPEED_PROPERTY_NAME = "Copter_CruiseSpeed";

        public double defaultCruiseSpeed = 5;

        public double cruiseSpeed
        {
            set
            {
                string id = GeneratatePropertyId(CRUISE_SPEED_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(CRUISE_SPEED_PROPERTY_NAME);
                double result = defaultCruiseSpeed;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultCruiseSpeed;
            }
        }

        #endregion

        #region mode

        public static Copter_ModeComboBox modeComboBox;

        public static string MODE_PROPERTY_NAME = "Copter_Mode";

        public string defaultMode = "Hover";

        public string mode
        {
            set
            {
                string id = GeneratatePropertyId(MODE_PROPERTY_NAME);
                SetVariable<string>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MODE_PROPERTY_NAME);
                string result = defaultMode;
                if (GetVariable<string>(id, out result)) {
                    return result;
                }
                return defaultMode;
            }
        }

        #endregion

        #region use copter

        static Copter_UseCopterSwitch useCopterSwitch;
        static Copter_UseCopterToggleAction useCopterToggleAction;
        static Copter_UseCopterEnableAction useCopterEnableAction;
        static Copter_UseCopterDisableAction useCopterDisableAction;

        public static string USE_COPTER_PROPERTY_NAME = "Copter_UseCopter";

        public bool defaultUseCopter = true;

        public bool useCopter
        {
            set
            {
                string id = GeneratatePropertyId(USE_COPTER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_COPTER_PROPERTY_NAME);
                bool result = defaultUseCopter;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseCopter;
            }
        }


        #endregion

        protected double desiredPitch = 0, desiredRoll = 0;
        protected double pitch = 0, roll = 0, yaw = 0;

        public Vector3D desiredUp;

        public Copter(BakurBlock block) : base(block) {
        }

        #region lifecycle

        public override void Initialize() {

            // title

            if (copterSeparator == null) {
                copterSeparator = new Separator<Copter>("Copter_CopterSeparator");
                copterSeparator.Initialize();
            }

            if (copterLabel == null) {
                copterLabel = new Label<Copter>("Copter_CopterLabel", "Copter");
                copterLabel.Initialize();
            }

            // use copter

            if (useCopterSwitch == null) {
                useCopterSwitch = new Copter_UseCopterSwitch();
                useCopterSwitch.Initialize();
            }

            if (useCopterToggleAction == null) {
                useCopterToggleAction = new Copter_UseCopterToggleAction();
                useCopterToggleAction.Initialize();
            }

            if (useCopterEnableAction == null) {
                useCopterEnableAction = new Copter_UseCopterEnableAction();
                useCopterEnableAction.Initialize();
            }

            if (useCopterDisableAction == null) {
                useCopterDisableAction = new Copter_UseCopterDisableAction();
                useCopterDisableAction.Initialize();
            }

            // responsivity

            if (responsivitySlider == null) {
                responsivitySlider = new Copter_ResponsivitySlider();
                responsivitySlider.Initialize();
            }

            if (maxPitchAngleSlider == null) {
                maxPitchAngleSlider = new Copter_MaxPitchAngleSlider();
                maxPitchAngleSlider.Initialize();
            }
            if (maxRollAngleSlider == null) {
                maxRollAngleSlider = new Copter_MaxRollAngleSlider();
                maxRollAngleSlider.Initialize();
            }

            if (modeComboBox == null) {
                modeComboBox = new Copter_ModeComboBox();
                modeComboBox.Initialize();
            }

            if (copterCruiseSeparator == null) {
                copterCruiseSeparator = new Separator<Copter>("Copter_CopterSeparator");
                copterCruiseSeparator.Initialize();
            }

            if (cruiseSpeedSlider == null) {
                cruiseSpeedSlider = new Copter_CruiseSpeedSlider();
                cruiseSpeedSlider.Initialize();
            }

            // mode

            if (setGlideModeAction == null) {
                setGlideModeAction = new Copter_SetModeGlideAction();
                setGlideModeAction.Initialize();
            }

            if (setFreeGlideModeAction == null) {
                setFreeGlideModeAction = new Copter_SetModeFreeGlideAction();
                setFreeGlideModeAction.Initialize();
            }

            if (setPitchModeAction == null) {
                setPitchModeAction = new Copter_SetModePitchAction();
                setPitchModeAction.Initialize();
            }

            if (setRollModeAction == null) {
                setRollModeAction = new Copter_SetModeRollAction();
                setRollModeAction.Initialize();
            }

            if (setCruiseModeAction == null) {
                setCruiseModeAction = new Copter_SetModeCruiseAction();
                setCruiseModeAction.Initialize();
            }

            if (setHoverModeAction == null) {
                setHoverModeAction = new Copter_SetModeHoverAction();
                setHoverModeAction.Initialize();
            }
        }

        public override void Destroy() {

        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Copter ==");
            customInfo.AppendLine("Pitch : " + pitch);
            customInfo.AppendLine("Yaw : " + yaw);
            customInfo.AppendLine("Roll : " + roll);
            customInfo.AppendLine("DesiredPitch : " + desiredPitch);
            customInfo.AppendLine("DesiredRoll : " + desiredRoll);
            customInfo.AppendLine("setSpeed : " + cruiseSpeed);
            customInfo.AppendLine("Mode : " + mode);
        }

        #endregion

        public const string GLIDE_MODE = "glide";
        public const string FREE_GLIDE_MODE = "freeglide";
        public const string HOVER_MODE = "hover";
        public const string PITCH_MODE = "pitch";
        public const string ROLL_MODE = "roll";
        public const string CRUISE_MODE = "cruise";

        private Copter_SetModeHoverAction setHoverModeAction;
        private Copter_SetModeCruiseAction setCruiseModeAction;
        private Copter_SetModeRollAction setRollModeAction;
        private Copter_SetModePitchAction setPitchModeAction;
        private Copter_SetModeFreeGlideAction setFreeGlideModeAction;
        private Copter_SetModeGlideAction setGlideModeAction;

        public Vector3 GetDesiredUp(Vector3D defaultUp) {

            IMyCubeGrid grid = block.CubeGrid;

            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D worldLinearVelocity = grid.Physics.LinearVelocity;
            Vector3D localLinearVelocity = Vector3D.Transform(worldLinearVelocity, invWorldRot);

            double forwardSpeed = localLinearVelocity.Z;
            double sidewaysSpeed = localLinearVelocity.X;
            //localSpeed = worldSpeed;

            pitch = Vector3.Dot(component.gravityUp, block.WorldMatrix.Forward) / (component.gravity.Length() * block.WorldMatrix.Forward.Length()) * 90;
            roll = Vector3.Dot(component.gravityUp, block.WorldMatrix.Right) / (component.gravity.Length() * block.WorldMatrix.Right.Length()) * 90;

            if (double.IsNaN(pitch)) {
                pitch = 0;
            }

            if (double.IsNaN(roll)) {
                roll = 0;
            }

            switch (mode.ToLower()) {
                case GLIDE_MODE:
                    desiredPitch = 0;
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;

                case FREE_GLIDE_MODE:
                    desiredPitch = 0;
                    desiredRoll = 0;
                    break;

                case PITCH_MODE:
                    desiredPitch = Curve(forwardSpeed / responsivity, maxPitch);
                    desiredRoll = roll;
                    break;

                case ROLL_MODE:
                    desiredPitch = pitch;
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;

                case CRUISE_MODE:
                    desiredPitch = Curve((forwardSpeed + cruiseSpeed) / responsivity, maxPitch);
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;

                case HOVER_MODE:
                default:
                    desiredPitch = Curve(forwardSpeed / responsivity, maxPitch);
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;
            }

            double pitchRate = (desiredPitch - pitch) / 90;
            double rollRate = (desiredRoll - roll) / 90;

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null && shipController.MoveIndicator.X != 0) {
                pitchRate = 0;
            }

            if (shipController != null && shipController.MoveIndicator.Z != 0) {
                rollRate = 0;
            }

            //double pitchRate = (desiredPitch) / 90;
            //double rollRate = (desiredPitch) / 90;
            // Quaternion quatPitch = BakurMathHelper.AxisAngle(Vector3.Right, (float)pitchRate);
            // MyAPIGateway.Utilities.ShowMessage("Copter", "P: " + Math.Round(desiredPitch, 1) + "," + Math.Round(pitchRate, 1));
            //MyAPIGateway.Utilities.ShowMessage("Copter", "R: " + Math.Round(desiredRoll, 1) + "," + Math.Round(rollRate, 1));

            Matrix blockOrientation = block.WorldMatrix.GetOrientation();
            //Vector3 rotationVec = new Vector3(pitchRate, 0, rollRate) * (float)BakurMathHelper.Deg2Rad;
            Vector3 rotationVec = new Vector3(-pitchRate, 0, -rollRate);
            //rotationVec = Vector3.Transform(rotationVec, blockOrientation);

            Quaternion pitchRotation = Quaternion.CreateFromAxisAngle(block.WorldMatrix.Right, rotationVec.X);
            Quaternion rollRotation = Quaternion.CreateFromAxisAngle(block.WorldMatrix.Forward, rotationVec.Z);

            //Vector3 rotationVec = new Vector3(-pitchRate, 0, -rollRate);

            //double rollRate = (desiredRoll);// * BakurMathHelper.Deg2Rad;
            //Quaternion quatRoll = BakurMathHelper.AxisAngle(Vector3.Forward, (float)rollRate);
            //Quaternion.CreateFromAxisAngle(V)
            //output = Vector3D.Transform(grid.WorldMatrix.Up, quatPitch * quatRoll);
            //MyAPIGateway.Utilities.ShowMessage("", "p:" + pitchRate + ", r: " + desiredRoll);

            //output = Vector3D.Transform(up, quatPitch * quatRoll);
            Quaternion rotation = pitchRotation + rollRotation;
            desiredUp = Vector3.Transform(defaultUp, rotation);
            return desiredUp;
        }

        double Curve(double value, double max) {
            return Math.Atan(value / MathHelper.PiOver2) * max;
        }
    }

}