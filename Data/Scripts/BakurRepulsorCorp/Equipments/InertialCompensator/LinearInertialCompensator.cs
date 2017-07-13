using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class LinearInertialCompensator : BakurBlockEquipment {

        public LinearInertialCompensator(BakurBlock block) : base(block) {
        }

        static Separator<LinearInertialCompensator> linearCompensatorSeparator;
        static Label<LinearInertialCompensator> linearCompensatorLabel;

        #region use linear compensator

        static Compensator_UseLinearCompensatorSwitch useLinearCompensatorSwitch;
        static Compensator_UseLinearCompensationToggleAction useLinearCompensationToggleAction;
        static Compensator_UseLinearCompensationEnableAction useLinearCompensationEnableAction;
        static Compensator_UseLinearCompensationDisableAction useLinearCompensationDisableAction;

        public static string USE_LINEAR_COMPENSATOR_PROPERTY_NAME = "LinearInertialCompensator_UseLinearCompensator";

        public bool defaultUseLinearCompensator = true;

        public bool useLinearCompensator
        {
            set
            {
                string id = GeneratatePropertyId(USE_LINEAR_COMPENSATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_LINEAR_COMPENSATOR_PROPERTY_NAME);
                bool result = defaultUseLinearCompensator;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseLinearCompensator;
            }
        }


        #endregion

        #region dumpener

        static Compensator_LinearDumpenerSlider dumpenerSlider;
        static Compensator_IncraseLinearDumpenerAction incraseDumpenerAction;
        static Compensator_DecraseLinearDumpenerAction decraseDumpenerAction;

        public static string DUMPENER_PROPERTY_NAME = "Compensator_LinearDumpener";

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
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultDumpener;
            }

        }

        #endregion

        #region use forward

        static Compensator_UseForwardSwitch useForwardSwitch;
        static Compensator_UseForwardToggleAction useForwardToggleAction;
        static Compensator_UseForwardEnableAction useForwardEnableAction;
        static Compensator_UseForwardDisableAction useForwardDisableAction;

        public static string USE_FORWARD_PROPERTY_NAME = "Compensator_UseForward";

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

        static Compensator_UseSidewaysSwitch useSidewaysSwitch;
        static Compensator_UseSidewaysToggleAction useSidewaysToggleAction;
        static Compensator_UseSidewaysEnableAction useSidewaysEnableAction;
        static Compensator_UseSidewaysDisableAction useSidewaysDisableAction;

        public static string USE_SIDEWAYS_PROPERTY_NAME = "Compensator_UseSideways";

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

        static Compensator_UseUpSwitch useUpSwitch;
        static Compensator_UseUpToggleAction useUpToggleAction;
        static Compensator_UseUpEnableAction useUpEnableAction;
        static Compensator_UseUpDisableAction useUpDisableAction;

        public static string USE_UP_PROPERTY_NAME = "Compensator_UseUp";

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

        #region lifecycle

        public override void Initialize() {

            if (linearCompensatorSeparator == null) {
                linearCompensatorSeparator = new Separator<LinearInertialCompensator>("LinearInertialCompensator_LinearInertialCompensatorSeparator");
                linearCompensatorSeparator.Initialize();
            }

            if (linearCompensatorLabel == null) {
                linearCompensatorLabel = new Label<LinearInertialCompensator>("LinearInertialCompensator_LinearCompensatorLabel", "Linear Compensator");
                linearCompensatorLabel.Initialize();
            }

            // use linear compensator

            if (useLinearCompensatorSwitch == null) {
                useLinearCompensatorSwitch = new Compensator_UseLinearCompensatorSwitch();
                useLinearCompensatorSwitch.Initialize();
            }

            if (useLinearCompensationToggleAction == null) {
                useLinearCompensationToggleAction = new Compensator_UseLinearCompensationToggleAction();
                useLinearCompensationToggleAction.Initialize();
            }

            if (useLinearCompensationEnableAction == null) {
                useLinearCompensationEnableAction = new Compensator_UseLinearCompensationEnableAction();
                useLinearCompensationEnableAction.Initialize();
            }

            if (useLinearCompensationDisableAction == null) {
                useLinearCompensationDisableAction = new Compensator_UseLinearCompensationDisableAction();
                useLinearCompensationDisableAction.Initialize();
            }

            // dumpener slider 

            if (dumpenerSlider == null) {
                dumpenerSlider = new Compensator_LinearDumpenerSlider();
                dumpenerSlider.Initialize();
            }
            if (incraseDumpenerAction == null) {
                incraseDumpenerAction = new Compensator_IncraseLinearDumpenerAction();
                incraseDumpenerAction.Initialize();
            }
            if (decraseDumpenerAction == null) {
                decraseDumpenerAction = new Compensator_DecraseLinearDumpenerAction();
                decraseDumpenerAction.Initialize();
            }

            // use foward

            if (useForwardSwitch == null) {
                useForwardSwitch = new Compensator_UseForwardSwitch();
                useForwardSwitch.Initialize();
            }

            if (useForwardToggleAction == null) {
                useForwardToggleAction = new Compensator_UseForwardToggleAction();
                useForwardToggleAction.Initialize();
            }

            if (useForwardEnableAction == null) {
                useForwardEnableAction = new Compensator_UseForwardEnableAction();
                useForwardEnableAction.Initialize();
            }

            if (useForwardDisableAction == null) {
                useForwardDisableAction = new Compensator_UseForwardDisableAction();
                useForwardDisableAction.Initialize();

            }

            // use right

            if (useSidewaysSwitch == null) {
                useSidewaysSwitch = new Compensator_UseSidewaysSwitch();
                useSidewaysSwitch.Initialize();
            }

            if (useSidewaysToggleAction == null) {
                useSidewaysToggleAction = new Compensator_UseSidewaysToggleAction();
                useSidewaysToggleAction.Initialize();
            }

            if (useSidewaysEnableAction == null) {
                useSidewaysEnableAction = new Compensator_UseSidewaysEnableAction();
                useSidewaysEnableAction.Initialize();
            }

            if (useSidewaysDisableAction == null) {
                useSidewaysDisableAction = new Compensator_UseSidewaysDisableAction();
                useSidewaysDisableAction.Initialize();
            }

            // use up

            if (useUpSwitch == null) {
                useUpSwitch = new Compensator_UseUpSwitch();
                useUpSwitch.Initialize();
            }

            if (useUpToggleAction == null) {
                useUpToggleAction = new Compensator_UseUpToggleAction();
                useUpToggleAction.Initialize();
            }

            if (useUpEnableAction == null) {
                useUpEnableAction = new Compensator_UseUpEnableAction();
                useUpEnableAction.Initialize();
            }

            if (useUpDisableAction == null) {
                useUpDisableAction = new Compensator_UseUpDisableAction();
                useUpDisableAction.Initialize();
            }
        }

        public override void Destroy() {

        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Linear Inertial Compensator ==");
            customInfo.AppendLine("Dumpener : " + Math.Round(dumpener * 100, 1) + "%");
            customInfo.AppendLine("Use Sideways : " + (useSideways ? "On" : "Off"));
            customInfo.AppendLine("Use Up : " + (useUp ? "On" : "Off"));
            customInfo.AppendLine("Use Forward : " + (useForward ? "On" : "Off"));
        }

        #endregion

        #region input

        bool HasInputForward(IMyShipController controller) {
            return Math.Abs(controller.MoveIndicator.Z) > 0;
        }

        bool HasInputSideways(IMyShipController controller) {
            return Math.Abs(controller.MoveIndicator.X) > 0;
        }

        bool HasInputUp(IMyShipController controller) {
            return Math.Abs(controller.MoveIndicator.Y) > 0;
        }

        IMyThrust[] GetThrusters() {
            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
            block.CubeGrid.GetBlocks(blocks, (IMySlimBlock block) => {
                return block.FatBlock is IMyThrust;
            });
            List<IMyThrust> thrusters = new List<IMyThrust>();
            foreach (IMySlimBlock block in blocks) {
                thrusters.Add(block.FatBlock as IMyThrust);
            }
            return thrusters.ToArray();
        }

        bool HasOverridedForward(IMyThrust[] thrusters) {
            foreach (IMyThrust thruster in thrusters) {
                if (thruster.ThrustOverride > 0 && thruster.GridThrustDirection == Vector3I.Forward) {
                    return true;
                }
            }
            return false;
        }

        bool HasOverridedSideways(IMyThrust[] thrusters) {
            foreach (IMyThrust thruster in thrusters) {
                if (thruster.ThrustOverride > 0 && thruster.GridThrustDirection == Vector3I.Right) {
                    return true;
                }
            }
            return false;
        }

        bool HasOverridedUp(IMyThrust[] thrusters) {
            foreach (IMyThrust thruster in thrusters) {
                if (thruster.ThrustOverride > 0 && thruster.GridThrustDirection == Vector3I.Up) {
                    return true;
                }
            }
            return false;
        }

        bool HasControlForward() {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.FORWARD) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.BACKWARD);
        }

        bool HasControlSideways() {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT);
        }

        bool HasControlUp() {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH);
        }

        #endregion

        public Vector3D GetDesiredLinearAcceleration(double physicsDeltaTime) {

            IMyCubeGrid grid = block.CubeGrid;
            MatrixD invertedOrientation = block.WorldMatrixInvScaled.GetOrientation();

            //Vector3D velocityProjectedOnGravity = -(Vector3D)grid.Physics.LinearAcceleration * dumpener;

            //if (component.IsInGravity) {
            //velocityProjectedOnGravity = BakurMathHelper.ProjectOnPlane(velocityProjectedOnGravity, component.gravityUp);
            //}

            Vector3D localLinearAcceleration = Vector3D.Transform(-(Vector3D)grid.Physics.LinearVelocity * 0.9 * dumpener, invertedOrientation);

            if (BakurBlockUtils.IsUnderControl(grid)) {

                IMyThrust[] thrusters = GetThrusters();

                IMyShipController controller = BakurBlockUtils.GetShipControllerUnderControl(grid);

                if (controller.CanControlShip) {
                    if (!useSideways || (HasOverridedSideways(thrusters) || HasInputSideways(controller))) {
                        localLinearAcceleration.X = 0;
                    }
                    if (!useUp || (HasOverridedUp(thrusters) || HasInputUp(controller))) {
                        localLinearAcceleration.Y = 0;
                    }
                    if (!useForward || (HasOverridedForward(thrusters) || HasInputForward(controller))) {
                        localLinearAcceleration.Z = 0;
                    }
                } else {
                    if (!useSideways || (HasOverridedSideways(thrusters) || HasControlSideways())) {
                        localLinearAcceleration.X = 0;
                    }
                    if (!useUp || (HasOverridedUp(thrusters) || HasControlUp())) {
                        localLinearAcceleration.Y = 0;
                    }
                    if (!useForward || (HasOverridedForward(thrusters) || HasControlForward())) {
                        localLinearAcceleration.Z = 0;
                    }
                }
                // MyAPIGateway.Utilities.ShowMessage("Linear Inertia Compensator", "CanControlShip: " + controller.CanControlShip + ", localAngularVelocity: " + localLinearVelocity);
            }

            MatrixD worldOrientation = block.WorldMatrix.GetOrientation();
            desiredLinearAcceleration = Vector3.Transform(localLinearAcceleration, worldOrientation);
            //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "localLinearDumpenerDirection: " + localLinearDumpenerDirection);
            return desiredLinearAcceleration;
        }

        public Vector3D desiredLinearAcceleration;
    }
}
