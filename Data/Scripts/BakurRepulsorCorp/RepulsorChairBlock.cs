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

            repulsorLift = new RepulsorLift(this, maxLinearAcceleration);
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

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            if (!IsInGravity) {
                return;
            }
            /*
            altitudeSensor.UpdateSensor(physicsDeltaTime);
            planetSurfaceNormalSensor.UpdateSensor(physicsDeltaTime);

            IMyCubeGrid grid = block.CubeGrid;

            Vector3D liftForce = Vector3D.Zero;

            Vector3D desiredLinearAcceleration = Vector3D.Zero;
            Vector3D desiredAngularAcceleration = Vector3D.Zero;

            // gyro stabiliser

            Vector3D desiredUp = gyroStabiliser.GetDesiredUp(planetSurfaceNormalSensor.surfaceNormal);

            // lift

            liftForce = repulsorLift.GetTension(physicsDeltaTime, desiredUp, altitudeSensor.altitude, altitudeSensor.precisionMode, altitudeSensor.nearestPlanet.AtmosphereRadius);
            AddForce(liftForce);

            // generator

            desiredLinearAcceleration = repulsorDrive.GetDesiredLinearAcceleration(maxLinearAcceleration);
            desiredLinearAcceleration = Vector3D.ClampToSphere(desiredLinearAcceleration, maxLinearAcceleration);
            AddLinearAcceleration(desiredLinearAcceleration);

            desiredAngularAcceleration = repulsorDrive.GetDesiredAngularAcceleration(maxAngularAcceleration);
            desiredAngularAcceleration = Vector3D.ClampToSphere(desiredAngularAcceleration, maxAngularAcceleration);
            AddAngularAcceleration(desiredAngularAcceleration);

            // attitude stabiliser

            Vector3D currentUp = block.WorldMatrix.Up;
            desiredAngularAcceleration = attitudeStabiliser.GetDesiredAngularAcceleration(maxAngularAcceleration, currentUp, desiredUp);
            desiredAngularAcceleration = Vector3D.ClampToSphere(desiredAngularAcceleration, maxAngularAcceleration);
            AddAngularAcceleration(desiredAngularAcceleration);
            */
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