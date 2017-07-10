﻿using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsorGenerator", "LargeBlockRepulsorGenerator" })]
    public class RepulsorGeneratorBlock : NonStaticBakurBlock {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorGenerator", "LargeBlockRepulsorGenerator" };

        public RepulsorLinearGenerator repulsorLinearGenerator;
        public RepulsorAngularGenerator repulsorAngularGenerator;

        double maxLinearAcceleration = 1;
        double maxAngularAcceleration = 25;

        protected override void Initialize() {

            base.Initialize();

            repulsorLinearGenerator = new RepulsorLinearGenerator(this, maxLinearAcceleration);
            Add(repulsorLinearGenerator);

            repulsorAngularGenerator = new RepulsorAngularGenerator(this, maxAngularAcceleration);
            Add(repulsorAngularGenerator);
        }

        protected override void Destroy() {

            base.Destroy();

            Remove(repulsorLinearGenerator);
            repulsorLinearGenerator = null;

            Remove(repulsorAngularGenerator);
            repulsorAngularGenerator = null;
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Generator Block ==");
            base.AppendCustomInfo(block, customInfo);
        }

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            IMyCubeGrid grid = block.CubeGrid;


            Vector3D force = Vector3D.Zero;
            Vector3D forcePoint = Vector3D.Zero;

            Vector3D torque = Vector3D.Zero;

            // generator

            force = repulsorLinearGenerator.GetForce(physicsDeltaTime) * maxLinearAcceleration;
            force = Vector3D.ClampToSphere(force, maxLinearAcceleration);
            AddLinearAcceleration(force);

            torque = repulsorAngularGenerator.GetDesiredAngularAcceleration(physicsDeltaTime) * maxAngularAcceleration;
            torque = Vector3D.ClampToSphere(torque, maxAngularAcceleration);
            AddAngularAcceleration(torque);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_generator_working_start", "repulsor_generator_working_loop", "repulsor_generator_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("952f402f-12f4-4828-892e-1918f015629e");
        }
    }

}
