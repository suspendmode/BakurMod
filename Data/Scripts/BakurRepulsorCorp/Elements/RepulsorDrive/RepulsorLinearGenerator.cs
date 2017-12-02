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

        public double maxLinearVelocity = 7.5;
        public double maxLinearAcceleration = 1;

        public RepulsorLinearGenerator(LogicComponent component) : base(component)
        {

        }

        #region use linear generator

        public readonly string USE_LINEAR_DRIVE_PROPERTY_NAME = "LinearGenerator_UseLinearGenerator";

        public bool defaultUseLinearGenerator = true;

        public bool useLinearGenerator
        {
            set
            {
                string id = GeneratePropertyId(USE_LINEAR_DRIVE_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_LINEAR_DRIVE_PROPERTY_NAME);
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

        public readonly string POWER_PROPERTY_NAME = "LinearGenerator_Power";

        public double defaultPower = 1;

        public double power
        {
            set
            {
                string id = GeneratePropertyId(POWER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(POWER_PROPERTY_NAME);
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

        public readonly string FORWARD_PROPERTY_NAME = "LinearGenerator_Forward";

        public double defaultForward = 0;

        public double forward
        {
            set
            {
                string id = GeneratePropertyId(FORWARD_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(FORWARD_PROPERTY_NAME);
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

        public readonly string SIDEWAYS_PROPERTY_NAME = "LinearGenerator_Sideways";

        public double defaultSideways = 0;

        public double sideways
        {
            set
            {
                string id = GeneratePropertyId(SIDEWAYS_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(SIDEWAYS_PROPERTY_NAME);
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

        public readonly string UP_PROPERTY_NAME = "LinearGenerator_UP";

        public double defaultUp = 0;

        public double up
        {
            set
            {
                string id = GeneratePropertyId(UP_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(UP_PROPERTY_NAME);
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
            customInfo.AppendLine("Type: Repulsor Linear Generator");
            customInfo.AppendLine("Sideways: " + Math.Round(sideways, 1) + " m/s");
            customInfo.AppendLine("Up: " + Math.Round(up, 1) + " m/s");
            customInfo.AppendLine("Forward: " + Math.Round(forward, 1) + " m/s");

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
