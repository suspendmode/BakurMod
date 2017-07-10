using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game.ModAPI;

namespace BakurRepulsorCorp {

    public class BakurBlockUtils {

        static List<IMySlimBlock> shipControllers = new List<IMySlimBlock>();

        public static bool IsUnderControl(IMyCubeGrid grid) {            
            return GetShipControllerUnderControl(grid) != null;
        }


        public static IMyShipController GetShipControllerUnderControl(IMyCubeGrid grid) {
            shipControllers.Clear();
            grid.GetBlocks(shipControllers, (IMySlimBlock block) => {
                bool isShipController = block.FatBlock is IMyShipController;
                return isShipController;
            });

            for (int i = 0; i < shipControllers.Count; i++) {
                IMyShipController shipController = shipControllers[i].FatBlock as IMyShipController;
                if (shipController.IsUnderControl) {
                    return shipController;
                }
            }
            return null;
        }
    }
}