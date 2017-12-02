using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallAntiGravityGenerator", "LargeAntiGravityGenerator" })]
    public class AntiGravityGeneratorComponent : LogicComponent
    {
        public DefaultUIController<IMyUpgradeModule> defaultUI;

        public AntiGravityGenerator antiGravityGenerator;
        public AntiGravityGeneratorUIController<IMyUpgradeModule> antiGravityGeneratorUI;

        protected override void Initialize()
        {
            base.Initialize();

            defaultUI = new DefaultUIController<IMyUpgradeModule>(this);
            AddElement(defaultUI);

            antiGravityGenerator = new AntiGravityGenerator(this);
            AddElement(antiGravityGenerator);

            antiGravityGeneratorUI = new AntiGravityGeneratorUIController<IMyUpgradeModule>(this);
            AddElement(antiGravityGeneratorUI);

            /*SetPowerRequirements(block, () => {
                return repulsorCoil.PowerRequirements();
            });*/
        }

        protected override void Destroy()
        {
            base.Destroy();

            RemoveElement(antiGravityGenerator);
            antiGravityGenerator = null;

            RemoveElement(antiGravityGeneratorUI);
            antiGravityGeneratorUI = null;
        }

        protected override void Debug()
        {
            if (debugEnabled)
            {
                IMyCubeGrid grid = block.CubeGrid;
                DebugDraw.DrawLine(block.GetPosition(), block.GetPosition() + rigidbody.gravityUp * rigidbody.gravity.Length(), Color.OrangeRed, 0.1f);
            }
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Anti Gravity Generator Component");
            customInfo.AppendLine("Use: " + (antiGravityGenerator.useGenerator ? "On" : "Off"));
            base.AppendCustomInfo(block, customInfo);
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
                return new string[] { "antigravity_generator_working_start", "antigravity_generator_working_loop", "antigravity_generator_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("4f9558ad-e935-4d22-8f58-c72de5916d43");
        }

        protected override void UpdateAfterSimulation(double physicsDeltaTime)
        {

        }
    }

}
