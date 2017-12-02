using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;

namespace BakurRepulsorCorp
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public abstract class SessionComponent : MySessionComponentBase
    {

        public bool initialized;

        public abstract void Initialize();
        public abstract void Destroy();

        public abstract void UpdateBefore(double physicsDeltaTime);
        public abstract void UpdateAfter(double physicsDeltaTime);

        public override void LoadData()
        {
            base.LoadData();

            if (!initialized)
            {
                if (MyAPIGateway.Session == null)
                    return;
                initialized = true;
                //MyAPIGateway.Utilities.ShowMessage("RepulsorCoilSession", "Init");
                Initialize();
            }
        }

        protected override void UnloadData()
        {
            base.UnloadData();

            if (initialized)
            {
                if (MyAPIGateway.Session == null)
                    return;
                initialized = false;
                //MyAPIGateway.Utilities.ShowMessage("RepulsorCoilSession", "Init");
                Destroy();
            }
        }
        public override void UpdateBeforeSimulation()
        {
            if (!initialized)
            {
                return;
            }

            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;

            UpdateBefore(physicsDeltaTime);

        }

        public override void UpdateAfterSimulation()
        {
            if (!initialized)
            {
                return;
            }

            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;

            UpdateAfter(physicsDeltaTime);

        }
    }

}
