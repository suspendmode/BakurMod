using VRage.Game;

namespace BakurRepulsorCorp
{

    public abstract class NonStaticBakurBlock : BakurBlock {

        #region update

        public override void UpdateBeforeSimulation() {

            if (!initialized || !enabled || block == null || block.MarkedForClose || block.Closed || !block.IsWorking || block.CubeGrid == null || block.CubeGrid.IsStatic || block.CubeGrid.Physics == null || !block.CubeGrid.Physics.Enabled) {
                return;
            }

            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            double updateDeltaTime = MyEngineConstants.UPDATE_STEP_SIZE_IN_SECONDS;

            UpdateBeforeFrame(physicsDeltaTime, updateDeltaTime);
            if (debugEnabled) {
                foreach (EquipmentBase equipment in equipments) {
                    equipment.Debug();
                }
            }
        }

        #endregion

    }
}