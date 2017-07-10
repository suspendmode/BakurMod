using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockCopter", "LargeBlockCopter" })]
    public class CopterBlock : NonStaticBakurBlock {

        Copter copter;
        AttitudeStabiliser attitudeStabiliser;

        public float maxAngularAcceleration = 180;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();


            copter = new Copter(this);
            Add(copter);

            attitudeStabiliser = new AttitudeStabiliser(this);
            Add(attitudeStabiliser);
        }

        protected override void Destroy() {

            Remove(copter);
            copter = null;

            Remove(attitudeStabiliser);
            attitudeStabiliser = null;

            base.Destroy();

        }

        #endregion

        protected override void Debug() {
            if (debugEnabled) {
                Vector3D center = block.WorldAABB.Center;
                float length = block.CubeGrid.LocalAABB.Size.Length() * 10;
                DebugDraw.DrawLine(center, center + gravityUp * length, Color.Green, 0.02f);
                DebugDraw.DrawLine(center, center + copter.desiredUp * length, Color.Red, 0.02f);
                DebugDraw.DrawLine(center, center + block.WorldMatrix.Forward * length, Color.Blue, 0.01f);
            }
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Copter Block ==");
            base.AppendCustomInfo(block, customInfo);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "copter_working_start", "copter_working_loop", "copter_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("7d747b37-84f0-4f18-872e-a2a1d3c1ceec");
        }

        #region acceleration

        Vector3D desiredAngularAcceleration;

        public Vector3D GetAngularAcceleration(double physicsDeltaTime) {

            desiredAngularAcceleration = Vector3D.Zero;

            if (!IsInGravity) {
                return desiredAngularAcceleration;
            }

            IMyCubeGrid grid = block.CubeGrid;

            // copter

            Vector3 desiredUp = copter.GetDesiredUp(gravityUp);

            // stabiliser            

            Vector3D currentUp = block.WorldMatrix.Up;
            desiredAngularAcceleration = attitudeStabiliser.GetDesiredAngularAcceleration(maxAngularAcceleration, currentUp, desiredUp);

            desiredAngularAcceleration = Vector3D.ClampToSphere(desiredAngularAcceleration, maxAngularAcceleration);
            return desiredAngularAcceleration;
        }

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {
            throw new NotImplementedException();
        }

        #endregion

    }
}
