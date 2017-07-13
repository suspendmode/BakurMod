using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class PlanetsSession : BakurSession {

        public static List<MyPlanet> planets = new List<MyPlanet>();

        HashSet<IMyEntity> entitiesHelper = new HashSet<IMyEntity>();

        public override void Initialize() {
            planets.Clear();
        }

        public override void Destroy() {
            planets.Clear();
        }

        public override void Update(double physicsDeltaTime) {
            try {
                UpdatePlanets();
            } catch (Exception exception) {
                planets.Clear();
                MyAPIGateway.Utilities.ShowMessage("PlanetsSession", exception.StackTrace.ToString());
            }
        }

        protected void UpdatePlanets() {
            MyAPIGateway.Entities.GetEntities(entitiesHelper, delegate (IMyEntity entity) {
                if (entity is MyPlanet) {

                    MyPlanet planet = entity as MyPlanet;
                    if (!planets.Contains(planet)) {
                        planets.Add(planet);
                    }
                    if (planet.Closed || planet.MarkedForClose) {
                        planets.Remove(planet);
                    }
                }
                return false;
            });
        }

        public static MyPlanet GetNearestPlanet(IMyCubeGrid grid) {

            Vector3D center = grid.WorldAABB.Center;

            //Vector3D center = grid.Physics.CenterOfMassWorld;

            MyPlanet nearestPlanet = null;

            foreach (MyPlanet planet in planets) {

                if (planet == null || planet.Closed || planet.MarkedForClose) {
                    continue;
                }

                bool isNearPlanet = IsNearPlanet(center, planet);

                //MyAPIGateway.Utilities.ShowMessage("N", "count:" + planets.Count + ", distance:" + Vector3.Distance(grid.WorldMatrix.Translation, planet.WorldMatrix.Translation) + ", atmo:" + planet.AtmosphereRadius + ", min:" + planet.MinimumRadius + ", avg:" + planet.AverageRadius + ", max:" + planet.MaximumRadius);
                //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "inGravity:" + IsInGravity + ", near:" + isNearPlanet);

                if (!isNearPlanet) {
                    continue;
                }

                if (nearestPlanet == null) {
                    nearestPlanet = planet;
                    continue;
                }

                double distance = Vector3D.Distance(center, planet.WorldMatrix.Translation);
                double distanceToNearest = Vector3D.Distance(center, nearestPlanet.WorldMatrix.Translation);

                if (distance < distanceToNearest) {
                    nearestPlanet = planet;
                }
            }

            return nearestPlanet;
        }


        public static bool IsNearPlanet(Vector3D point, MyPlanet planet) {
            return Vector3D.Distance(point, planet.WorldMatrix.Translation) < (planet.AtmosphereRadius);
        }


        /*
        public bool IsNearPlanet(Vector3D point, MyPlanet planet) {
            if (planet.HasAtmosphere) {
                return Vector3D.DistanceSquared(point, planet.WorldMatrix.Translation) < (planet.AtmosphereRadius * planet.AtmosphereRadius);
            } else {
                return Vector3D.Distance(point, planet.WorldMatrix.Translation) < planet.SizeInMetres.LengthSquared();
            }
        }
        */


    }
}