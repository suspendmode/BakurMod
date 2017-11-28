using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Input;
using VRageMath;

namespace BakurRepulsorCorp {

    public class RepulsorAngularGenerator : BakurBlockEquipment {

        public double maxAngularVelocity = 90;
        public double maxAngularAcceleration = 180;

        public RepulsorAngularGenerator(BakurBlock component) : base(component) {
            
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

        #region power

        static AngularGenerator_PowerSlider powerSlider;
        static AngularGenerator_IncrasePowerAction incrasePowerAction;
        static AngularGenerator_DecrasePowerAction decrasePowerAction;

        public static string POWER_PROPERTY_NAME = "AngularGenerator_Power";

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

           
            // power

            if (powerSlider == null) {
                powerSlider = new AngularGenerator_PowerSlider();
                powerSlider.Initialize();
            }
            if (incrasePowerAction == null) {
                incrasePowerAction = new AngularGenerator_IncrasePowerAction();
                incrasePowerAction.Initialize();
            }
            if (decrasePowerAction == null) {
                decrasePowerAction = new AngularGenerator_DecrasePowerAction();
                decrasePowerAction.Initialize();
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

        public Vector3D GetAngularAcceleration(double physicsDeltaTime) {

            desiredAngularAcceleration = Vector3D.Zero;

            if (!useAngularGenerator) {
                Clear();
                return desiredAngularAcceleration;
            }

            IMyCubeGrid grid = block.CubeGrid;
            // if ((grid.Physics.AngularVelocity.Length() * BakurMathHelper.Rad2Deg) > maxAngularVelocity) {
            //      Clear();
            //     return desiredAngularAcceleration;
            //  }

            MatrixD blockWorldOrientation = block.PositionComp.WorldMatrix.GetOrientation();
            Vector3D angularVelocity = angularOverride;
           // MyAPIGateway.Utilities.ShowMessage("Generator", "override:" + angularVelocity + ", " + angularVelocity.Length());

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);
            if (shipController != null) {
                angularVelocity += GetShipControllerAngularInput(shipController);
            }

            angularVelocity *= maxAngularVelocity;

            //MyAPIGateway.Utilities.ShowMessage("Generator", "input:" + angularVelocity + ", " + angularVelocity.Length());

            //MyAPIGateway.Utilities.ShowMessage("Generator", "maxed:" + angularVelocity + ", " + angularVelocity.Length());

            desiredAngularAcceleration = Vector3D.Transform(angularVelocity, blockWorldOrientation) / physicsDeltaTime;
            desiredAngularAcceleration = Vector3D.ClampToSphere(desiredAngularAcceleration, maxAngularAcceleration);
            return desiredAngularAcceleration * power;
        }

        #endregion     

        #region input

        Vector3D angularOverride {
            get
            {
                return new Vector3D(pitch, yaw, roll);
            }
        }

        Vector3D GetShipControllerAngularInput(IMyShipController shipController) {

            IMyCubeGrid grid = block.CubeGrid;
            Vector3D angularInput = Vector3D.Zero;

            if (!shipController.CanControlShip) {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_UP)) {
                    angularInput.X = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_DOWN)) {
                    angularInput.X = 1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_LEFT)) {
                    angularInput.Y = 1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_RIGHT)) {
                    angularInput.Y = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_LEFT)) {
                    angularInput.Z = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT)) {
                    angularInput.Z = 1;
                }

            } else {
                //MyAPIGateway.Input.GetRoll();

                //MyAPIGateway.Input.GetGameControl
                float sensitivity = MyAPIGateway.Input.GetMouseSensitivity() * 0.13f;

                angularInput.X = shipController.RotationIndicator.X < 0 ? 1 : shipController.RotationIndicator.X > 0 ? -1 : 0;
                angularInput.Y = shipController.RotationIndicator.Y < 0 ? 1 : shipController.RotationIndicator.Y > 0 ? -1 : 0;
                angularInput.Z = shipController.RollIndicator < 0 ? 1 : shipController.RollIndicator > 0 ? -1 : 0;
            }

            return angularInput;
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