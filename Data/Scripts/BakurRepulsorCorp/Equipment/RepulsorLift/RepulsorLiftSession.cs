using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]

    public class RepulsorLiftSession : BakurSession {

        public static double largeGrid = 2.5;
        public static double smallGrid = 0.5;

        public static Dictionary<IMyCubeGrid, List<RepulsorLift>> repulsorLiftLookup = new Dictionary<IMyCubeGrid, List<RepulsorLift>>();

        public static Dictionary<IMyCubeGrid, double> maxDistanceLookup = new Dictionary<IMyCubeGrid, double>();

        public override void Initialize() {

        }

        public override void Destroy() {

        }

        public override void Update(double physicsDeltaTime) {

            //MyAPIGateway.Utilities.ShowMessage("RepulsorCoilSession", "UpdateRepulsorCoils: " + repulsorCoilLookup.Count);

            foreach (KeyValuePair<IMyCubeGrid, List<RepulsorLift>> item in repulsorLiftLookup) {

                IMyCubeGrid cubeGrid = item.Key;
                List<RepulsorLift> list = item.Value;

                if (cubeGrid == null) {
                    continue;
                }

                if (cubeGrid.Physics == null || cubeGrid.Physics.IsStatic || cubeGrid.Physics.Gravity == Vector3.Zero) {
                    continue;
                }

                if (list.Count == 0) {
                    continue;
                }

                double maxDistance = maxDistanceLookup[cubeGrid];
                int enabledLiftsCount = 0;

                foreach (RepulsorLift coil in list) {
                    coil.maxDistance = maxDistance;
                    if (coil.useLift) {
                        enabledLiftsCount++;
                    }
                }

                foreach (RepulsorLift coil in list) {
                    coil.enabledLiftsCount = enabledLiftsCount;
                }
            }
        }

        public static double GetMaxDistance(IMyCubeGrid cubeGrid) {

            List<RepulsorLift> list;

            if (!repulsorLiftLookup.ContainsKey(cubeGrid)) {
                return double.NaN;
            }

            list = repulsorLiftLookup[cubeGrid];

            double maxDistance = 0;
            foreach (RepulsorLift lift in list) {
                if (!lift.component.enabled) {
                    continue;
                }
                if (!lift.useLift) {
                    continue;
                }

                double max = cubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Large ? largeGrid : smallGrid;
                maxDistance += max;
            }
            return maxDistance;
        }

        public static void AddRepulsorCoil(IMyCubeGrid cubeGrid, RepulsorLift repulsorCoil) {
            List<RepulsorLift> list;

            if (!repulsorLiftLookup.ContainsKey(cubeGrid)) {
                repulsorLiftLookup[cubeGrid] = new List<RepulsorLift>();
            }

            list = repulsorLiftLookup[cubeGrid];
            list.Add(repulsorCoil);

            maxDistanceLookup[cubeGrid] = GetMaxDistance(cubeGrid);
        }

        public static void RemoveRepulsorCoil(IMyCubeGrid cubeGrid, RepulsorLift repulsorCoil) {

            List<RepulsorLift> list;

            if (!repulsorLiftLookup.ContainsKey(cubeGrid)) {
                return;
            }

            list = repulsorLiftLookup[cubeGrid];
            list.Remove(repulsorCoil);
            if (list.Count == 0) {
                repulsorLiftLookup.Remove(cubeGrid);
            }

            maxDistanceLookup[cubeGrid] = GetMaxDistance(cubeGrid);
        }
    }

}