using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorChair", "LargeBlockRepulsorChair" })]
    public class RepulsorChairComponent : LogicComponent
    {

        RepulsorCoil repulsorCoil;
        RepulsorLift repulsorLift;
        RepulsorLinearGenerator repulsorLinearGenerator;
        RepulsorAngularGenerator repulsorAngularGenerator;
        LinearInertialCompensator linearInertialCompensator;
        AngularInertialCompensator angularInertialCompensator;
        PlanetSurfaceNormalSensor planetSurfaceNormalSensor;
        GyroStabiliser gyroStabiliser;
        AttitudeStabiliser attitudeStabiliser;
        PlanetAltitudeSensor planetAltitudeSensor;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            repulsorCoil = new RepulsorCoil(this);
            AddEquipment(repulsorCoil);

            repulsorLift = new RepulsorLift(this);
            AddEquipment(repulsorLift);

            repulsorLinearGenerator = new RepulsorLinearGenerator(this);
            AddEquipment(repulsorLinearGenerator);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this);
            AddEquipment(repulsorAngularGenerator);

            linearInertialCompensator = new LinearInertialCompensator(this);
            AddEquipment(linearInertialCompensator);

            angularInertialCompensator = new AngularInertialCompensator(this);
            AddEquipment(angularInertialCompensator);

            planetSurfaceNormalSensor = new PlanetSurfaceNormalSensor(this);
            AddEquipment(planetSurfaceNormalSensor);

            gyroStabiliser = new GyroStabiliser(this);
            AddEquipment(gyroStabiliser);

            attitudeStabiliser = new AttitudeStabiliser(this);
            AddEquipment(attitudeStabiliser);

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddEquipment(planetAltitudeSensor);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(repulsorCoil);
            repulsorCoil = null;

            RemoveEquipment(repulsorLift);
            repulsorLift = null;

            RemoveEquipment(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            RemoveEquipment(repulsorAngularGenerator);
            repulsorAngularGenerator = null;

            RemoveEquipment(linearInertialCompensator);
            linearInertialCompensator = null;

            RemoveEquipment(angularInertialCompensator);
            angularInertialCompensator = null;

            RemoveEquipment(planetSurfaceNormalSensor);
            planetSurfaceNormalSensor = null;

            RemoveEquipment(gyroStabiliser);
            gyroStabiliser = null;

            RemoveEquipment(attitudeStabiliser);
            attitudeStabiliser = null;
        }


        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Chair Component");

            base.AppendCustomInfo(block, customInfo);

            IMyCubeGrid grid = block.CubeGrid;
            float forwardSize = (float)grid.WorldAABB.Extents.Z;
            float sideSize = (float)grid.WorldAABB.Extents.X;

            customInfo.AppendLine("Forward Size : " + Math.Round(forwardSize, 2));
            customInfo.AppendLine("Side Size : " + Math.Round(sideSize, 1));
        }

        public override void UpdateAfterSimulation10()
        {
            base.UpdateAfterSimulation10();
        }

        Vector3D liftAcceleration;

        Vector3D linearAcceleration;
        Vector3D angularAcceleration;
        Vector3D stabiliserAngularAcceleration;

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);
            planetSurfaceNormalSensor.UpdateSensor(physicsDeltaTime);

            IMyCubeGrid grid = block.CubeGrid;

            liftAcceleration = Vector3D.Zero;

            linearAcceleration = Vector3D.Zero;
            angularAcceleration = Vector3D.Zero;
            stabiliserAngularAcceleration = Vector3D.Zero;

            // gyro stabiliser

            Vector3D desiredUp = gyroStabiliser.GetDesiredUp(planetSurfaceNormalSensor.surfaceNormal);

            // lift

            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length()) / 2;
            liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);

            // generator

            linearAcceleration = repulsorLinearGenerator.GetLinearAcceleration(physicsDeltaTime);
            rigidbody.AddLinearAcceleration(linearAcceleration);

            angularAcceleration = repulsorAngularGenerator.GetAngularAcceleration(physicsDeltaTime);

            // attitude stabiliser

            Vector3D currentUp = block.WorldMatrix.Up;
            Vector3D currentForward = block.WorldMatrix.Forward;

            Vector3D desiredForward = block.WorldMatrix.Forward;
            angularAcceleration += attitudeStabiliser.GetAngularAcceleration(physicsDeltaTime, currentForward, currentUp, desiredForward, desiredUp);

            // linear compensator

            if (linearInertialCompensator.useLinearCompensator)
            {
                Vector3D desiredLinearAcceleration = linearInertialCompensator.GetDesiredLinearAcceleration(physicsDeltaTime);
                rigidbody.AddLinearAcceleration(desiredLinearAcceleration);
            }

            // angular compensator

            if (angularInertialCompensator.useAngularCompensator)
            {
                Vector3D desiredAngularAcceleration = angularInertialCompensator.GetDesiredAngularAcceleration(physicsDeltaTime);
                rigidbody.AddAngularAcceleration(desiredAngularAcceleration);
            }

            /// apply

            rigidbody.AddLinearAcceleration(liftAcceleration);
            rigidbody.AddAngularAcceleration(angularAcceleration);
            rigidbody.AddAngularAcceleration(stabiliserAngularAcceleration);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_chair_working_start", "repulsor_chair_working_loop", "repulsor_chair_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("49b84a0e-e409-4ae9-ac76-aaf6c59fda4e");
        }
    }

}
