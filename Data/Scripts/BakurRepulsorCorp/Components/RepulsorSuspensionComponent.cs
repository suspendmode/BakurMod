﻿using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsor", "LargeBlockRepulsor" })]
    public class RepulsorSuspensionComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsor", "LargeBlockRepulsor" };

        AltitudeSensor surfaceSensor;
        AltitudeSensorUIController<IMyUpgradeModule> surfaceSensorUI;

        RepulsorSuspension repulsorSuspension;
        RepulsorSuspensionUIController<IMyUpgradeModule> repulsorSuspensionUI;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            surfaceSensor = new AltitudeSensor(this);
            AddElement(surfaceSensor);

            surfaceSensorUI = new AltitudeSensorUIController<IMyUpgradeModule>(this);
            AddElement(surfaceSensorUI);

            repulsorSuspension = new RepulsorSuspension(this);
            AddElement(repulsorSuspension);

            repulsorSuspensionUI = new RepulsorSuspensionUIController<IMyUpgradeModule>(this);
            AddElement(repulsorSuspensionUI);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(surfaceSensor);
            surfaceSensor = null;

            RemoveElement(surfaceSensorUI);
            surfaceSensorUI = null;

            RemoveElement(repulsorSuspension);
            repulsorSuspension = null;

            RemoveElement(repulsorSuspensionUI);
            repulsorSuspensionUI = null;
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

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Component");

            base.AppendCustomInfo(block, customInfo);

        }

        #endregion

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {

            surfaceSensor.UpdateSensor();

            if (!surfaceSensor.hasSurface)
            {
                return;
            }

            IMyCubeGrid grid = block.CubeGrid;

            Vector3D suspensionForce = Vector3D.Zero;
            Vector3D desiredUp = block.WorldMatrix.Up;

            // lift

            double gridHalfSize = block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5;
            suspensionForce = repulsorSuspension.GetForce(physicsDeltaTime, desiredUp, surfaceSensor.altitude - gridHalfSize);
            Vector3D position = block.PositionComp.GetPosition() + block.LocalVolume.Center;
            rigidbody.AddForce(suspensionForce, position);
        }

        protected override void Debug()
        {
            if (surfaceSensor.hasSurface && debugEnabled)
            {
                DebugDraw.DrawLine(block.GetPosition(), surfaceSensor.surfacePoint, Color.Aquamarine, 0.03f);
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("fe299648-2f06-4a7e-9a8d-54d3ee991e23");
        }
    }
}
