using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;

namespace BakurRepulsorCorp
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallArtificialGravityGenerator", "LargeArtificialGravityGenerator" })]
    public class ArtificialGravityGeneratorComponent : LogicComponent
    {

        ArtificialGravityGenerator artificialGravityGenerator;

        #region lifecycle

        protected override void Initialize()
        {

            base.Initialize();


            artificialGravityGenerator = new ArtificialGravityGenerator(this);
            AddEquipment(artificialGravityGenerator);
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(artificialGravityGenerator);
            artificialGravityGenerator = null;

        }

        #endregion

        #region update

        protected override void UpdateSimulation(double physicsDeltaTime)
        {

            artificialGravityGenerator.UpdateGenerator(physicsDeltaTime);
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Artificial Gravity Generator Component");

            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "artificial_gravity_generator_working_start", "artificial_gravity_generator_working_loop", "artificial_gravity_generator_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("a3672713-d475-4bd7-ad04-c96ce6b8cc56");
        }
    }

}
