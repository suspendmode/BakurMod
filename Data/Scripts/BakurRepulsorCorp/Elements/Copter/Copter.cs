﻿using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{
    public static class CopterMode
    {
        public const string GLIDE_MODE = "glide";
        public const string FREE_GLIDE_MODE = "freeglide";
        public const string HOVER_MODE = "hover";
        public const string PITCH_MODE = "pitch";
        public const string ROLL_MODE = "roll";
        public const string CRUISE_MODE = "cruise";

        public static string[] modes = { "Hover", "Glide", "FreeGlide", "Pitch", "Roll", "Cruise" };
    }

    public class Copter : LogicElement
    {

        #region responsivity

        public readonly string RESPONSIVITY_PROPERTY_NAME = "Copter_Responsivity";

        public double defaultResponsivity = 10;

        public double responsivity
        {
            set
            {
                string id = GeneratePropertyId(RESPONSIVITY_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(RESPONSIVITY_PROPERTY_NAME);
                double result = defaultResponsivity;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultResponsivity;
            }
        }

        #endregion

        #region max pitch angle

        public readonly string MAX_PITCH_PROPERTY_NAME = "Copter_MaxPitch";

        public double defaultMaxPitch = 15;

        public double maxPitch
        {
            set
            {
                string id = GeneratePropertyId(MAX_PITCH_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MAX_PITCH_PROPERTY_NAME);
                double result = defaultMaxPitch;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxPitch;
            }
        }

        #endregion

        #region max roll angle

        public readonly string MAX_ROLL_PROPERTY_NAME = "Copter_MaxRoll";

        public double defaultMaxRoll = 15;

        public double maxRoll
        {
            set
            {
                string id = GeneratePropertyId(MAX_ROLL_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MAX_ROLL_PROPERTY_NAME);
                double result = defaultMaxRoll;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxRoll;
            }
        }

        #endregion

        #region cruise speed

        public readonly string CRUISE_SPEED_PROPERTY_NAME = "Copter_CruiseSpeed";

        public double defaultCruiseSpeed = 5;

        public double cruiseSpeed
        {
            set
            {
                string id = GeneratePropertyId(CRUISE_SPEED_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(CRUISE_SPEED_PROPERTY_NAME);
                double result = defaultCruiseSpeed;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultCruiseSpeed;
            }
        }

        #endregion

        #region mode

        public readonly string MODE_PROPERTY_NAME = "Copter_Mode";

        public string defaultMode = "Hover";

        public string mode
        {
            set
            {
                string id = GeneratePropertyId(MODE_PROPERTY_NAME);
                SetVariable<string>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MODE_PROPERTY_NAME);
                string result = defaultMode;
                if (GetVariable<string>(id, out result))
                {
                    return result;
                }
                return defaultMode;
            }
        }

        #endregion

        #region use copter

        public readonly string USE_COPTER_PROPERTY_NAME = "Copter_UseCopter";

        public bool defaultUseCopter = true;

        public bool useCopter
        {
            set
            {
                string id = GeneratePropertyId(USE_COPTER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_COPTER_PROPERTY_NAME);
                bool result = defaultUseCopter;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseCopter;
            }
        }


        #endregion

        protected double desiredPitch = 0, desiredRoll = 0;
        protected double pitch = 0, roll = 0, yaw = 0;

        public Vector3D desiredUp;

        public Copter(LogicComponent block) : base(block)
        {
        }

        #region lifecycle

        public override void Initialize()
        {
        }

        public override void Destroy()
        {

        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
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

        public Vector3 GetDesiredUp(Vector3D defaultUp)
        {



            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D worldLinearVelocity = grid.Physics.LinearVelocity;
            Vector3D localLinearVelocity = Vector3D.Transform(worldLinearVelocity, invWorldRot);

            double forwardSpeed = localLinearVelocity.Z;
            double sidewaysSpeed = localLinearVelocity.X;
            //localSpeed = worldSpeed;

            pitch = Vector3.Dot(logicComponent.rigidbody.gravityUp, block.WorldMatrix.Forward) / (logicComponent.rigidbody.gravity.Length() * block.WorldMatrix.Forward.Length()) * 90;
            roll = Vector3.Dot(logicComponent.rigidbody.gravityUp, block.WorldMatrix.Right) / (logicComponent.rigidbody.gravity.Length() * block.WorldMatrix.Right.Length()) * 90;

            if (double.IsNaN(pitch))
            {
                pitch = 0;
            }

            if (double.IsNaN(roll))
            {
                roll = 0;
            }

            switch (mode.ToLower())
            {
                case CopterMode.GLIDE_MODE:
                    desiredPitch = 0;
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;

                case CopterMode.FREE_GLIDE_MODE:
                    desiredPitch = 0;
                    desiredRoll = 0;
                    break;

                case CopterMode.PITCH_MODE:
                    desiredPitch = Curve(forwardSpeed / responsivity, maxPitch);
                    desiredRoll = roll;
                    break;

                case CopterMode.ROLL_MODE:
                    desiredPitch = pitch;
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;

                case CopterMode.CRUISE_MODE:
                    desiredPitch = Curve((forwardSpeed + cruiseSpeed) / responsivity, maxPitch);
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;

                case CopterMode.HOVER_MODE:
                default:
                    desiredPitch = Curve(forwardSpeed / responsivity, maxPitch);
                    desiredRoll = Curve(sidewaysSpeed / responsivity, maxRoll);
                    break;
            }

            double pitchRate = (desiredPitch - pitch) / 90;
            double rollRate = (desiredRoll - roll) / 90;

            IMyShipController shipController = BakurBlockUtils.GetShipControllerUnderControl(grid);

            if (shipController != null && Math.Abs(shipController.MoveIndicator.X) > 0.1)
            {
                pitchRate = 0;
            }

            if (shipController != null && Math.Abs(shipController.MoveIndicator.Z) > 0.1)
            {
                rollRate = 0;
            }

            //double pitchRate = (desiredPitch) / 90;
            //double rollRate = (desiredPitch) / 90;
            // Quaternion quatPitch = BakurMathHelper.AxisAngle(Vector3.Right, (float)pitchRate);
            // MyAPIGateway.Utilities.ShowMessage("Copter", "P: " + Math.Round(desiredPitch, 1) + "," + Math.Round(pitchRate, 1));
            //MyAPIGateway.Utilities.ShowMessage("Copter", "R: " + Math.Round(desiredRoll, 1) + "," + Math.Round(rollRate, 1));

            Matrix blockOrientation = block.WorldMatrix.GetOrientation();
            //Vector3 rotationVec = new Vector3(pitchRate, 0, rollRate) * (float)BakurMathHelper.Deg2Rad;
            Vector3 rotationVec = new Vector3(-pitchRate, 0, rollRate);
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

        double Curve(double value, double max)
        {
            return Math.Atan(value / MathHelper.PiOver2) * max;
        }

        public override void Debug()
        {

        }
    }

}
