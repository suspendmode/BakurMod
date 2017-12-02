using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorSuspension : LogicElement
    {

        public RepulsorSuspension(LogicComponent component) : base(component)
        {
        }

        #region damping

        public readonly string DAMPING_PROPERTY_NAME = "RepulsorSuspension_Damping";

        public double defaultDamping = RepulsorSuspensionSettings.maximumDumpening;

        public double damping
        {
            set
            {
                string id = GeneratePropertyId(DAMPING_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(DAMPING_PROPERTY_NAME);
                double result = defaultDamping;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultDamping;
            }

        }

        #endregion

        #region rest length

        public readonly string REST_LENGTH_PROPERTY_NAME = "RepulsorSuspension_RestLength";

        public double defaultRestLength = double.NaN;

        public double restLength
        {
            set
            {
                string id = GeneratePropertyId(REST_LENGTH_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(REST_LENGTH_PROPERTY_NAME);
                double result = defaultRestLength;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultRestLength;
            }
        }

        #endregion

        #region stiffness

        public readonly string STIFFNES_PROPERTY_NAME = "RepulsorSuspension_Stiffness";

        public double defaultStiffness = 8;

        public double stiffness
        {
            set
            {
                string id = GeneratePropertyId(STIFFNES_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(STIFFNES_PROPERTY_NAME);
                double result = defaultStiffness;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultStiffness;
            }
        }

        #endregion

        #region impulse

        public readonly string IMPULSE_PROPERTY_NAME = "RepulsorSuspension_Impulse";

        public double defaultImpulse = RepulsorSuspensionSettings.maximumImpulse;

        public double impulse
        {
            set
            {
                string id = GeneratePropertyId(IMPULSE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(IMPULSE_PROPERTY_NAME);
                double result = defaultImpulse;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultImpulse;
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
            desiredForce = Vector3D.Zero;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Suspension");
            customInfo.AppendLine("Stiffness: " + Math.Round(stiffness, 1));
            customInfo.AppendLine("Rest Length: " + Math.Round(restLength, 1) + " m");
            customInfo.AppendLine("Damping: " + Math.Round(damping, 1));
            customInfo.AppendLine("Ratio: " + Math.Round(ratio, 3));
            customInfo.AppendLine("Impulse: " + Math.Round(impulse, 1));
            customInfo.AppendLine("Altitude: " + Math.Round(altitude, 1) + " m");
            customInfo.AppendLine("Previous Altitude: " + Math.Round(previousAltitude, 1) + " m");
            customInfo.AppendLine("Desired Force: " + Math.Round(desiredForce.Length() / 1000, 3) + " kN");
        }

        #endregion

        public Vector3D desiredForce;
        public double altitude;
        double previousAltitude;
        public Vector3D desiredUp;
        double ratio;

        public Vector3D GetForce(double physicsDeltaTime, Vector3D desiredUp, double altitude)
        {

            this.altitude = altitude;
            this.desiredUp = desiredUp;

            desiredForce = Vector3.Zero;

            // auto distance if (double.IsNaN(restLength) && !double.IsNaN(this.distance)) {

            if (altitude >= RepulsorSuspensionSettings.maximumRestLength)
            {
                return desiredForce;
            }


            if (double.IsNaN(restLength))
            {
                restLength = (grid.LocalAABB.Size.Length() * 0.5f) + 0.5f;
                restLength = MathHelper.Clamp(restLength * 0.5f, RepulsorSuspensionSettings.minimumRestLength, RepulsorSuspensionSettings.maximumRestLength);
            }

            // desired altitude
            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null)
            {
                restLength += GetShipControllerLinearInput(physicsDeltaTime, shipController);
            }

            restLength = MathHelper.Clamp(restLength, RepulsorSuspensionSettings.minimumRestLength, RepulsorSuspensionSettings.maximumRestLength);

            ratio = stiffness * (restLength - this.altitude) + damping * (previousAltitude - this.altitude);
            previousAltitude = this.altitude;

            desiredForce = desiredUp * MathHelper.Clamp(ratio * impulse * logicComponent.rigidbody.gridMass * logicComponent.rigidbody.gravity.Length(), 0, double.MaxValue);

            return desiredForce;
        }

        double GetShipControllerLinearInput(double physicsDeltaTime, IMyShipController shipController)
        {

            double desired = 0;

            if (!shipController.CanControlShip)
            {
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP))
                {
                    desired = 1;
                }
                if (MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH))
                {
                    desired = -1;
                }
            }
            else
            {
                desired = MathHelper.Clamp(shipController.MoveIndicator.Y, -1, 1);
            }
            desired *= (physicsDeltaTime * physicsDeltaTime * 0.3);
            return desired;

        }
        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }
        }
    }
}
