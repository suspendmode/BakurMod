using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class LinearInertialCompensator : LogicElement
    {

        public LinearInertialCompensator(LogicComponent block) : base(block)
        {
        }

        #region use linear compensator

        public readonly string USE_LINEAR_COMPENSATOR_PROPERTY_NAME = "LinearInertialCompensator_UseLinearCompensator";

        public bool defaultUseLinearCompensator = true;

        public bool useLinearCompensator
        {
            set
            {
                string id = GeneratePropertyId(USE_LINEAR_COMPENSATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_LINEAR_COMPENSATOR_PROPERTY_NAME);
                bool result = defaultUseLinearCompensator;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseLinearCompensator;
            }
        }


        #endregion

        #region dumpener

        public readonly string DUMPENER_PROPERTY_NAME = "Compensator_LinearDumpener";

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

        #region use forward

        public readonly string USE_FORWARD_PROPERTY_NAME = "Compensator_UseForward";

        public bool defaultUseForward = true;

        public bool useForward
        {
            set
            {
                string id = GeneratePropertyId(USE_FORWARD_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_FORWARD_PROPERTY_NAME);
                bool result = defaultUseForward;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseForward;
            }

        }

        #endregion

        #region use sideways

        public readonly string USE_SIDEWAYS_PROPERTY_NAME = "Compensator_UseSideways";

        public bool defaultUseSideways = true;

        public bool useSideways
        {
            set
            {
                string id = GeneratePropertyId(USE_SIDEWAYS_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_SIDEWAYS_PROPERTY_NAME);
                bool result = defaultUseSideways;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseSideways;
            }

        }

        #endregion

        #region use up

        public readonly string USE_UP_PROPERTY_NAME = "Compensator_UseUp";

        public bool defaultUseUp = true;

        public bool useUp
        {
            set
            {
                string id = GeneratePropertyId(USE_UP_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_UP_PROPERTY_NAME);
                bool result = defaultUseUp;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseUp;
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
            customInfo.AppendLine("== Linear Inertial Compensator ==");
            customInfo.AppendLine("Dumpener : " + Math.Round(dumpener * 100, 1) + "%");
            customInfo.AppendLine("Use Sideways : " + (useSideways ? "On" : "Off"));
            customInfo.AppendLine("Use Up : " + (useUp ? "On" : "Off"));
            customInfo.AppendLine("Use Forward : " + (useForward ? "On" : "Off"));
        }

        #endregion

        #region input

        bool HasInputForward(IMyShipController controller)
        {
            return Math.Abs(controller.MoveIndicator.Z) > 0;
        }

        bool HasInputSideways(IMyShipController controller)
        {
            return Math.Abs(controller.MoveIndicator.X) > 0;
        }

        bool HasInputUp(IMyShipController controller)
        {
            return Math.Abs(controller.MoveIndicator.Y) > 0;
        }

        IMyThrust[] GetThrusters()
        {
            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
            block.CubeGrid.GetBlocks(blocks, (IMySlimBlock b) =>
            {
                return b.FatBlock is IMyThrust;
            });
            List<IMyThrust> thrusters = new List<IMyThrust>();
            foreach (IMySlimBlock b in blocks)
            {
                thrusters.Add(b.FatBlock as IMyThrust);
            }
            return thrusters.ToArray();
        }

        bool HasOverridedForward(IMyThrust[] thrusters)
        {
            foreach (IMyThrust thruster in thrusters)
            {
                if (thruster.ThrustOverride > 0 && thruster.GridThrustDirection == Vector3I.Forward)
                {
                    return true;
                }
            }
            return false;
        }

        bool HasOverridedSideways(IMyThrust[] thrusters)
        {
            foreach (IMyThrust thruster in thrusters)
            {
                if (thruster.ThrustOverride > 0 && thruster.GridThrustDirection == Vector3I.Right)
                {
                    return true;
                }
            }
            return false;
        }

        bool HasOverridedUp(IMyThrust[] thrusters)
        {
            foreach (IMyThrust thruster in thrusters)
            {
                if (thruster.ThrustOverride > 0 && thruster.GridThrustDirection == Vector3I.Up)
                {
                    return true;
                }
            }
            return false;
        }

        bool HasControlForward()
        {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.FORWARD) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.BACKWARD);
        }

        bool HasControlSideways()
        {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_LEFT) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.STRAFE_RIGHT);
        }

        bool HasControlUp()
        {
            return MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.JUMP) || MyAPIGateway.Input.IsGameControlPressed(MyControlsSpace.CROUCH);
        }

        #endregion

        public Vector3D GetDesiredLinearAcceleration(double physicsDeltaTime)
        {


            MatrixD invertedOrientation = block.WorldMatrixInvScaled.GetOrientation();

            //Vector3D velocityProjectedOnGravity = -(Vector3D)grid.Physics.LinearAcceleration * dumpener;

            //if (component.IsInGravity) {
            //velocityProjectedOnGravity = BakurMathHelper.ProjectOnPlane(velocityProjectedOnGravity, component.rigidbody.gravityUp);
            //}

            Vector3D linearVelocity = (Vector3D)grid.Physics.LinearVelocity;
            //if (logicComponent.rigidbody.IsInGravity)
            //{
            //  linearVelocity = BakurMathHelper.ProjectOnPlane(linearVelocity, logicComponent.rigidbody.gravityUp);
            //}

            Vector3D localLinearAcceleration = Vector3D.Transform(-linearVelocity / physicsDeltaTime * physicsDeltaTime * dumpener, invertedOrientation);

            if (BakurBlockUtils.IsUnderControl(grid))
            {

                IMyThrust[] thrusters = GetThrusters();

                IMyShipController controller = BakurBlockUtils.GetShipControllerUnderControl(grid);

                if (controller.CanControlShip)
                {
                    if (!useSideways || (HasOverridedSideways(thrusters) || HasInputSideways(controller)))
                    {
                        localLinearAcceleration.X = 0;
                    }
                    if (!useUp || (HasOverridedUp(thrusters) || HasInputUp(controller)))
                    {
                        localLinearAcceleration.Y = 0;
                    }
                    if (!useForward || (HasOverridedForward(thrusters) || HasInputForward(controller)))
                    {
                        localLinearAcceleration.Z = 0;
                    }
                }
                else
                {
                    if (!useSideways || (HasOverridedSideways(thrusters) || HasControlSideways()))
                    {
                        localLinearAcceleration.X = 0;
                    }
                    if (!useUp || (HasOverridedUp(thrusters) || HasControlUp()))
                    {
                        localLinearAcceleration.Y = 0;
                    }
                    if (!useForward || (HasOverridedForward(thrusters) || HasControlForward()))
                    {
                        localLinearAcceleration.Z = 0;
                    }
                }
                // MyAPIGateway.Utilities.ShowMessage("Linear Inertia Compensator", "CanControlShip: " + controller.CanControlShip + ", localAngularVelocity: " + localLinearVelocity);
            }

            MatrixD worldOrientation = block.WorldMatrix.GetOrientation();
            desiredLinearAcceleration = Vector3D.Transform(localLinearAcceleration, worldOrientation);
            //desiredLinearAcceleration = Vector3D.ClampToSphere(desiredLinearAcceleration, 90);
            //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "localLinearDumpenerDirection: " + localLinearDumpenerDirection);
            return desiredLinearAcceleration;
        }

        public Vector3D desiredLinearAcceleration;

        public override void Debug()
        {

        }
    }
}
