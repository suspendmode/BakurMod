using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockGyroStabiliser", "LargeBlockGyroStabiliser" })]
    public class GyroStabiliserBlock : NonStaticBakurBlock {

        PlanetSurfaceNormalSensor planetSurfaceNormalSensor;
        GyroStabiliser gyroStabiliser;
        AttitudeStabiliser attitudeStabiliser;

        double maxAngularAcceleration = 180;

        protected override void Initialize() {

            base.Initialize();

            planetSurfaceNormalSensor = new PlanetSurfaceNormalSensor(this);
            Add(planetSurfaceNormalSensor);

            gyroStabiliser = new GyroStabiliser(this);
            Add(gyroStabiliser);

            attitudeStabiliser = new AttitudeStabiliser(this);
            Add(attitudeStabiliser);
        }

        protected override void Destroy() {

            base.Destroy();

            Remove(planetSurfaceNormalSensor);
            planetSurfaceNormalSensor = null;

            Remove(gyroStabiliser);
            gyroStabiliser = null;

            Remove(attitudeStabiliser);
            attitudeStabiliser = null;
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {

            customInfo.AppendLine();
            customInfo.AppendLine("== Gyro Stabiliser Block ==");

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

        Vector3D angularAcceleration;

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            angularAcceleration = Vector3D.Zero;

            if (!IsInGravity) {
                return;
            }

            planetSurfaceNormalSensor.UpdateSensor(physicsDeltaTime);

            IMyCubeGrid grid = block.CubeGrid;


            // stabiliser            

            Vector3D currentUp = block.WorldMatrix.Up;
            Vector3D desiredUp = gyroStabiliser.GetDesiredUp(planetSurfaceNormalSensor.surfaceNormal);
            angularAcceleration = attitudeStabiliser.GetAngularAcceleration(maxAngularAcceleration, currentUp, desiredUp);
            angularAcceleration = Vector3D.ClampToSphere(angularAcceleration, maxAngularAcceleration);

            // apply

            AddAngularAcceleration(angularAcceleration);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "gyroscopic_stabiliser_working_start", "gyroscopic_stabiliser_working_loop", "gyroscopic_stabiliser_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("130652c2-5ce0-4333-8511-08e69abde757");
        }
    }

}