using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorAngularGenerator : LogicElement
    {

        public double maxAngularVelocity = 30;
        public double maxAngularAcceleration = 45;

        public RepulsorAngularGenerator(LogicComponent component) : base(component)
        {

        }

        #region use angular generator

        public readonly string USE_ANGULAR_GENERATOR_PROPERTY_NAME = "AngularGenerator_UseAngularGenerator";

        public bool defaultUseAngularGenerator = true;

        public bool useAngularGenerator
        {
            set
            {
                string id = GeneratePropertyId(USE_ANGULAR_GENERATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_ANGULAR_GENERATOR_PROPERTY_NAME);
                bool result = defaultUseAngularGenerator;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseAngularGenerator;
            }
        }

        #endregion

        #region power

        public readonly string POWER_PROPERTY_NAME = "AngularGenerator_Power";

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

        #region pitch

        public readonly string PITCH_PROPERTY_NAME = "AngularGenerator_Pitch";

        public double defaultPitch = 0;

        public double pitch
        {
            set
            {
                string id = GeneratePropertyId(PITCH_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(PITCH_PROPERTY_NAME);
                double result = defaultPitch;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultPitch;
            }
        }

        #endregion

        #region yaw

        public readonly string YAW_PROPERTY_NAME = "AngularGenerator_Yaw";

        public double defaultYaw = 0;

        public double yaw
        {
            set
            {
                string id = GeneratePropertyId(YAW_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(YAW_PROPERTY_NAME);
                double result = defaultYaw;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultYaw;
            }
        }

        #endregion

        #region roll

        public readonly string ROLL_PROPERTY_NAME = "AngularGenerator_Roll";

        public double defaultRoll = 0;

        public double roll
        {
            set
            {
                string id = GeneratePropertyId(ROLL_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(ROLL_PROPERTY_NAME);
                double result = defaultRoll;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultRoll;
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
            desiredAngularAcceleration = Vector3D.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Angular Generator ==");
            customInfo.AppendLine("Pitch : " + Math.Round(pitch, 1) + " °/s");
            customInfo.AppendLine("Yaw : " + Math.Round(yaw, 1) + " °/s");
            customInfo.AppendLine("Roll : " + Math.Round(roll, 1) + " °/s²");
        }


        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
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

        public Vector3D GetAngularAcceleration(double physicsDeltaTime)
        {

            desiredAngularAcceleration = Vector3D.Zero;

            if (!useAngularGenerator)
            {
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
            if (shipController != null)
            {
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

        Vector3D angularOverride
        {
            get
            {
                return new Vector3D(pitch, yaw, roll);
            }
        }

        Vector3D GetShipControllerAngularInput(IMyShipController shipController)
        {

            IMyCubeGrid grid = block.CubeGrid;
            Vector3D angularInput = Vector3D.Zero;

            if (!shipController.CanControlShip)
            {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_UP))
                {
                    angularInput.X = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_DOWN))
                {
                    angularInput.X = 1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_LEFT))
                {
                    angularInput.Y = 1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_RIGHT))
                {
                    angularInput.Y = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_LEFT))
                {
                    angularInput.Z = -1;
                }

                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT))
                {
                    angularInput.Z = 1;
                }

            }
            else
            {
                //MyAPIGateway.Input.GetRoll();

                //MyAPIGateway.Input.GetGameControl
                float sensitivity = MyAPIGateway.Input.GetMouseSensitivity() * 0.13f;

                angularInput.X = shipController.RotationIndicator.X < 0 ? 1 : shipController.RotationIndicator.X > 0 ? -1 : 0;
                angularInput.Y = shipController.RotationIndicator.Y < 0 ? 1 : shipController.RotationIndicator.Y > 0 ? -1 : 0;
                angularInput.Z = shipController.RollIndicator < 0 ? 1 : shipController.RollIndicator > 0 ? -1 : 0;
            }

            return angularInput;
        }

        bool HasControlPitch()
        {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_UP) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_DOWN);
        }

        bool HasControlYaw()
        {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_LEFT) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROTATION_RIGHT);
        }

        bool HasControlRoll()
        {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.ROLL_LEFT);

        }

        #endregion

    }
}
