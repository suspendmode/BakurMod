using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallBlockRepulsorPointLift", "LargeBlockRepulsorPointLift" })]
    public class RepulsorPointLiftBlock : NonStaticBakurBlock {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorPointLift", "LargeBlockRepulsorPointLift" };

        RepulsorPointLift repulsorPointLift;
        AltitudeSensor altitudeSensor;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();

            altitudeSensor = new AltitudeSensor(this);
            Add(altitudeSensor);

            repulsorPointLift = new RepulsorPointLift(this);
            Add(repulsorPointLift);

        }

        protected override void Destroy() {

            base.Destroy();

            Remove(repulsorPointLift);
            repulsorPointLift = null;

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

            altitudeSensor.UpdateSensor();            

            IMyCubeGrid grid = block.CubeGrid;

            Vector3D desiredForce = Vector3D.Zero;
            Vector3D desiredUp = block.WorldMatrix.Up;

            // lift

            desiredForce = repulsorPointLift.GetForce(physicsDeltaTime, desiredUp, altitudeSensor.altitude, 100);
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