using Sandbox.ModAPI;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;
using System;
using VRage.Utils;
using VRage.Game.ObjectBuilders.Definitions;

namespace BakurRepulsorCorp {


    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), true, new string[] { "SmallArtificialGravityGenerator", "LargeArtificialGravityGenerator" })]
    public class ArtificialGravityGeneratorBlock : BakurBlock {

        ArtificialGravityGenerator artificialGravityGenerator;

        #region lifecycle

        protected override void Initialize() {

            base.Initialize();


            artificialGravityGenerator = new ArtificialGravityGenerator(this);
            Add(artificialGravityGenerator);
        }

        protected override void Destroy() {

            base.Destroy();

            Remove(artificialGravityGenerator);
            artificialGravityGenerator = null;

        }

        #endregion

        #region update

        protected override void UpdateBeforeFrame(double physicsDeltaTime, double updateDeltaTime) {

            artificialGravityGenerator.UpdateGenerator(physicsDeltaTime);
        }

        #endregion

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Artificial Gravity Generator ==");

            base.AppendCustomInfo(block, customInfo);

        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "artificial_gravity_generator_working_start", "artificial_gravity_generator_working_loop", "artificial_gravity_generator_working_end" };
            }
        }

        protected override Guid blockGUID() {
            return new Guid("a3672713-d475-4bd7-ad04-c96ce6b8cc56");
        }
    }

}
