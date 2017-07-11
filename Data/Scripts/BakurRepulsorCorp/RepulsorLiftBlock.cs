using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsorLift", "LargeBlockRepulsorLift" })]
    public class RepulsorLiftBlock : NonStaticBakurBlock {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorLift", "LargeBlockRepulsorLift" };

        PlanetAltitudeSensor altitudeSensor;
        RepulsorLift repulsorLift;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();

            altitudeSensor = new PlanetAltitudeSensor(this);
            Add(altitudeSensor);

            repulsorLift = new RepulsorLift(this);
            Add(repulsorLift);

        }

        protected override void Destroy() {

            base.Destroy();

            Remove(repulsorLift);
            repulsorLift = null;

            Remove(altitudeSensor);
            altitudeSensor = null;
        }

        #endregion


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Lift Block ==");
            base.AppendCustomInfo(block, customInfo);
        }

        Vector3D liftAcceleration = Vector3D.Zero;
        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            liftAcceleration = Vector3D.Zero;

            altitudeSensor.UpdateSensor(physicsDeltaTime);

            if (!IsInGravity) {
                return;
            }

            // lift

            liftAcceleration = repulsorLift.GetLinearAcceleration(physicsDeltaTime, altitudeSensor.altitude, altitudeSensor.nearestPlanet.HasAtmosphere ? altitudeSensor.nearestPlanet.AtmosphereAltitude : altitudeSensor.nearestPlanet.MaximumRadius);

            // apply

            AddLinearAcceleration(liftAcceleration);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_lift_working_start", "repulsor_lift_working_loop", "repulsor_lift_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("3a95a757-3c62-4d4a-a88e-7fa2a2835922");
        }
    }

}