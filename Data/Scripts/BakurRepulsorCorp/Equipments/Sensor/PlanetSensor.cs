using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{
    public abstract class PlanetSensor : EquipmentBase
    {
        public MyPlanet nearestPlanet;

        HashSet<IMyEntity> entitiesHelper = new HashSet<IMyEntity>();

        public virtual void UpdateSensor(double physicsDeltaTime)
        {
            UpdateNearestPlanet(grid);
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Planet Sensor");
            customInfo.AppendLine("IsInGravity: " + component.rigidbody.IsInGravity);
            customInfo.AppendLine("Planets: " + PlanetsList.planets.Count);
            customInfo.AppendLine("Nearest Planet: " + (nearestPlanet != null ? nearestPlanet.Name : "None"));
        }

        public PlanetSensor(BakurBlock component) : base(component)
        {
        }

        protected void UpdateNearestPlanet(IMyCubeGrid grid)
        {

            Clear();

            Vector3D center = grid.WorldAABB.Center;

            nearestPlanet = PlanetsList.GetNearestPlanet(grid);
        }

        protected virtual void Clear()
        {
            nearestPlanet = null;
        }
    }
}
