using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockGyroStabiliser", "LargeBlockGyroStabiliser" })]
    public class GyroStabiliserComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        PlanetSurfaceNormalSensor planetSurfaceNormalSensor;
        PlanetSurfaceNormalSensorUIController<IMyUpgradeModule> planetSurfaceNormalSensorUI;

        GyroStabiliser gyroStabiliser;
        GyroStabiliserUIController<IMyUpgradeModule> gyroStabiliserUI;

        AttitudeStabiliser attitudeStabiliser;
        AttitudeStabiliserUIController<IMyUpgradeModule> attitudeStabiliserUI;

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            planetSurfaceNormalSensor = new PlanetSurfaceNormalSensor(this);
            AddElement(planetSurfaceNormalSensor);

            planetSurfaceNormalSensorUI = new PlanetSurfaceNormalSensorUIController<IMyUpgradeModule>(this);
            AddElement(planetSurfaceNormalSensorUI);

            gyroStabiliser = new GyroStabiliser(this);
            AddElement(gyroStabiliser);

            gyroStabiliserUI = new GyroStabiliserUIController<IMyUpgradeModule>(this);
            AddElement(gyroStabiliserUI);

            attitudeStabiliser = new AttitudeStabiliser(this);
            AddElement(attitudeStabiliser);

            attitudeStabiliserUI = new AttitudeStabiliserUIController<IMyUpgradeModule>(this);
            AddElement(attitudeStabiliserUI);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(planetSurfaceNormalSensor);
            planetSurfaceNormalSensor = null;

            RemoveElement(planetSurfaceNormalSensorUI);
            planetSurfaceNormalSensorUI = null;

            RemoveElement(gyroStabiliser);
            gyroStabiliser = null;

            RemoveElement(gyroStabiliserUI);
            gyroStabiliserUI = null;

            RemoveElement(attitudeStabiliser);
            attitudeStabiliser = null;

            RemoveElement(attitudeStabiliserUI);
            attitudeStabiliserUI = null;
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {

            customInfo.AppendLine();
            customInfo.AppendLine("Type: Gyro Stabiliser Component");

            base.AppendCustomInfo(block, customInfo);
            IMyCubeGrid grid = block.CubeGrid;
            float forwardSize = (float)grid.WorldAABB.Extents.Z;
            float sideSize = (float)grid.WorldAABB.Extents.X;

            customInfo.AppendLine("Forward Size : " + Math.Round(forwardSize, 2));
            customInfo.AppendLine("Side Size : " + Math.Round(sideSize, 1));
        }

        public override void UpdateAfterSimulation10()
        {
            base.UpdateAfterSimulation10();

        }

        Vector3D angularAcceleration;

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {

            angularAcceleration = Vector3D.Zero;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            planetSurfaceNormalSensor.UpdateSensor(physicsDeltaTime);

            IMyCubeGrid grid = block.CubeGrid;


            // stabiliser            

            Vector3D currentUp = block.WorldMatrix.Up;
            Vector3D currentForward = block.WorldMatrix.Forward;
            Vector3D desiredUp = gyroStabiliser.GetDesiredUp(planetSurfaceNormalSensor.surfaceNormal);
            Vector3D desiredForward = block.WorldMatrix.Forward;
            angularAcceleration += attitudeStabiliser.GetAngularAcceleration(physicsDeltaTime, currentForward, currentUp, desiredForward, desiredUp);

            // apply

            rigidbody.AddAngularAcceleration(angularAcceleration);
        }

        public override void DrawEmissive()
        {
            if (block.CubeGrid.IsStatic)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 120, 0), 1);
            }
            else if (!block.IsWorking || !block.IsFunctional || !enabled || !rigidbody.IsInGravity)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 0, 0), 1);
            }
            else
            {
                block.SetEmissiveParts("Emissive1", new Color(0, 255, 0), 1);
            }
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "gyroscopic_stabiliser_working_start", "gyroscopic_stabiliser_working_loop", "gyroscopic_stabiliser_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("130652c2-5ce0-4333-8511-08e69abde757");
        }
    }

}
