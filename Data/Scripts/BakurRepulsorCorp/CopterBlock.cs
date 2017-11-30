using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockCopter", "LargeBlockCopter" })]
    public class CopterBlock : BakurBlock
    {

        Copter copter;
        AttitudeStabiliser attitudeStabiliser;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            copter = new Copter(this);
            AddEquipment(copter);

            attitudeStabiliser = new AttitudeStabiliser(this);
            AddEquipment(attitudeStabiliser);
        }

        protected override void Destroy()
        {

            RemoveEquipment(copter);
            copter = null;

            RemoveEquipment(attitudeStabiliser);
            attitudeStabiliser = null;

            base.Destroy();

        }

        #endregion

        protected override void Debug()
        {
            if (debugEnabled)
            {
                Vector3D center = block.WorldAABB.Center;
                float length = block.CubeGrid.LocalAABB.Size.Length() * 10;
                DebugDraw.DrawLine(center, center + rigidbody.gravityUp * length, Color.Green, 0.02f);
                DebugDraw.DrawLine(center, center + copter.desiredUp * length, Color.Red, 0.02f);
                DebugDraw.DrawLine(center, center + block.WorldMatrix.Forward * length, Color.Blue, 0.01f);
            }
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
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

        protected override Guid blockGUID()
        {
            return new Guid("7d747b37-84f0-4f18-872e-a2a1d3c1ceec");
        }

        Vector3D angularAcceleration;

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            angularAcceleration = Vector3D.Zero;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            IMyCubeGrid grid = block.CubeGrid;

            // copter


            // stabiliser            

            Vector3D currentUp = block.WorldMatrix.Up;
            Vector3D currentForward = block.WorldMatrix.Forward;
            Vector3D desiredUp = copter.GetDesiredUp(rigidbody.gravityUp);
            Vector3D desiredForward = block.WorldMatrix.Forward;

            angularAcceleration += attitudeStabiliser.GetAngularAcceleration(physicsDeltaTime, currentForward, currentUp, desiredForward, desiredUp);

            // apply

            rigidbody.AddAngularAcceleration(angularAcceleration);
        }
    }
}
