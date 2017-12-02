using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorJammer", "LargeBlockRepulsorJammer" })]
    public class RepulsorJammerComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        private static readonly string[] subTypeIds = { "SmallBlockRepulsorJammer", "LargeBlockRepulsorJammer" };

        RepulsorJammer repulsorJammer;
        RepulsorJammerUIController<IMyUpgradeModule> repulsorJammerUI;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            repulsorJammer = new RepulsorJammer(this);
            AddElement(repulsorJammer);

            repulsorJammerUI = new RepulsorJammerUIController<IMyUpgradeModule>(this);
            AddElement(repulsorJammerUI);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveElement(repulsorJammer);
            repulsorJammer = null;

            RemoveElement(repulsorJammerUI);
            repulsorJammerUI = null;

        }

        #endregion


        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Jammer Component");
            base.AppendCustomInfo(block, customInfo);
        }



        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            repulsorJammer.UpdateJammer();
        }

        public override void UpdateAfterSimulation10()
        {
            base.UpdateAfterSimulation10();
            repulsorJammer.UpdateJammer();
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
                return new string[] { "repulsor_jammer_working_start", "repulsor_jammer_working_loop", "repulsor_jammer_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("b0ee9325-b5a6-48c9-8a24-d1a9752addf7");
        }
    }

}
