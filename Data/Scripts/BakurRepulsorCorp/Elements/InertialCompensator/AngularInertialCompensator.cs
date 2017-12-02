using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{
    public class AngularInertialCompensator : LogicElement
    {

        public AngularInertialCompensator(LogicComponent block) : base(block)
        {
        }

        #region use angular compensator

        public readonly string USE_ANGULAR_COMPENSATOR_PROPERTY_NAME = "AngularInertialCompensator_UseAngularCompensator";

        public bool defaultUseAngularCompensator = true;

        public bool useAngularCompensator
        {
            set
            {
                string id = GeneratePropertyId(USE_ANGULAR_COMPENSATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_ANGULAR_COMPENSATOR_PROPERTY_NAME);
                bool result = defaultUseAngularCompensator;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseAngularCompensator;
            }
        }

        #endregion

        #region dumpener

        public readonly string DUMPENER_PROPERTY_NAME = "Compensator_AngularDumpener";

        public double defaultDumpener = 0.9f;

        public double dumpener
        {
            set
            {
                string id = GeneratePropertyId(DUMPENER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(DUMPENER_PROPERTY_NAME);
                double result = defaultDumpener;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultDumpener;
            }

        }

        #endregion

        #region use pitch

        public readonly string USE_PITCH_PROPERTY_NAME = "Compensator_UsePitch";

        public bool defaultUsePitch = true;

        public bool usePitch
        {
            set
            {
                string id = GeneratePropertyId(USE_PITCH_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_PITCH_PROPERTY_NAME);
                bool result = defaultUsePitch;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUsePitch;
            }

        }

        #endregion

        #region use yaw

        public readonly string USE_YAW_PROPERTY_NAME = "Compensator_UseYaw";

        public bool defaultUseYaw = true;

        public bool useYaw
        {
            set
            {
                string id = GeneratePropertyId(USE_YAW_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_YAW_PROPERTY_NAME);
                bool result = defaultUseYaw;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseYaw;
            }

        }

        #endregion

        #region use roll

        public readonly string USE_ROLL_PROPERTY_NAME = "Compensator_UseRoll";

        public bool defaultUseRoll = true;

        public bool useRoll
        {
            set
            {
                string id = GeneratePropertyId(USE_ROLL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_ROLL_PROPERTY_NAME);
                bool result = defaultUseRoll;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseRoll;
            }

        }

        #endregion

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
            customInfo.AppendLine("== Angular Inertial Compensator ==");
            customInfo.AppendLine("Dumpener : " + Math.Round(dumpener * 100, 1) + "%");
            customInfo.AppendLine("Use Pitch : " + (usePitch ? "On" : "Off"));
            customInfo.AppendLine("Use Yaw : " + (useYaw ? "On" : "Off"));
            customInfo.AppendLine("Use Roll : " + (useRoll ? "On" : "Off"));
        }

        #endregion                

        Vector3D desiredAngularAcceleration;

        public Vector3D GetDesiredAngularAcceleration(double physicsDeltaTime)
        {

            MatrixD invertedOrientation = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D angularVelocity = (Vector3D)grid.Physics.AngularVelocity * BakurMathHelper.Rad2Deg * dumpener;
            Vector3D localAngularAcceleration = Vector3D.Transform(-angularVelocity / physicsDeltaTime * physicsDeltaTime, invertedOrientation);

            if (BakurBlockUtils.IsUnderControl(grid))
            {

                IMyGyro[] gyroscopes = GetGyroscopes();

                IMyShipController controller = BakurBlockUtils.GetShipControllerUnderControl(grid);
                if (controller.CanControlShip)
                {
                    if (!usePitch || HasInputPitch(controller) || HasOverridedPitch(gyroscopes))
                    {
                        localAngularAcceleration.X = 0;
                    }
                    if (!useYaw || HasInputYaw(controller) || HasOverridedYaw(gyroscopes))
                    {
                        localAngularAcceleration.Y = 0;
                    }
                    if (!useRoll || HasInputRoll(controller) || HasOverridedRoll(gyroscopes))
                    {
                        localAngularAcceleration.Z = 0;
                    }
                }
                else
                {
                    if (!usePitch || HasControlPitch() || HasOverridedPitch(gyroscopes))
                    {
                        localAngularAcceleration.X = 0;
                    }
                    if (!useYaw || HasControlYaw() || HasOverridedYaw(gyroscopes))
                    {
                        localAngularAcceleration.Y = 0;
                    }
                    if (!useRoll || HasControlRoll() || HasOverridedRoll(gyroscopes))
                    {
                        localAngularAcceleration.Z = 0;
                    }
                }
            }

            MatrixD worldOrientation = grid.WorldMatrix.GetOrientation();
            desiredAngularAcceleration = Vector3.Transform(localAngularAcceleration, worldOrientation);
            //desiredAngularAcceleration = Vector3D.ClampToSphere(desiredAngularAcceleration, 90);
            return desiredAngularAcceleration;
            //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "desiredAngularVelocity: " + desiredAngularVelocity.Length() + ", radiusSquared: " + radiusSquared + ", inertia: " + inertia + ", desiredTorque: " + desiredTorque);
        }

        bool HasInputPitch(IMyShipController controller)
        {
            return Math.Abs(controller.RotationIndicator.X) > 0;
        }

        bool HasInputYaw(IMyShipController controller)
        {
            return Math.Abs(controller.RotationIndicator.Y) > 0;
        }

        bool HasInputRoll(IMyShipController controller)
        {
            return Math.Abs(controller.RollIndicator) > 0;
        }


        IMyGyro[] GetGyroscopes()
        {
            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
            block.CubeGrid.GetBlocks(blocks, (IMySlimBlock b) =>
            {
                return b.FatBlock is IMyGyro;
            });
            List<IMyGyro> thrusters = new List<IMyGyro>();
            foreach (IMySlimBlock b in blocks)
            {
                thrusters.Add(b.FatBlock as IMyGyro);
            }
            return thrusters.ToArray();
        }

        bool HasOverridedPitch(IMyGyro[] gyroscopes)
        {
            foreach (IMyGyro gyroscope in gyroscopes)
            {
                if (gyroscope.GyroOverride && Math.Abs(gyroscope.Pitch) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        bool HasOverridedYaw(IMyGyro[] gyroscopes)
        {
            foreach (IMyGyro gyroscope in gyroscopes)
            {
                if (gyroscope.GyroOverride && Math.Abs(gyroscope.Yaw) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        bool HasOverridedRoll(IMyGyro[] gyroscopes)
        {
            foreach (IMyGyro gyroscope in gyroscopes)
            {
                if (gyroscope.GyroOverride && Math.Abs(gyroscope.Roll) > 0)
                {
                    return true;
                }
            }
            return false;
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

        public override void Debug()
        {

        }
    }
}
