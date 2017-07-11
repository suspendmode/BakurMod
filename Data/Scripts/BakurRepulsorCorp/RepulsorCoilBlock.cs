using Sandbox.ModAPI;
using System;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsorCoil", "LargeBlockRepulsorCoil" })]
    public class RepulsorCoilBlock : NonStaticBakurBlock {

        RepulsorCoil repulsorCoil;

        protected override void Initialize() {

            base.Initialize();

            repulsorCoil = new RepulsorCoil(this);
            Add(repulsorCoil);
        }

        protected override void Destroy() {

            base.Destroy();

            Remove(repulsorCoil);
            repulsorCoil = null;
        }

        Vector3D coilAcceleration;

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            coilAcceleration = Vector3D.Zero;

            if (!IsInGravity) {
                return;
            }

            // coil

            coilAcceleration = repulsorCoil.GetLinearAcceleration(physicsDeltaTime);

            // apply

            AddLinearAcceleration(coilAcceleration);
        }

        protected override void Debug() {
            if (debugEnabled) {
                IMyCubeGrid grid = block.CubeGrid;
                DebugDraw.DrawLine(block.GetPosition(), block.GetPosition() + gravityUp * gravity.Length(), Color.DeepSkyBlue, 0.1f);
            }
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Coil Block ==");
            customInfo.AppendLine("Use : " + (repulsorCoil.useCoil ? "On" : "Off"));

            base.AppendCustomInfo(block, customInfo);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_coil_working_start", "repulsor_coil_working_loop", "repulsor_coil_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("bf8957e0-500d-4356-9875-91db9dd4a912");
        }

    }

}