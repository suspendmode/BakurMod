using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorJammer", "LargeBlockRepulsorJammer" })]
    public class RepulsorJammerComponent : LogicComponent
    {

        private static readonly string[] subTypeIds = new string[] { "SmallBlockRepulsorJammer", "LargeBlockRepulsorJammer" };

        RepulsorJammer repulsorJammer;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            repulsorJammer = new RepulsorJammer(this);
            AddEquipment(repulsorJammer);

        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(repulsorJammer);
            repulsorJammer = null;

        }

        #endregion


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Jammer Component");
            base.AppendCustomInfo(block, customInfo);
        }



        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            if (!rigidbody.IsInGravity)
            {
                return;
            }


        }

        public override void UpdateAfterSimulation10()
        {
            base.UpdateAfterSimulation10();
            repulsorJammer.UpdateJammer();
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_jammer_working_start", "repulsor_jammer_working_loop", "repulsor_jammer_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("b0ee9325-b5a6-48c9-8a24-d1a9752addf7");
        }
    }

}
