using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace BakurRepulsorCorp
{


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallAntiGravityGenerator", "LargeAntiGravityGenerator" })]
    public class AntiGravityGeneratorBlock : BakurBlock
    {

        AntiGravityGenerator antiGravityGenerator;
        // PoweredEquipment poweredEquipment;

        float requiredPowerInput = 0.1f;

        #region lifecycle

        MyResourceSinkComponent resourceSink;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {

            base.Init(objectBuilder);

            resourceSink = new MyResourceSinkComponent();
            resourceSink.Init(
               MyStringHash.GetOrCompute("Utility"),
                requiredPowerInput, CalculateRequiredPowerInput);
            resourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
            resourceSink.Update();

            block.ResourceSink = resourceSink;
        }

        protected void Receiver_IsPoweredChanged()
        {
            // UpdateIsWorking();
            //resourceSink.Update();

            //UpdateText();
            // UpdateEmissivity();
            //isPowered = resourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
            //suppliedRatio = resourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);
            //MyAPIGateway.Utilities.ShowMessage("Equipment", "Receiver_IsPoweredChanged: " + isPowered);
        }

        protected override void OnIsWorkingChanged(IMyCubeBlock block)
        {
            resourceSink.Update();
            base.OnIsWorkingChanged(block);
        }

        protected override void OnEnabledChanged()
        {
            resourceSink.Update();
            base.OnEnabledChanged();
        }

        protected override void Initialize()
        {

            base.Initialize();


            antiGravityGenerator = new AntiGravityGenerator(this);
            AddEquipment(antiGravityGenerator);

            // poweredEquipment = new PoweredEquipment(this, (float)requiredPowerInput, CalculateRequiredPowerInput);
            // Add(poweredEquipment);

            resourceSink.Update();
        }

        float CalculateRequiredPowerInput()
        {
            if (!enabled || !block.IsWorking)
            {
                return 0;
            }

            //return requiredPowerInput * load * suppliedRatio;
            return requiredPowerInput * load;
        }

        float CalculateOutput()
        {
            if (!enabled || !block.IsWorking)
            {
                return 0;
            }

            //return requiredPowerInput * load * suppliedRatio;
            return CalculateRequiredPowerInput() * suppliedRatio;
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(antiGravityGenerator);
            antiGravityGenerator = null;

            // Remove(poweredEquipment);
            // poweredEquipment = null;

        }

        #endregion

        #region update

        bool isPowered;
        float load = 0;
        float suppliedRatio = 0;

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            if (!enabled || !block.IsWorking || !isPowered)
            {
                return;
            }

            load = antiGravityGenerator.GetOutput(suppliedRatio);
            resourceSink.Update();
            isPowered = block.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
            suppliedRatio = resourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);
            //MyAPIGateway.Utilities.ShowMessage("Power", "isPowered:" + isPowered + ", suppliedRatio: " + suppliedRatio);
        }

        public override void DrawEmissive()
        {

            if (!enabled)
            {
                block.SetEmissiveParts("Emissive1", new Color(0, 0, 0), 1);
            }
            else if (!block.IsWorking)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 0, 0), 1);
            }
            else if (!isPowered)
            {
                block.SetEmissiveParts("Emissive1", new Color(255, 255, 0), 1);
            }
            else
            {
                block.SetEmissiveParts("Emissive1", new Color(0, 255, 0), 1);
            }
        }



        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== AntiGravity Generator ==");
            customInfo.AppendLine("Required Power: " + CalculateRequiredPowerInput());
            customInfo.AppendLine("Output: " + CalculateOutput());
            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "antigravity_generator_working_start", "antigravity_generator_working_loop", "antigravity_generator_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("4f9558ad-e935-4d22-8f58-c72de5916d43");
        }
    }

}
