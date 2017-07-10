﻿using Sandbox.ModAPI;
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

        RepulsorCoil repulsorCoil;
        RepulsorLift repulsorLift;
        PlanetAltitudeSensor altitudeSensor;

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

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            altitudeSensor.UpdateSensor(physicsDeltaTime);

            if (!IsInGravity) {
                return;
            }

            IMyCubeGrid grid = block.CubeGrid;

            Vector3D desiredForce = Vector3D.Zero;
            Vector3D desiredUp = gravityUp;

            // lift

            desiredForce = repulsorLift.GetTension(physicsDeltaTime, desiredUp, altitudeSensor.altitude, altitudeSensor.precisionMode, altitudeSensor.nearestPlanet.AtmosphereRadius);
            AddForce(desiredForce);
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