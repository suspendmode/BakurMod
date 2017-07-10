using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class RepulsorLinearGenerator : BakurBlockEquipment {

        public RepulsorLinearGenerator(BakurBlock component) : base(component) {
        }

        static Separator<RepulsorLinearGenerator> repulsorLinearGeneratorSeparator;
        static Label<RepulsorLinearGenerator> repulsorLinearGeneratorLabel;

        static Separator<RepulsorLinearGenerator> linearOverrideSeparator;
        static Label<RepulsorLinearGenerator> linearOverrideLabel;

        #region use linear generator

        static LinearGenerator_UseLinearGeneratorSwitch useLinearGeneratorSwitch;
        static LinearGenerator_UseLinearGeneratorToggleAction useLinearGeneratorToggleAction;
        static LinearGenerator_UseLinearGeneratorEnableAction useLinearGeneratorEnableAction;
        static LinearGenerator_UseLinearGeneratorDisableAction useLinearGeneratorDisableAction;

        public static string USE_LINEAR_DRIVE_PROPERTY_NAME = "LinearGenerator_UseLinearGenerator";

        public bool defaultUseLinearGenerator = true;

        public bool useLinearGenerator
        {
            set
            {
                string id = GeneratatePropertyId(USE_LINEAR_DRIVE_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_LINEAR_DRIVE_PROPERTY_NAME);
                bool result = defaultUseLinearGenerator;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseLinearGenerator;
            }
        }

        #endregion

        #region use forward

        static LinearGenerator_UseForwardSwitch useForwardSwitch;
        static LinearGenerator_UseForwardToggleAction useForwardToggleAction;
        static LinearGenerator_UseForwardEnableAction useForwardEnableAction;
        static LinearGenerator_UseForwardDisableAction useForwardDisableAction;

        public static string USE_FORWARD_PROPERTY_NAME = "LinearGenerator_UseForward";

        public bool defaultUseForward = true;

        public bool useForward
        {
            set
            {
                string id = GeneratatePropertyId(USE_FORWARD_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_FORWARD_PROPERTY_NAME);
                bool result = defaultUseForward;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseForward;
            }

        }

        #endregion

        #region use sideways

        static LinearGenerator_UseSidewaysSwitch useSidewaysSwitch;
        static LinearGenerator_UseSidewaysToggleAction useSidewaysToggleAction;
        static LinearGenerator_UseSidewaysEnableAction useSidewaysEnableAction;
        static LinearGenerator_UseSidewaysDisableAction useSidewaysDisableAction;

        public static string USE_SIDEWAYS_PROPERTY_NAME = "LinearGenerator_UseSideways";

        public bool defaultUseSideways = true;

        public bool useSideways
        {
            set
            {
                string id = GeneratatePropertyId(USE_SIDEWAYS_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_SIDEWAYS_PROPERTY_NAME);
                bool result = defaultUseSideways;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseSideways;
            }

        }

        #endregion

        #region use up

        static LinearGenerator_UseUpSwitch useUpSwitch;
        static LinearGenerator_UseUpToggleAction useUpToggleAction;
        static LinearGenerator_UseUpEnableAction useUpEnableAction;
        static LinearGenerator_UseUpDisableAction useUpDisableAction;

        public static string USE_UP_PROPERTY_NAME = "LinearGenerator_UseUp";

        public bool defaultUseUp = true;

        public bool useUp
        {
            set
            {
                string id = GeneratatePropertyId(USE_UP_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_UP_PROPERTY_NAME);
                bool result = defaultUseUp;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseUp;
            }

        }

        #endregion

        #region forward

        static LinearGenerator_ForwardSlider forwardSlider;
        static LinearGenerator_IncraseForwardAction incraseForwardAction;
        static LinearGenerator_DecraseForwardAction decraseForwardAction;
        static LinearGenerator_ZeroForwardAction zeroForwardAction;

        public static string FORWARD_PROPERTY_NAME = "LinearGenerator_Forward";

        public double defaultForward = 0;

        public double forward
        {
            set
            {
                string id = GeneratatePropertyId(FORWARD_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(FORWARD_PROPERTY_NAME);
                double result = defaultForward;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultForward;
            }

        }

        #endregion

        #region sideways

        static LinearGenerator_SidewaysSlider sidewaysSlider;
        static LinearGenerator_IncraseSidewaysAction incraseSidewaysAction;
        static LinearGenerator_DecraseSidewaysAction decraseSidewaysAction;
        static LinearGenerator_ZeroSidewaysAction zeroSidewaysAction;

        public static string SIDEWAYS_PROPERTY_NAME = "LinearGenerator_Sideways";

        public double defaultSideways = 0;

        public double sideways
        {
            set
            {
                string id = GeneratatePropertyId(SIDEWAYS_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(SIDEWAYS_PROPERTY_NAME);
                double result = defaultSideways;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultSideways;
            }
        }

        #endregion

        #region up

        static LinearGenerator_UpSlider upSlider;
        static LinearGenerator_IncraseUpAction incraseUpAction;
        static LinearGenerator_DecraseUpAction decraseUpAction;
        static LinearGenerator_ZeroUpAction zeroUpAction;

        public static string UP_PROPERTY_NAME = "LinearGenerator_UP";

        public double defaultUp = 0;

        public double up
        {
            set
            {
                string id = GeneratatePropertyId(UP_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(UP_PROPERTY_NAME);
                double result = defaultUp;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultUp;
            }
        }

        #endregion

        #region lifecycle

        public override void Initialize() {

            // label

            if (repulsorLinearGeneratorSeparator == null) {
                repulsorLinearGeneratorSeparator = new Separator<RepulsorLinearGenerator>("LinearGenerator_GeneratorSeparator");
                repulsorLinearGeneratorSeparator.Initialize();
            }

            if (repulsorLinearGeneratorLabel == null) {
                repulsorLinearGeneratorLabel = new Label<RepulsorLinearGenerator>("LinearGenerator_RepulsorLinearGeneratorLabel", "Repulsor Linear Generator");
                repulsorLinearGeneratorLabel.Initialize();
            }

            // linear

            if (linearOverrideSeparator == null) {
                linearOverrideSeparator = new Separator<RepulsorLinearGenerator>("LinearGenerator_LinearOverrideSeparator");
                linearOverrideSeparator.Initialize();
            }

            if (linearOverrideLabel == null) {
                linearOverrideLabel = new Label<RepulsorLinearGenerator>("LinearGenerator_LinearOverrideLabel", "Linear Override");
                linearOverrideLabel.Initialize();
            }

            // use linear generator

            if (useLinearGeneratorSwitch == null) {
                useLinearGeneratorSwitch = new LinearGenerator_UseLinearGeneratorSwitch();
                useLinearGeneratorSwitch.Initialize();
            }

            if (useLinearGeneratorToggleAction == null) {
                useLinearGeneratorToggleAction = new LinearGenerator_UseLinearGeneratorToggleAction();
                useLinearGeneratorToggleAction.Initialize();
            }

            if (useLinearGeneratorEnableAction == null) {
                useLinearGeneratorEnableAction = new LinearGenerator_UseLinearGeneratorEnableAction();
                useLinearGeneratorEnableAction.Initialize();
            }

            if (useLinearGeneratorDisableAction == null) {
                useLinearGeneratorDisableAction = new LinearGenerator_UseLinearGeneratorDisableAction();
                useLinearGeneratorDisableAction.Initialize();
            }

            // use sideways

            if (useSidewaysSwitch == null) {
                useSidewaysSwitch = new LinearGenerator_UseSidewaysSwitch();
                useSidewaysSwitch.Initialize();
            }

            if (useSidewaysToggleAction == null) {
                useSidewaysToggleAction = new LinearGenerator_UseSidewaysToggleAction();
                useSidewaysToggleAction.Initialize();
            }

            if (useSidewaysEnableAction == null) {
                useSidewaysEnableAction = new LinearGenerator_UseSidewaysEnableAction();
                useSidewaysEnableAction.Initialize();
            }

            if (useSidewaysDisableAction == null) {
                useSidewaysDisableAction = new LinearGenerator_UseSidewaysDisableAction();
                useSidewaysDisableAction.Initialize();
            }

            // sideways

            if (sidewaysSlider == null) {
                sidewaysSlider = new LinearGenerator_SidewaysSlider();
                sidewaysSlider.Initialize();
            }

            if (incraseSidewaysAction == null) {
                incraseSidewaysAction = new LinearGenerator_IncraseSidewaysAction();
                incraseSidewaysAction.Initialize();
            }

            if (decraseSidewaysAction == null) {
                decraseSidewaysAction = new LinearGenerator_DecraseSidewaysAction();
                decraseSidewaysAction.Initialize();
            }

            if (zeroSidewaysAction == null) {
                zeroSidewaysAction = new LinearGenerator_ZeroSidewaysAction();
                zeroSidewaysAction.Initialize();
            }

            // use up

            if (useUpSwitch == null) {
                useUpSwitch = new LinearGenerator_UseUpSwitch();
                useUpSwitch.Initialize();
            }

            if (useUpToggleAction == null) {
                useUpToggleAction = new LinearGenerator_UseUpToggleAction();
                useUpToggleAction.Initialize();
            }

            if (useUpEnableAction == null) {
                useUpEnableAction = new LinearGenerator_UseUpEnableAction();
                useUpEnableAction.Initialize();
            }

            if (useUpDisableAction == null) {
                useUpDisableAction = new LinearGenerator_UseUpDisableAction();
                useUpDisableAction.Initialize();
            }

            // up

            if (upSlider == null) {
                upSlider = new LinearGenerator_UpSlider();
                upSlider.Initialize();
            }

            if (incraseUpAction == null) {
                incraseUpAction = new LinearGenerator_IncraseUpAction();
                incraseUpAction.Initialize();
            }

            if (decraseUpAction == null) {
                decraseUpAction = new LinearGenerator_DecraseUpAction();
                decraseUpAction.Initialize();
            }

            if (zeroUpAction == null) {
                zeroUpAction = new LinearGenerator_ZeroUpAction();
                zeroUpAction.Initialize();
            }

            // use foward

            if (useForwardSwitch == null) {
                useForwardSwitch = new LinearGenerator_UseForwardSwitch();
                useForwardSwitch.Initialize();
            }

            if (useForwardToggleAction == null) {
                useForwardToggleAction = new LinearGenerator_UseForwardToggleAction();
                useForwardToggleAction.Initialize();
            }

            if (useForwardEnableAction == null) {
                useForwardEnableAction = new LinearGenerator_UseForwardEnableAction();
                useForwardEnableAction.Initialize();
            }

            if (useForwardDisableAction == null) {
                useForwardDisableAction = new LinearGenerator_UseForwardDisableAction();
                useForwardDisableAction.Initialize();
            }

            // forward            

            if (forwardSlider == null) {
                forwardSlider = new LinearGenerator_ForwardSlider();
                forwardSlider.Initialize();
            }

            if (incraseForwardAction == null) {
                incraseForwardAction = new LinearGenerator_IncraseForwardAction();
                incraseForwardAction.Initialize();
            }

            if (decraseForwardAction == null) {
                decraseForwardAction = new LinearGenerator_DecraseForwardAction();
                decraseForwardAction.Initialize();
            }

            if (zeroForwardAction == null) {
                zeroForwardAction = new LinearGenerator_ZeroForwardAction();
                zeroForwardAction.Initialize();
            }
        }

        public override void Destroy() {
            Clear();
        }

        void Clear() {
            desiredLinearAcceleration = Vector3D.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Linear Generator ==");
            customInfo.AppendLine("Sideways : " + Math.Round(sideways, 1) + " m/s");
            customInfo.AppendLine("Up : " + Math.Round(up, 1) + " m/s");
            customInfo.AppendLine("Forward : " + Math.Round(forward, 1) + " m/s");

        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }
            Vector3D center = block.WorldAABB.Center;
            float length = block.CubeGrid.LocalAABB.Size.Length() * 10;

            DebugDraw.DrawLine(center, center + desiredLinearAcceleration * length, Color.Cyan, 0.01f);
        }

        #endregion

        #region velocity

        public Vector3D desiredLinearAcceleration;

        #endregion

        #region input       

        Vector3D GetShipControllerLinearInput(IMyShipController shipController) {

            Vector3D desired = Vector3D.Zero;

            if (!shipController.CanControlShip) {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT)) {
                    desired.X = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT)) {
                    desired.X = -1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP)) {
                    desired.Y = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH)) {
                    desired.Y = -1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.FORWARD)) {
                    desired.Z = -1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.BACKWARD)) {
                    desired.Z = 1;
                }
            } else {
                desired.X = MathHelper.Clamp(shipController.MoveIndicator.X, -1, 1);
                desired.Y = MathHelper.Clamp(shipController.MoveIndicator.Y, -1, 1);
                desired.Z = MathHelper.Clamp(shipController.MoveIndicator.Z, -1, 1);
            }

            desired.X = useSideways ? desired.X : 0;
            desired.Y = useUp ? desired.Y : 0;
            desired.Z = useForward ? desired.Z : 0;

            return desired;
        }

        Vector3D GetLinearOverride() {

            Vector3D desired = Vector3D.Zero;

            desired.X = useSideways ? sideways : 0;
            desired.Y = useUp ? up : 0;
            desired.Z = useForward ? -forward : 0;

            // MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "desired: " + desired);

            return desired;
        }

        #endregion



        public Vector3D GetLinearAcceleration(double physicsDeltaTime, double maxLinearAcceleration) {

            desiredLinearAcceleration = Vector3D.Zero;

            IMyCubeGrid grid = block.CubeGrid;
            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            MatrixD blockWorldOrientation = block.PositionComp.WorldMatrix.GetOrientation();
            Vector3D linear = GetLinearOverride();

            if (shipController != null) {
                linear += GetShipControllerLinearInput(shipController);
            }

            desiredLinearAcceleration = Vector3D.Transform(linear * maxLinearAcceleration, blockWorldOrientation);
            desiredLinearAcceleration = Vector3D.ClampToSphere(desiredLinearAcceleration, maxLinearAcceleration);
            return desiredLinearAcceleration;
        }
    }
}