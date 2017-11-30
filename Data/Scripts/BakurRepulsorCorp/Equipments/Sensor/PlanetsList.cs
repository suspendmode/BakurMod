using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class PlanetsList : SessionComponent
    {
        public static List<MyPlanet> planets = new List<MyPlanet>();

        public override void Initialize()
        {

            MyAPIGateway.Entities.OnEntityAdd += EntityAdded;
            MyAPIGateway.Entities.OnEntityRemove += EntityRemoved;
        }

        public override void Destroy()
        {
            MyEntities.OnEntityAdd -= EntityAdded;
            MyEntities.OnEntityRemove -= EntityRemoved;

            planets.Clear();
        }

        public override void Update(double physicsDeltaTime)
        {

        }

        void EntityAdded(IMyEntity entity)
        {
            var planet = entity as MyPlanet;
            if (planet != null)
            {
                planets.Add(planet);
            }
        }

        void EntityRemoved(IMyEntity entity)
        {
            var planet = entity as MyPlanet;
            if (planet != null)
            {
                planets.Remove(planet);
            }
        }

        public static bool IsNearPlanet(Vector3D position, MyPlanet planet)
        {
            var distanceFromPlanet = Vector3D.Distance(planet.PositionComp.GetPosition(), position);
            return distanceFromPlanet <= planet.MaximumRadius + Math.Max(100, planet.MaximumRadius);
        }

        public static MyPlanet GetNearestPlanet(IMyCubeGrid grid)
        {
            Vector3D position = grid.WorldAABB.Center;

            MyPlanet planet = MyGamePruningStructure.GetClosestPlanet(position);

            if (planet != null && !IsNearPlanet(position, planet))
            {
                planet = null; //too far planet
            }

            return planet;
        }

        public override void UpdateAfterSimulation()
        {
            base.UpdateAfterSimulation();

        }
    }
}
