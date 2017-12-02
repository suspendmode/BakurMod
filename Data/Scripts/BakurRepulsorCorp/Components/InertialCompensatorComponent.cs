using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockInertialCompensator", "LargeBlockInertialCompensator" })]
    public class InertialCompensatorComponent : PoweredLogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        double maxLinearAcceleration = 100;
        double maxAngularAcceleration = 180;

        LinearInertialCompensator linearInertialCompensator;
        LinearInertialCompensatorUIController<IMyUpgradeModule> linearInertialCompensatorUI;

        AngularInertialCompensator angularInertialCompensator;
        AngularInertialCompensatorUIController<IMyUpgradeModule> angularInertialCompensatorUI;

        #region lifecycle

        protected override void Initialize()
        {
            base.Initialize();
            //MyAPIGateway.Utilities.ShowMessage("InertialCompensatorBlock", "Initialize");

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            linearInertialCompensator = new LinearInertialCompensator(this);
            AddElement(linearInertialCompensator);

            linearInertialCompensatorUI = new LinearInertialCompensatorUIController<IMyUpgradeModule>(this);
            AddElement(linearInertialCompensatorUI);

            angularInertialCompensator = new AngularInertialCompensator(this);
            AddElement(angularInertialCompensator);

            angularInertialCompensatorUI = new AngularInertialCompensatorUIController<IMyUpgradeModule>(this);
            AddElement(angularInertialCompensatorUI);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(linearInertialCompensator);
            linearInertialCompensator = null;

            RemoveElement(linearInertialCompensatorUI);
            linearInertialCompensatorUI = null;

            RemoveElement(angularInertialCompensator);
            angularInertialCompensator = null;

            RemoveElement(angularInertialCompensatorUI);
            angularInertialCompensatorUI = null;
        }

        #endregion

        #region update

        protected override double UpdatePoweredDevice(double physicsDeltaTime, double power_ratio)
        {
            //projectedVelocity = MyMath.ForwardVectorProjection(-component.Gravity, projectedVelocity);
            //double speedNormalized = MathHelper.Clamp(currentVelocity.Length() / 100, 0, 1);            

            Vector3D desiredLinearAcceleration = Vector3D.Zero;
            Vector3D desiredAngularAcceleration = Vector3D.Zero;

            // linear compensator

            if (linearInertialCompensator.useLinearCompensator)
            {
                desiredLinearAcceleration = linearInertialCompensator.GetDesiredLinearAcceleration(physicsDeltaTime) * power_ratio;
                desiredLinearAcceleration = Vector3D.ClampToSphere(desiredLinearAcceleration, maxLinearAcceleration);
                rigidbody.AddLinearAcceleration(desiredLinearAcceleration);
            }
            else
            {
                if (!angularInertialCompensator.useAngularCompensator)
                {
                    return 0;
                }
            }

            // angular compensator

            if (angularInertialCompensator.useAngularCompensator)
            {
                desiredAngularAcceleration = angularInertialCompensator.GetDesiredAngularAcceleration(physicsDeltaTime) * power_ratio;
                desiredAngularAcceleration = Vector3D.ClampToSphere(desiredAngularAcceleration, maxAngularAcceleration);
                rigidbody.AddAngularAcceleration(desiredAngularAcceleration);
            }
            else
            {
                if (!linearInertialCompensator.useLinearCompensator)
                {
                    return 0;
                }
            }

            double desiredLinearAccelerationNormalized = BakurMathHelper.InverseLerp(0, maxLinearAcceleration, desiredLinearAcceleration.Length());
            double desiredAngularAccelerationNormalized = BakurMathHelper.InverseLerp(0, maxAngularAcceleration, desiredAngularAcceleration.Length());

            return Math.Max(desiredLinearAccelerationNormalized, desiredAngularAccelerationNormalized);
        }


        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {

            base.UpdateAfterSimulation(physicsDeltaTime);


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
