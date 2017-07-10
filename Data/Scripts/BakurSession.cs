using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;

namespace BakurRepulsorCorp {
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public abstract class BakurSession : MySessionComponentBase {

        public bool initialized;

        protected override void UnloadData() {
            initialized = false;
            //MyAPIGateway.Utilities.ShowMessage("RepulsorCoilSession", "UnloadData");
        }

        public abstract void Initialize();
        public abstract void Destroy();
        public abstract void Update(double physicsDeltaTime);

        public override void UpdateBeforeSimulation() {

            if (!initialized) {
                if (MyAPIGateway.Session == null)
                    return;
                initialized = true;
                //MyAPIGateway.Utilities.ShowMessage("RepulsorCoilSession", "Init");
                Initialize();
            }

            if (!initialized) {
                return;
            }

            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;

            Update(physicsDeltaTime);
        }
    }

}