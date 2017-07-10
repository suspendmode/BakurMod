﻿using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsor", "LargeBlockRepulsor" })]
    public class RepulsorSuspensionBlock : NonStaticBakurBlock {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsor", "LargeBlockRepulsor" };

        AltitudeSensor surfaceSensor;
        RepulsorSuspension repulsorSuspension;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();

            surfaceSensor = new AltitudeSensor(this);
            Add(surfaceSensor);

            repulsorSuspension = new RepulsorSuspension(this);
            Add(repulsorSuspension);
        }

        protected override void Destroy() {

            base.Destroy();

            Remove(surfaceSensor);
            surfaceSensor = null;

            Remove(repulsorSuspension);
            repulsorSuspension = null;
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_working_start", "repulsor_working_loop", "repulsor_working_end" };
            }
        }
        #endregion

        #region visual

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Block ==");

            base.AppendCustomInfo(block, customInfo);

        }

        #endregion

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            surfaceSensor.UpdateSensor();

            if (!surfaceSensor.hasSurface) {
                return;
            }

            IMyCubeGrid grid = block.CubeGrid;

            Vector3D suspensionForce = Vector3D.Zero;
            Vector3D desiredUp = block.WorldMatrix.Up;

            // lift

            suspensionForce = repulsorSuspension.GetForce(physicsDeltaTime, desiredUp, surfaceSensor.altitude);
            Vector3D position = block.PositionComp.GetPosition() + block.LocalVolume.Center;
            AddForce(suspensionForce, position);
        }

        protected override void Debug() {
            if (surfaceSensor.hasSurface && debugEnabled) {
                DebugDraw.DrawLine(block.GetPosition(), surfaceSensor.surfacePoint, Color.Aquamarine, 0.03f);
            }
        }

        protected override Guid blockGUID() {
            return new Guid("fe299648-2f06-4a7e-9a8d-54d3ee991e23");
        }
    }
}
