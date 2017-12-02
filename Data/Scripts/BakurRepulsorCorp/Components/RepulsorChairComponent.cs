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
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Cockpit), true, new string[] { "SmallBlockRepulsorChair", "LargeBlockRepulsorChair" })]
    public class RepulsorChairComponent : LogicComponent
    {
        public DefaultUIController<IMyCockpit> defaultUI;

        RepulsorCoil repulsorCoil;
        RepulsorCoilUIController<IMyCockpit> repulsorCoilUI;

        RepulsorLift repulsorLift;
        RepulsorLiftUIController<IMyCockpit> repulsorLiftUI;

        RepulsorLinearGenerator repulsorLinearGenerator;
        RepulsorLinearGeneratorUIController<IMyCockpit> repulsorLinearGeneratorUI;

        RepulsorAngularGenerator repulsorAngularGenerator;
        RepulsorAngularGeneratorUIController<IMyCockpit> repulsorAngularGeneratorUI;

        LinearInertialCompensator linearInertialCompensator;
        LinearInertialCompensatorUIController<IMyCockpit> linearInertialCompensatorUI;

        AngularInertialCompensator angularInertialCompensator;
        AngularInertialCompensatorUIController<IMyCockpit> angularInertialCompensatorUI;

        PlanetSurfaceNormalSensor planetSurfaceNormalSensor;
        PlanetSurfaceNormalSensorUIController<IMyCockpit> planetSurfaceNormalSensorUI;

        GyroStabiliser gyroStabiliser;
        GyroStabiliserUIController<IMyCockpit> gyroStabiliserUI;

        AttitudeStabiliser attitudeStabiliser;
        AttitudeStabiliserUIController<IMyCockpit> attitudeStabiliserUI;

        PlanetAltitudeSensor planetAltitudeSensor;
        PlanetAltitudeSensorUIController<IMyCockpit> planetAltitudeSensorUI;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyCockpit>(this);
            AddElement(defaultUI);

            repulsorCoil = new RepulsorCoil(this);
            AddElement(repulsorCoil);

            repulsorCoilUI = new RepulsorCoilUIController<IMyCockpit>(this);
            AddElement(repulsorCoilUI);

            repulsorLift = new RepulsorLift(this);
            AddElement(repulsorLift);

            repulsorLiftUI = new RepulsorLiftUIController<IMyCockpit>(this);
            AddElement(repulsorLiftUI);

            repulsorLinearGenerator = new RepulsorLinearGenerator(this);
            AddElement(repulsorLinearGenerator);

            repulsorLinearGeneratorUI = new RepulsorLinearGeneratorUIController<IMyCockpit>(this);
            AddElement(repulsorLinearGeneratorUI);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this);
            AddElement(repulsorAngularGenerator);

            repulsorAngularGeneratorUI = new RepulsorAngularGeneratorUIController<IMyCockpit>(this);
            AddElement(repulsorAngularGeneratorUI);

            linearInertialCompensator = new LinearInertialCompensator(this);
            AddElement(linearInertialCompensator);

            linearInertialCompensatorUI = new LinearInertialCompensatorUIController<IMyCockpit>(this);
            AddElement(linearInertialCompensatorUI);

            angularInertialCompensator = new AngularInertialCompensator(this);
            AddElement(angularInertialCompensator);

            angularInertialCompensatorUI = new AngularInertialCompensatorUIController<IMyCockpit>(this);
            AddElement(angularInertialCompensatorUI);

            planetSurfaceNormalSensor = new PlanetSurfaceNormalSensor(this);
            AddElement(planetSurfaceNormalSensor);

            planetSurfaceNormalSensorUI = new PlanetSurfaceNormalSensorUIController<IMyCockpit>(this);
            AddElement(planetSurfaceNormalSensorUI);

            gyroStabiliser = new GyroStabiliser(this);
            AddElement(gyroStabiliser);

            gyroStabiliserUI = new GyroStabiliserUIController<IMyCockpit>(this);
            AddElement(gyroStabiliserUI);

            attitudeStabiliser = new AttitudeStabiliser(this);
            AddElement(attitudeStabiliser);

            attitudeStabiliserUI = new AttitudeStabiliserUIController<IMyCockpit>(this);
            AddElement(attitudeStabiliserUI);

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddElement(planetAltitudeSensor);

            planetAltitudeSensorUI = new PlanetAltitudeSensorUIController<IMyCockpit>(this);
            AddElement(planetAltitudeSensorUI);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(repulsorCoil);
            repulsorCoil = null;

            RemoveElement(repulsorCoilUI);
            repulsorCoilUI = null;

            RemoveElement(repulsorLift);
            repulsorLift = null;

            RemoveElement(repulsorLiftUI);
            repulsorLiftUI = null;

            RemoveElement(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            RemoveElement(repulsorLinearGeneratorUI);
            repulsorLinearGeneratorUI = null;

            RemoveElement(repulsorAngularGenerator);
            repulsorAngularGenerator = null;

            RemoveElement(repulsorAngularGeneratorUI);
            repulsorAngularGeneratorUI = null;

            RemoveElement(linearInertialCompensator);
            linearInertialCompensator = null;

            RemoveElement(linearInertialCompensatorUI);
            linearInertialCompensatorUI = null;

            RemoveElement(angularInertialCompensator);
            angularInertialCompensator = null;

            RemoveElement(angularInertialCompensatorUI);
            angularInertialCompensatorUI = null;

            RemoveElement(planetSurfaceNormalSensor);
            planetSurfaceNormalSensor = null;

            RemoveElement(planetSurfaceNormalSensorUI);
            planetSurfaceNormalSensorUI = null;

            RemoveElement(gyroStabiliser);
            gyroStabiliser = null;

            RemoveElement(gyroStabiliserUI);
            gyroStabiliserUI = null;

            RemoveElement(attitudeStabiliser);
            attitudeStabiliser = null;

            RemoveElement(attitudeStabiliserUI);
            attitudeStabiliserUI = null;
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

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
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

            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length());
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
        public override void DrawEmissive()
        {
            if (block.CubeGrid.IsStatic)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 120, 0), 1);
            }
            else if (!block.IsWorking || !block.IsFunctional || !enabled || !rigidbody.IsInGravity)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 0, 0), 1);
            }
            else
            {
                block.SetEmissiveParts("Emissive1", new Color(0, 255, 0), 1);
            }
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
