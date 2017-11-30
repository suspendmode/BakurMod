using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "LargeBlockRepulsorCoil", "LargeBlockRepulsorCoil" })]
    public class AntiGravityGeneratorComponent : LogicComponent
    {
        AntiGravityGenerator antiGravityGenerator;

        protected override void Initialize()
        {
            base.Initialize();

            antiGravityGenerator = new AntiGravityGenerator(this);
            AddEquipment(antiGravityGenerator);

            /*SetPowerRequirements(block, () => {
                return repulsorCoil.PowerRequirements();
            });*/
        }

        protected override void Destroy()
        {
            base.Destroy();

            RemoveEquipment(antiGravityGenerator);
            antiGravityGenerator = null;

        }

        Vector3D generatorAcceleration;

        protected override void UpdateSimulation(double physicsDeltaTime)
        {
            generatorAcceleration = Vector3D.Zero;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            // generator

            generatorAcceleration = antiGravityGenerator.GetLinearAcceleration();

            // apply

            rigidbody.AddLinearAcceleration(generatorAcceleration);
        }

        protected override void Debug()
        {
            if (debugEnabled)
            {
                IMyCubeGrid grid = block.CubeGrid;
                DebugDraw.DrawLine(block.GetPosition(), block.GetPosition() + rigidbody.gravityUp * rigidbody.gravity.Length(), Color.DeepSkyBlue, 0.1f);
            }
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Anti Gravity Generator Component");
            customInfo.AppendLine("Use: " + (antiGravityGenerator.useGenerator ? "On" : "Off"));
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
