using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockInertialCompensator", "LargeBlockInertialCompensator" })]
    public class InertialCompensatorComponent : LogicComponent
    {

        LinearInertialCompensator linearInertialCompensator;
        AngularInertialCompensator angularInertialCompensator;

        #region lifecycle

        protected override void Initialize()
        {
            base.Initialize();
            //MyAPIGateway.Utilities.ShowMessage("InertialCompensatorBlock", "Initialize");

            linearInertialCompensator = new LinearInertialCompensator(this);
            AddEquipment(linearInertialCompensator);

            angularInertialCompensator = new AngularInertialCompensator(this);
            AddEquipment(angularInertialCompensator);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(linearInertialCompensator);
            linearInertialCompensator = null;

            RemoveEquipment(angularInertialCompensator);
            angularInertialCompensator = null;
        }

        #endregion

        #region update

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            //projectedVelocity = MyMath.ForwardVectorProjection(-component.Gravity, projectedVelocity);
            //double speedNormalized = MathHelper.Clamp(currentVelocity.Length() / 100, 0, 1);

            IMyCubeGrid grid = block.CubeGrid;

            Vector3D desiredLinearAcceleration = Vector3D.Zero;
            Vector3D desiredAngularAcceleration = Vector3D.Zero;

            // linear compensator

            if (linearInertialCompensator.useLinearCompensator)
            {
                desiredLinearAcceleration = linearInertialCompensator.GetDesiredLinearAcceleration(physicsDeltaTime);
                rigidbody.AddLinearAcceleration(desiredLinearAcceleration);
            }

            // angular compensator

            if (angularInertialCompensator.useAngularCompensator)
            {
                desiredAngularAcceleration = angularInertialCompensator.GetDesiredAngularAcceleration(physicsDeltaTime);
                rigidbody.AddAngularAcceleration(desiredAngularAcceleration);
            }
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Inertial Compensator Component");

            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "inertial_compensator_working_start", "inertial_compensator_working_loop", "inertial_compensator_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("44126a78-23f0-42ec-ade9-268fc6adb9b0");
        }
    }

}
