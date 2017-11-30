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

        static Separator<AngularInertialCompensator> angularCompensatorSeparator;
        static Label<AngularInertialCompensator> angularCompensatorLabel;

        #region use angular compensator

        static Compensator_UseAngularCompensatorSwitch useAngularCompensatorSwitch;
        static Compensator_UseAngularCompensationToggleAction useAngularCompensationToggleAction;
        static Compensator_UseAngularCompensationEnableAction useAngularCompensationEnableAction;
        static Compensator_UseAngularCompensationDisableAction useAngularCompensationDisableAction;

        public static string USE_ANGULAR_COMPENSATOR_PROPERTY_NAME = "AngularInertialCompensator_UseAngularCompensator";

        public bool defaultUseAngularCompensator = true;

        public bool useAngularCompensator
        {
            set
            {
                string id = GeneratatePropertyId(USE_ANGULAR_COMPENSATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_ANGULAR_COMPENSATOR_PROPERTY_NAME);
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

        static Compensator_AngularDumpenerSlider dumpenerSlider;
        static Compensator_IncraseAngularDumpenerAction incraseDumpenerAction;
        static Compensator_DecraseAngularDumpenerAction decraseDumpenerAction;

        public static string DUMPENER_PROPERTY_NAME = "Compensator_AngularDumpener";

        public double defaultDumpener = 0.9f;

        public double dumpener
        {
            set
            {
                string id = GeneratatePropertyId(DUMPENER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(DUMPENER_PROPERTY_NAME);
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

        static Compensator_UsePitchSwitch usePitchSwitch;
        static Compensator_UsePitchToggleAction usePitchToggleAction;
        static Compensator_UsePitchEnableAction usePitchEnableAction;
        static Compensator_UsePitchDisableAction usePitchDisableAction;

        public static string USE_PITCH_PROPERTY_NAME = "Compensator_UsePitch";

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
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUsePitch;
            }

        }

        #endregion

        #region use yaw

        static Compensator_UseYawSwitch useYawSwitch;
        static Compensator_UseYawToggleAction useYawToggleAction;
        static Compensator_UseYawEnableAction useYawEnableAction;
        static Compensator_UseYawDisableAction useYawDisableAction;

        public static string USE_YAW_PROPERTY_NAME = "Compensator_UseYaw";

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
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseYaw;
            }

        }

        #endregion

        #region use roll

        static Compensator_UseRollSwitch useRollSwitch;
        static Compensator_UseRollToggleAction useRollToggleAction;
        static Compensator_UseRollEnableAction useRollEnableAction;
        static Compensator_UseRollDisableAction useRollDisableAction;

        public static string USE_ROLL_PROPERTY_NAME = "Compensator_UseRoll";

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

            if (angularCompensatorSeparator == null)
            {
                angularCompensatorSeparator = new Separator<AngularInertialCompensator>("AngularInertialCompensator_AngularCompensatorSeparator");
                angularCompensatorSeparator.Initialize();
            }


            if (angularCompensatorLabel == null)
            {
                angularCompensatorLabel = new Label<AngularInertialCompensator>("AngularInertialCompensator_AngularCompensatorLabel", "Angular Compensator");
                angularCompensatorLabel.Initialize();
            }


            // use angular compensator

            if (useAngularCompensatorSwitch == null)
            {
                useAngularCompensatorSwitch = new Compensator_UseAngularCompensatorSwitch();
                useAngularCompensatorSwitch.Initialize();
            }

            if (useAngularCompensationToggleAction == null)
            {
                useAngularCompensationToggleAction = new Compensator_UseAngularCompensationToggleAction();
                useAngularCompensationToggleAction.Initialize();
            }

            if (useAngularCompensationEnableAction == null)
            {
                useAngularCompensationEnableAction = new Compensator_UseAngularCompensationEnableAction();
                useAngularCompensationEnableAction.Initialize();
            }

            if (useAngularCompensationDisableAction == null)
            {
                useAngularCompensationDisableAction = new Compensator_UseAngularCompensationDisableAction();
                useAngularCompensationDisableAction.Initialize();
            }

            // dumpener slider 

            if (dumpenerSlider == null)
            {
                dumpenerSlider = new Compensator_AngularDumpenerSlider();
                dumpenerSlider.Initialize();
            }
            if (incraseDumpenerAction == null)
            {
                incraseDumpenerAction = new Compensator_IncraseAngularDumpenerAction();
                incraseDumpenerAction.Initialize();
            }
            if (decraseDumpenerAction == null)
            {
                decraseDumpenerAction = new Compensator_DecraseAngularDumpenerAction();
                decraseDumpenerAction.Initialize();
            }

            // use pitch

            if (usePitchSwitch == null)
            {
                usePitchSwitch = new Compensator_UsePitchSwitch();
                usePitchSwitch.Initialize();
            }

            if (usePitchToggleAction == null)
            {
                usePitchToggleAction = new Compensator_UsePitchToggleAction();
                usePitchToggleAction.Initialize();
            }

            if (usePitchEnableAction == null)
            {
                usePitchEnableAction = new Compensator_UsePitchEnableAction();
                usePitchEnableAction.Initialize();
            }

            if (usePitchDisableAction == null)
            {
                usePitchDisableAction = new Compensator_UsePitchDisableAction();
                usePitchDisableAction.Initialize();
            }

            // use yaw

            if (useYawSwitch == null)
            {
                useYawSwitch = new Compensator_UseYawSwitch();
                useYawSwitch.Initialize();
            }

            if (useYawToggleAction == null)
            {
                useYawToggleAction = new Compensator_UseYawToggleAction();
                useYawToggleAction.Initialize();
            }

            if (useYawEnableAction == null)
            {
                useYawEnableAction = new Compensator_UseYawEnableAction();
                useYawEnableAction.Initialize();
            }

            if (useYawDisableAction == null)
            {
                useYawDisableAction = new Compensator_UseYawDisableAction();
                useYawDisableAction.Initialize();
            }


            // use roll

            if (useRollSwitch == null)
            {
                useRollSwitch = new Compensator_UseRollSwitch();
                useRollSwitch.Initialize();
            }

            if (useRollToggleAction == null)
            {
                useRollToggleAction = new Compensator_UseRollToggleAction();
                useRollToggleAction.Initialize();
            }

            if (useRollEnableAction == null)
            {
                useRollEnableAction = new Compensator_UseRollEnableAction();
                useRollEnableAction.Initialize();
            }

            if (useRollDisableAction == null)
            {
                useRollDisableAction = new Compensator_UseRollDisableAction();
                useRollDisableAction.Initialize();
            }
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
