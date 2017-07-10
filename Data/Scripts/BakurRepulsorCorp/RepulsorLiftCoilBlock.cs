﻿using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsorLiftCoil", "LargeBlockRepulsorLiftCoil" })]
    public class RepulsorLiftCoilBlock : NonStaticBakurBlock {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorLiftCoil", "LargeBlockRepulsorLiftCoil" };

        RepulsorCoil repulsorCoil;
        RepulsorLift repulsorLift;
        PlanetAltitudeSensor altitudeSensor;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();

            repulsorCoil = new RepulsorCoil(this);
            Add(repulsorCoil);

            altitudeSensor = new PlanetAltitudeSensor(this);
            Add(altitudeSensor);

            repulsorLift = new RepulsorLift(this);
            Add(repulsorLift);

        }

        protected override void Destroy() {


            base.Destroy();

            Remove(repulsorCoil);
            repulsorCoil = null;

            Remove(repulsorLift);
            repulsorLift = null;

            Remove(altitudeSensor);
            altitudeSensor = null;
        }

        #endregion
        
        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Lift Coil Block ==");
            customInfo.AppendLine("Lift Force : " + Math.Round(liftForce.Length(), 2));
            base.AppendCustomInfo(block, customInfo);
        }

        
        Vector3D liftForce = Vector3D.Zero;

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            altitudeSensor.UpdateSensor(physicsDeltaTime);
            
            liftForce = Vector3D.Zero;

            if (!IsInGravity) {
                return;
            }

            IMyCubeGrid grid = block.CubeGrid;

            Vector3D desiredUp = gravityUp;

            // lift
            liftForce = repulsorLift.GetTension(physicsDeltaTime, desiredUp, altitudeSensor.altitude, altitudeSensor.precisionMode, altitudeSensor.nearestPlanet.AtmosphereRadius);
            AddForce(liftForce);
        }


        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_lift_coil_working_start", "repulsor_lift_coil_working_loop", "repulsor_lift_coil_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("e2df91e5-e7c3-447c-a1e0-dd2da257ed06");
        }
    }

}