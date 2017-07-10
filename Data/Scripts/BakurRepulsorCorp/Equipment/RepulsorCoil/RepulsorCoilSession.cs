using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class RepulsorCoilSession : BakurSession {

        public static double largeGridMaxDistance = 2.5;
        public static double smallGridMaxDistance = 0.5;

        public static Dictionary<IMyCubeGrid, List<RepulsorCoil>> repulsorCoilLookup = new Dictionary<IMyCubeGrid, List<RepulsorCoil>>();

        public static Dictionary<IMyCubeGrid, double> maxDistanceLookup = new Dictionary<IMyCubeGrid, double>();

        public override void Initialize() {

        }

        public override void Destroy() {

        }

        public override void Update(double physicsDeltaTime) {

            //MyAPIGateway.Utilities.ShowMessage("RepulsorCoilSession", "UpdateRepulsorCoils: " + repulsorCoilLookup.Count);

            foreach (KeyValuePair<IMyCubeGrid, List<RepulsorCoil>> item in repulsorCoilLookup) {

                IMyCubeGrid cubeGrid = item.Key;
                List<RepulsorCoil> list = item.Value;

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
                int enabledCoilsCount = 0;

                foreach (RepulsorCoil coil in list) {
                    coil.maxDistance = maxDistance;
                    if (coil.useCoil) {
                        enabledCoilsCount++;
                    }
                }

                foreach (RepulsorCoil coil in list) {
                    coil.enabledCoilsCount = enabledCoilsCount;
                }
            }
        }

        public static double GetMaxDistance(IMyCubeGrid cubeGrid) {

            List<RepulsorCoil> list;

            if (!repulsorCoilLookup.ContainsKey(cubeGrid)) {
                return double.NaN;
            }

            list = repulsorCoilLookup[cubeGrid];

            double maxDistance = 0;
            foreach (RepulsorCoil coil in list) {
                if (!coil.component.enabled) {
                    continue;
                }
                if (!coil.useCoil) {
                    continue;
                }

                double max = cubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Large ? largeGridMaxDistance : smallGridMaxDistance;
                maxDistance += max;
            }
            return maxDistance;
        }

        public static void AddRepulsorCoil(IMyCubeGrid cubeGrid, RepulsorCoil repulsorCoil) {
            List<RepulsorCoil> list;

            if (!repulsorCoilLookup.ContainsKey(cubeGrid)) {
                repulsorCoilLookup[cubeGrid] = new List<RepulsorCoil>();
            }

            list = repulsorCoilLookup[cubeGrid];
            list.Add(repulsorCoil);

            maxDistanceLookup[cubeGrid] = GetMaxDistance(cubeGrid);
        }

        public static void RemoveRepulsorCoil(IMyCubeGrid cubeGrid, RepulsorCoil repulsorCoil) {

            List<RepulsorCoil> list;

            if (!repulsorCoilLookup.ContainsKey(cubeGrid)) {
                return;
            }

            list = repulsorCoilLookup[cubeGrid];
            list.Remove(repulsorCoil);
            if (list.Count == 0) {
                repulsorCoilLookup.Remove(cubeGrid);
            }

            maxDistanceLookup[cubeGrid] = GetMaxDistance(cubeGrid);
        }
    }

}