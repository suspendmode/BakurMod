using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class RepulsorAngularGenerator : BakurBlockEquipment {

        public RepulsorAngularGenerator(BakurBlock component, double maxAngularAcceleration) : base(component) {
        }

        static Label<RepulsorAngularGenerator> repulsorAngularGeneratorLabel;
        static Separator<RepulsorAngularGenerator> repulsorAngularGeneratorSeparator;

        static Separator<RepulsorAngularGenerator> angularOverrideSeparator;
        static Label<RepulsorAngularGenerator> angularOverrideLabel;

        #region use angular generator

        static AngularGenerator_UseAngularGeneratorSwitch useAngularGeneratorSwitch;
        static AngularGenerator_UseAngularGeneratorToggleAction useAngularGeneratorToggleAction;
        static AngularGenerator_UseAngularGeneratorEnableAction useAngularGeneratorEnableAction;
        static AngularGenerator_UseAngularGeneratorDisableAction useAngularGeneratorDisableAction;

        public static string USE_ANGULAR_GENERATOR_PROPERTY_NAME = "AngularGenerator_UseAngularGenerator";

        public bool defaultUseAngularGenerator = true;

        public bool useAngularGenerator
        {
            set
            {
                string id = GeneratatePropertyId(USE_ANGULAR_GENERATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_ANGULAR_GENERATOR_PROPERTY_NAME);
                bool result = defaultUseAngularGenerator;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseAngularGenerator;
            }
        }

        #endregion

        #region use pitch

        static AngularGenerator_UsePitchSwitch usePitchSwitch;
        static AngularGenerator_UsePitchToggleAction usePitchToggleAction;
        static AngularGenerator_UsePitchEnableAction usePitchEnableAction;
        static AngularGenerator_UsePitchDisableAction usePitchDisableAction;

        public static string USE_PITCH_PROPERTY_NAME = "AngularGenerator_UsePitch";

        public bool defaultUsePitch = true;

        public bool usePitch
        {
            set
            {
                string id = GeneratatePropertyId(USE_PITCH_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_PITCH_PROPERTY_NAME);
                bool result = defaultUsePitch;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUsePitch;
            }

        }

        #endregion

        #region use yaw

        static AngularGenerator_UseYawSwitch useYawSwitch;
        static AngularGenerator_UseYawToggleAction useYawToggleAction;
        static AngularGenerator_UseYawEnableAction useYawEnableAction;
        static AngularGenerator_UseYawDisableAction useYawDisableAction;

        public static string USE_YAW_PROPERTY_NAME = "AngularGenerator_UseYaw";

        public bool defaultUseYaw = true;

        public bool useYaw
        {
            set
            {
                string id = GeneratatePropertyId(USE_YAW_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_YAW_PROPERTY_NAME);
                bool result = defaultUseYaw;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseYaw;
            }

        }

        #endregion

        #region use roll

        static AngularGenerator_UseRollSwitch useRollSwitch;
        static AngularGenerator_UseRollToggleAction useRollToggleAction;
        static AngularGenerator_UseRollEnableAction useRollEnableAction;
        static AngularGenerator_UseRollDisableAction useRollDisableAction;

        public static string USE_ROLL_PROPERTY_NAME = "AngularGenerator_UseRoll";

        public bool defaultUseRoll = true;

        public bool useRoll
        {
            set
            {
                string id = GeneratatePropertyId(USE_ROLL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_ROLL_PROPERTY_NAME);
                bool result = defaultUseRoll;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseRoll;
            }

        }

        #endregion

        #region pitch

        static AngularGenerator_PitchSlider pitchSlider;
        static AngularGenerator_IncrasePitchAction incrasePitchAction;
        static AngularGenerator_DecrasePitchAction decrasePitchAction;
        static AngularGenerator_ZeroPitchAction zeroPitchAction;

        public static string PITCH_PROPERTY_NAME = "AngularGenerator_Pitch";

        public double defaultPitch = 0;

        public double pitch
        {
            set
            {
                string id = GeneratatePropertyId(PITCH_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(PITCH_PROPERTY_NAME);
                double result = defaultPitch;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultPitch;
            }
        }

        #endregion

        #region yaw

        static AngularGenerator_YawSlider yawSlider;
        static AngularGenerator_IncraseYawAction incraseYawAction;
        static AngularGenerator_DecraseYawAction decraseYawAction;
        static AngularGenerator_ZeroYawAction zeroYawAction;

        public static string YAW_PROPERTY_NAME = "AngularGenerator_Yaw";

        public double defaultYaw = 0;

        public double yaw
        {
            set
            {
                string id = GeneratatePropertyId(YAW_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(YAW_PROPERTY_NAME);
                double result = defaultYaw;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultYaw;
            }
        }

        #endregion

        #region roll

        static AngularGenerator_RollSlider rollSlider;
        static AngularGenerator_IncraseRollAction incraseRollAction;
        static AngularGenerator_DecraseRollAction decraseRollAction;
        static AngularGenerator_ZeroRollAction zeroRollAction;

        public static string ROLL_PROPERTY_NAME = "AngularGenerator_Roll";

        public double defaultRoll = 0;

        public double roll
        {
            set
            {
                string id = GeneratatePropertyId(ROLL_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(ROLL_PROPERTY_NAME);
                double result = defaultRoll;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultRoll;
            }
        }

        #endregion

        #region lifecycle

        public override void Initialize() {

            // label

            if (repulsorAngularGeneratorSeparator == null) {
                repulsorAngularGeneratorSeparator = new Separator<RepulsorAngularGenerator>("AngularGenerator_RepulsorAngularGeneratorSeparator");
                repulsorAngularGeneratorSeparator.Initialize();
            }

            if (repulsorAngularGeneratorLabel == null) {
                repulsorAngularGeneratorLabel = new Label<RepulsorAngularGenerator>("AngularGenerator_RepulsorAngularGeneratorabel", "Repulsor Angular Generator");
                repulsorAngularGeneratorLabel.Initialize();
            }

            // angular

            if (angularOverrideSeparator == null) {
                angularOverrideSeparator = new Separator<RepulsorAngularGenerator>("AngularGenerator_AngularOverrideSeparator");
                angularOverrideSeparator.Initialize();
            }

            if (angularOverrideLabel == null) {
                angularOverrideLabel = new Label<RepulsorAngularGenerator>("AngularGenerator_AngularOverrideLabel", "Angular Override");
                angularOverrideLabel.Initialize();
            }

            // use angular generator

            if (useAngularGeneratorSwitch == null) {
                useAngularGeneratorSwitch = new AngularGenerator_UseAngularGeneratorSwitch();
                useAngularGeneratorSwitch.Initialize();
            }

            if (useAngularGeneratorToggleAction == null) {
                useAngularGeneratorToggleAction = new AngularGenerator_UseAngularGeneratorToggleAction();
                useAngularGeneratorToggleAction.Initialize();
            }

            if (useAngularGeneratorEnableAction == null) {
                useAngularGeneratorEnableAction = new AngularGenerator_UseAngularGeneratorEnableAction();
                useAngularGeneratorEnableAction.Initialize();
            }

            if (useAngularGeneratorDisableAction == null) {
                useAngularGeneratorDisableAction = new AngularGenerator_UseAngularGeneratorDisableAction();
                useAngularGeneratorDisableAction.Initialize();
            }

            // use pitch

            if (usePitchSwitch == null) {
                usePitchSwitch = new AngularGenerator_UsePitchSwitch();
                usePitchSwitch.Initialize();
            }

            if (usePitchToggleAction == null) {
                usePitchToggleAction = new AngularGenerator_UsePitchToggleAction();
                usePitchToggleAction.Initialize();
            }

            if (usePitchEnableAction == null) {
                usePitchEnableAction = new AngularGenerator_UsePitchEnableAction();
                usePitchEnableAction.Initialize();
            }

            if (usePitchDisableAction == null) {
                usePitchDisableAction = new AngularGenerator_UsePitchDisableAction();
                usePitchDisableAction.Initialize();
            }



            // pitch

            if (pitchSlider == null) {
                pitchSlider = new AngularGenerator_PitchSlider();
                pitchSlider.Initialize();
            }

            if (incrasePitchAction == null) {
                incrasePitchAction = new AngularGenerator_IncrasePitchAction();
                incrasePitchAction.Initialize();
            }
            if (decrasePitchAction == null) {
                decrasePitchAction = new AngularGenerator_DecrasePitchAction();
                decrasePitchAction.Initialize();
            }
            if (zeroPitchAction == null) {
                zeroPitchAction = new AngularGenerator_ZeroPitchAction();
                zeroPitchAction.Initialize();
            }

            // use yaw

            if (useYawSwitch == null) {
                useYawSwitch = new AngularGenerator_UseYawSwitch();
                useYawSwitch.Initialize();
            }

            if (useYawToggleAction == null) {
                useYawToggleAction = new AngularGenerator_UseYawToggleAction();
                useYawToggleAction.Initialize();
            }

            if (useYawEnableAction == null) {
                useYawEnableAction = new AngularGenerator_UseYawEnableAction();
                useYawEnableAction.Initialize();
            }

            if (useYawDisableAction == null) {
                useYawDisableAction = new AngularGenerator_UseYawDisableAction();
                useYawDisableAction.Initialize();
            }



            // yaw

            if (yawSlider == null) {
                yawSlider = new AngularGenerator_YawSlider();
                yawSlider.Initialize();
            }

            if (incraseYawAction == null) {
                incraseYawAction = new AngularGenerator_IncraseYawAction();
                incraseYawAction.Initialize();
            }

            if (decraseYawAction == null) {
                decraseYawAction = new AngularGenerator_DecraseYawAction();
                decraseYawAction.Initialize();
            }

            if (zeroYawAction == null) {
                zeroYawAction = new AngularGenerator_ZeroYawAction();
                zeroYawAction.Initialize();
            }

            // use roll

            if (useRollSwitch == null) {
                useRollSwitch = new AngularGenerator_UseRollSwitch();
                useRollSwitch.Initialize();
            }

            if (useRollToggleAction == null) {
                useRollToggleAction = new AngularGenerator_UseRollToggleAction();
                useRollToggleAction.Initialize();
            }

            if (useRollEnableAction == null) {
                useRollEnableAction = new AngularGenerator_UseRollEnableAction();
                useRollEnableAction.Initialize();
            }

            if (useRollDisableAction == null) {
                useRollDisableAction = new AngularGenerator_UseRollDisableAction();
                useRollDisableAction.Initialize();
            }


            // roll

            if (rollSlider == null) {
                rollSlider = new AngularGenerator_RollSlider();
                rollSlider.Initialize();
            }

            if (incraseRollAction == null) {
                incraseRollAction = new AngularGenerator_IncraseRollAction();
                incraseRollAction.Initialize();
            }

            if (decraseRollAction == null) {
                decraseRollAction = new AngularGenerator_DecraseRollAction();
                decraseRollAction.Initialize();
            }

            if (zeroRollAction == null) {
                zeroRollAction = new AngularGenerator_ZeroRollAction();
                zeroRollAction.Initialize();
            }
        }

        public override void Destroy() {
            Clear();
        }

        void Clear() {
            desiredAngularAcceleration = Vector3D.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Angular Generator ==");
            customInfo.AppendLine("Pitch : " + Math.Round(pitch, 1) + " °/s");
            customInfo.AppendLine("Yaw : " + Math.Round(yaw, 1) + " °/s");
            customInfo.AppendLine("Roll : " + Math.Round(roll, 1) + " °/s²");
        }


        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }

            Vector3D center = block.WorldAABB.Center;
            float length = block.CubeGrid.LocalAABB.Size.Length() * 10;

            DebugDraw.DrawLine(center, center + block.WorldMatrix.Right * desiredAngularAcceleration.X * length, Color.Red, 0.05f);
            DebugDraw.DrawLine(center, center + block.WorldMatrix.Up * desiredAngularAcceleration.Y * length, Color.Green, 0.05f);
            DebugDraw.DrawLine(center, center + block.WorldMatrix.Forward * desiredAngularAcceleration.Z * length, Color.Blue, 0.05f);
        }

        #endregion

        #region velocity

        public Vector3D desiredAngularAcceleration;

        public Vector3D GetAngularAcceleration(double physicsDeltaTime, double maxAngularAcceleration) {

            IMyCubeGrid grid = block.CubeGrid;
            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            desiredAngularAcceleration = Vector3D.Zero;

            if (!useAngularGenerator) {
                return desiredAngularAcceleration;
            }

            MatrixD blockWorldOrientation = block.PositionComp.WorldMatrix.GetOrientation();
            Vector3D angularVelocity = Vector3.Zero;
            if (shipController != null) {
                angularVelocity += GetShipControllerAngularInput(shipController);
            }
            angularVelocity *= maxAngularAcceleration;

            desiredAngularAcceleration = Vector3D.Transform(angularVelocity, Quaternion.CreateFromRotationMatrix(blockWorldOrientation));
            desiredAngularAcceleration = Vector3D.ClampToSphere(desiredAngularAcceleration, maxAngularAcceleration);
            return desiredAngularAcceleration;
        }

        #endregion     

        #region input

        Vector3D GetAngularOverride() {

            Vector3D desired = Vector3D.Zero;

            desired.X = usePitch ? pitch : 0;
            desired.Y = useYaw ? yaw : 0;
            desired.Z = useRoll ? roll : 0;

            return desired;
        }

        Vector3D GetShipControllerAngularInput(IMyShipController shipController) {

            IMyCubeGrid grid = block.CubeGrid;
            Vector3D desired = Vector3D.Zero;

            if (!shipController.CanControlShip) {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_UP)) {
                    desired.X = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_DOWN)) {
                    desired.X = 1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_LEFT)) {
                    desired.Y = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_RIGHT)) {
                    desired.Y = 1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_LEFT)) {
                    desired.Z = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT)) {
                    desired.Z = 1;
                }

            } else {
                desired.X = shipController.RotationIndicator.X;
                desired.Y = shipController.RotationIndicator.Y;
                desired.Z = -shipController.RollIndicator;
            }

            desired.X = usePitch ? desired.X : 0;
            desired.Y = useYaw ? desired.Y : 0;
            desired.Z = useRoll ? desired.Z : 0;


            return desired;
        }

        bool HasControlPitch() {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_UP) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_DOWN);
        }

        bool HasControlYaw() {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_LEFT) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_RIGHT);
        }

        bool HasControlRoll() {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_LEFT);

        }

        #endregion

    }
}