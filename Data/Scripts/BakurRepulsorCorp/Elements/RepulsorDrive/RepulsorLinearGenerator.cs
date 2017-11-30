using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorLinearGenerator : LogicElement
    {

        public double maxLinearVelocity = 3;
        public double maxLinearAcceleration = 0.2;

        public RepulsorLinearGenerator(LogicComponent component) : base(component)
        {

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
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseLinearGenerator;
            }
        }

        #endregion

        #region power

        static LinearGenerator_PowerSlider powerSlider;
        static LinearGenerator_IncrasePowerAction incrasePowerAction;
        static LinearGenerator_DecrasePowerAction decrasePowerAction;

        public static string POWER_PROPERTY_NAME = "LinearGenerator_Power";

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
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultPower;
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
                if (GetVariable<double>(id, out result))
                {
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
                if (GetVariable<double>(id, out result))
                {
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
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultUp;
            }
        }

        #endregion

        #region lifecycle

        public override void Initialize()
        {

            // label

            if (repulsorLinearGeneratorSeparator == null)
            {
                repulsorLinearGeneratorSeparator = new Separator<RepulsorLinearGenerator>("LinearGenerator_GeneratorSeparator");
                repulsorLinearGeneratorSeparator.Initialize();
            }

            if (repulsorLinearGeneratorLabel == null)
            {
                repulsorLinearGeneratorLabel = new Label<RepulsorLinearGenerator>("LinearGenerator_RepulsorLinearGeneratorLabel", "Repulsor Linear Generator");
                repulsorLinearGeneratorLabel.Initialize();
            }

            // linear

            if (linearOverrideSeparator == null)
            {
                linearOverrideSeparator = new Separator<RepulsorLinearGenerator>("LinearGenerator_LinearOverrideSeparator");
                linearOverrideSeparator.Initialize();
            }

            if (linearOverrideLabel == null)
            {
                linearOverrideLabel = new Label<RepulsorLinearGenerator>("LinearGenerator_LinearOverrideLabel", "Linear Override");
                linearOverrideLabel.Initialize();
            }

            // use linear generator

            if (useLinearGeneratorSwitch == null)
            {
                useLinearGeneratorSwitch = new LinearGenerator_UseLinearGeneratorSwitch();
                useLinearGeneratorSwitch.Initialize();
            }

            if (useLinearGeneratorToggleAction == null)
            {
                useLinearGeneratorToggleAction = new LinearGenerator_UseLinearGeneratorToggleAction();
                useLinearGeneratorToggleAction.Initialize();
            }

            if (useLinearGeneratorEnableAction == null)
            {
                useLinearGeneratorEnableAction = new LinearGenerator_UseLinearGeneratorEnableAction();
                useLinearGeneratorEnableAction.Initialize();
            }

            if (useLinearGeneratorDisableAction == null)
            {
                useLinearGeneratorDisableAction = new LinearGenerator_UseLinearGeneratorDisableAction();
                useLinearGeneratorDisableAction.Initialize();
            }

            // power

            if (powerSlider == null)
            {
                powerSlider = new LinearGenerator_PowerSlider();
                powerSlider.Initialize();
            }
            if (incrasePowerAction == null)
            {
                incrasePowerAction = new LinearGenerator_IncrasePowerAction();
                incrasePowerAction.Initialize();
            }
            if (decrasePowerAction == null)
            {
                decrasePowerAction = new LinearGenerator_DecrasePowerAction();
                decrasePowerAction.Initialize();
            }

            // sideways

            if (sidewaysSlider == null)
            {
                sidewaysSlider = new LinearGenerator_SidewaysSlider();
                sidewaysSlider.Initialize();
            }

            if (incraseSidewaysAction == null)
            {
                incraseSidewaysAction = new LinearGenerator_IncraseSidewaysAction();
                incraseSidewaysAction.Initialize();
            }

            if (decraseSidewaysAction == null)
            {
                decraseSidewaysAction = new LinearGenerator_DecraseSidewaysAction();
                decraseSidewaysAction.Initialize();
            }

            if (zeroSidewaysAction == null)
            {
                zeroSidewaysAction = new LinearGenerator_ZeroSidewaysAction();
                zeroSidewaysAction.Initialize();
            }

            // up

            if (upSlider == null)
            {
                upSlider = new LinearGenerator_UpSlider();
                upSlider.Initialize();
            }

            if (incraseUpAction == null)
            {
                incraseUpAction = new LinearGenerator_IncraseUpAction();
                incraseUpAction.Initialize();
            }

            if (decraseUpAction == null)
            {
                decraseUpAction = new LinearGenerator_DecraseUpAction();
                decraseUpAction.Initialize();
            }

            if (zeroUpAction == null)
            {
                zeroUpAction = new LinearGenerator_ZeroUpAction();
                zeroUpAction.Initialize();
            }

            // forward            

            if (forwardSlider == null)
            {
                forwardSlider = new LinearGenerator_ForwardSlider();
                forwardSlider.Initialize();
            }

            if (incraseForwardAction == null)
            {
                incraseForwardAction = new LinearGenerator_IncraseForwardAction();
                incraseForwardAction.Initialize();
            }

            if (decraseForwardAction == null)
            {
                decraseForwardAction = new LinearGenerator_DecraseForwardAction();
                decraseForwardAction.Initialize();
            }

            if (zeroForwardAction == null)
            {
                zeroForwardAction = new LinearGenerator_ZeroForwardAction();
                zeroForwardAction.Initialize();
            }
        }

        public override void Destroy()
        {
            Clear();
        }

        void Clear()
        {
            desiredLinearAcceleration = Vector3D.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Linear Generator ==");
            customInfo.AppendLine("Sideways : " + Math.Round(sideways, 1) + " m/s");
            customInfo.AppendLine("Up : " + Math.Round(up, 1) + " m/s");
            customInfo.AppendLine("Forward : " + Math.Round(forward, 1) + " m/s");

        }

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
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

        Vector3D linearOverride
        {
            get
            {
                return new Vector3D(sideways, up, -forward);
            }
        }

        Vector3D GetShipControllerLinearInput(IMyShipController shipController)
        {

            Vector3D linearInput = Vector3D.Zero;

            if (!shipController.CanControlShip)
            {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT))
                {
                    linearInput.X = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT))
                {
                    linearInput.X = -1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP))
                {
                    linearInput.Y = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH))
                {
                    linearInput.Y = -1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.FORWARD))
                {
                    linearInput.Z = -1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.BACKWARD))
                {
                    linearInput.Z = 1;
                }
            }
            else
            {
                linearInput.X = shipController.MoveIndicator.X < 0 ? -1 : shipController.MoveIndicator.X > 0 ? 1 : 0;
                linearInput.Y = shipController.MoveIndicator.Y < 0 ? -1 : shipController.MoveIndicator.Y > 0 ? 1 : 0;
                linearInput.Z = shipController.MoveIndicator.Z < 0 ? -1 : shipController.MoveIndicator.Z > 0 ? 1 : 0;
            }

            return linearInput;
        }



        #endregion



        public Vector3D GetLinearAcceleration(double physicsDeltaTime)
        {

            desiredLinearAcceleration = Vector3D.Zero;

            if (!useLinearGenerator)
            {
                Clear();
                return desiredLinearAcceleration;
            }

            IMyCubeGrid grid = block.CubeGrid;

            if (grid.Physics.LinearVelocity.Length() > maxLinearVelocity)
            {
                Clear();
                return desiredLinearAcceleration;
            }

            MatrixD blockWorldOrientation = block.PositionComp.WorldMatrix.GetOrientation();
            Vector3D linear = linearOverride;

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);
            if (shipController != null)
            {
                linear += GetShipControllerLinearInput(shipController);
            }

            linear *= maxLinearVelocity;

            desiredLinearAcceleration = Vector3D.Transform(linear, blockWorldOrientation) / physicsDeltaTime;
            desiredLinearAcceleration = Vector3D.ClampToSphere(desiredLinearAcceleration, maxLinearAcceleration);
            return desiredLinearAcceleration * power;
        }
    }
}
