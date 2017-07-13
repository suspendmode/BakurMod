using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsorLiftGenerator", "LargeBlockRepulsorLiftGenerator" })]
    public class RepulsorLiftGeneratorBlock : NonStaticBakurBlock {

        PlanetAltitudeSensor altitudeSensor;
        RepulsorLift repulsorLift;
        RepulsorLinearGenerator repulsorLinearGenerator;
        RepulsorAngularGenerator repulsorAngularGenerator;

        double maxLinearAcceleration = 0.5;
        double maxAngularAcceleration = 25;

        protected override void Initialize() {

            base.Initialize();

            altitudeSensor = new PlanetAltitudeSensor(this);
            Add(altitudeSensor);

            repulsorLift = new RepulsorLift(this);
            Add(repulsorLift);

            repulsorLinearGenerator = new RepulsorLinearGenerator(this, maxLinearAcceleration);
            Add(repulsorLinearGenerator);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this, maxAngularAcceleration);
            Add(repulsorAngularGenerator);
        }

        protected override void Destroy() {

            base.Destroy();

            Remove(repulsorLift);
            repulsorLift = null;

            Remove(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            Remove(repulsorAngularGenerator);
            repulsorAngularGenerator = null;

            Remove(altitudeSensor);
            altitudeSensor = null;
        }

        Vector3D linearAcceleration;
        Vector3D angularAcceleration;
        Vector3D liftAcceleration;

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            altitudeSensor.UpdateSensor(physicsDeltaTime);

            IMyCubeGrid grid = block.CubeGrid;
            Vector3D desiredUp = gravityUp;

            linearAcceleration = Vector3D.Zero;
            angularAcceleration = Vector3D.Zero;
            liftAcceleration = Vector3D.Zero;

            // generator

            linearAcceleration = repulsorLinearGenerator.GetLinearAcceleration(physicsDeltaTime);
            angularAcceleration = repulsorAngularGenerator.GetAngularAcceleration(physicsDeltaTime);

            // lift

            if (IsInGravity) {
                liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, altitudeSensor.altitude, altitudeSensor.nearestPlanet.AtmosphereAltitude / 10);
            }

            // apply

            AddLinearAcceleration(liftAcceleration);
            AddLinearAcceleration(linearAcceleration);
            AddAngularAcceleration(angularAcceleration);
        }


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Lift Generator Block ==");
            base.AppendCustomInfo(block, customInfo);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_lift_generator_working_start", "repulsor_lift_generator_working_loop", "repulsor_lift_generator_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("a0498045-505d-4691-bb0c-00c515b01793");
        }
    }

}