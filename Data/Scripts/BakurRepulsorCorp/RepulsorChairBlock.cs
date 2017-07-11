using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Cockpit), true, new string[] { "SmallBlockRepulsorChair", "LargeBlockRepulsorChair" })]
    public class RepulsorChairBlock : NonStaticBakurBlock {

        RepulsorCoil repulsorCoil;
        RepulsorLift repulsorLift;
        RepulsorLinearGenerator repulsorLinearGenerator;
        RepulsorAngularGenerator repulsorAngularGenerator;
        PlanetSurfaceNormalSensor planetSurfaceNormalSensor;
        GyroStabiliser gyroStabiliser;
        AttitudeStabiliser attitudeStabiliser;
        PlanetAltitudeSensor altitudeSensor;

        double maxLinearAcceleration = 0.5;
        double maxAngularAcceleration = 90;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();

            repulsorCoil = new RepulsorCoil(this);
            Add(repulsorCoil);

            repulsorLift = new RepulsorLift(this);
            Add(repulsorLift);

            repulsorLinearGenerator = new RepulsorLinearGenerator(this, maxLinearAcceleration);
            Add(repulsorLinearGenerator);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this, maxAngularAcceleration);
            Add(repulsorAngularGenerator);

            planetSurfaceNormalSensor = new PlanetSurfaceNormalSensor(this);
            Add(planetSurfaceNormalSensor);

            gyroStabiliser = new GyroStabiliser(this);
            Add(gyroStabiliser);

            attitudeStabiliser = new AttitudeStabiliser(this);
            Add(attitudeStabiliser);

            altitudeSensor = new PlanetAltitudeSensor(this);
            Add(altitudeSensor);

        }

        protected override void Destroy() {

            base.Destroy();

            Remove(repulsorCoil);
            repulsorCoil = null;

            Remove(repulsorLift);
            repulsorLift = null;

            Remove(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            Remove(repulsorAngularGenerator);
            repulsorAngularGenerator = null;

            Add(planetSurfaceNormalSensor);
            Remove(planetSurfaceNormalSensor);
            planetSurfaceNormalSensor = null;

            Remove(gyroStabiliser);
            gyroStabiliser = null;

            Remove(attitudeStabiliser);
            attitudeStabiliser = null;
        }


        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Chair Block ==");

            base.AppendCustomInfo(block, customInfo);

            IMyCubeGrid grid = block.CubeGrid;
            float forwardSize = (float)grid.WorldAABB.Extents.Z;
            float sideSize = (float)grid.WorldAABB.Extents.X;

            customInfo.AppendLine("Forward Size : " + Math.Round(forwardSize, 2));
            customInfo.AppendLine("Side Size : " + Math.Round(sideSize, 1));
        }

        public override void UpdateAfterSimulation10() {
            base.UpdateAfterSimulation10();
        }

        Vector3D liftAcceleration;

        Vector3D linearAcceleration;
        Vector3D angularAcceleration;
        Vector3D stabiliserAngularAcceleration;
        Vector3D desiredUp;

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            if (!IsInGravity) {
                return;
            }

            altitudeSensor.UpdateSensor(physicsDeltaTime);
            planetSurfaceNormalSensor.UpdateSensor(physicsDeltaTime);

            IMyCubeGrid grid = block.CubeGrid;

            liftAcceleration = Vector3D.Zero;

            linearAcceleration = Vector3D.Zero;
            angularAcceleration = Vector3D.Zero;
            stabiliserAngularAcceleration = Vector3D.Zero;

            // gyro stabiliser

            Vector3D desiredUp = gyroStabiliser.GetDesiredUp(planetSurfaceNormalSensor.surfaceNormal);

            // lift

            liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, altitudeSensor.altitude, altitudeSensor.nearestPlanet.AtmosphereRadius);

            // generator

            linearAcceleration = repulsorLinearGenerator.GetLinearAcceleration(physicsDeltaTime, maxLinearAcceleration);
            linearAcceleration = Vector3D.ClampToSphere(linearAcceleration, maxLinearAcceleration);
            AddLinearAcceleration(linearAcceleration);

            angularAcceleration = repulsorAngularGenerator.GetAngularAcceleration(physicsDeltaTime, maxAngularAcceleration);
            angularAcceleration = Vector3D.ClampToSphere(angularAcceleration, maxAngularAcceleration);

            // attitude stabiliser

            Vector3D currentUp = block.WorldMatrix.Up;
            stabiliserAngularAcceleration = attitudeStabiliser.GetAngularAcceleration(maxAngularAcceleration, currentUp, desiredUp);
            stabiliserAngularAcceleration = Vector3D.ClampToSphere(stabiliserAngularAcceleration, maxAngularAcceleration);

            /// apply

            AddLinearAcceleration(liftAcceleration);
            AddAngularAcceleration(angularAcceleration);
            AddAngularAcceleration(stabiliserAngularAcceleration);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_chair_working_start", "repulsor_chair_working_loop", "repulsor_chair_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("49b84a0e-e409-4ae9-ac76-aaf6c59fda4e");
        }
    }

}