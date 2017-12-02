using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class RepulsorCoil : LogicElement
    {

        public RepulsorCoil(LogicComponent component) : base(component)
        {
        }

        public static Dictionary<long, List<RepulsorCoil>> repulsorCoils = new Dictionary<long, List<RepulsorCoil>>();


        #region use coil

        public readonly string USE_COIL_PROPERTY_NAME = "Coil_UseCoil";

        public bool defaultUseCoil = true;

        public bool useCoil
        {
            set
            {
                string id = GeneratePropertyId(USE_COIL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_COIL_PROPERTY_NAME);
                bool result = defaultUseCoil;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseCoil;
            }
        }

        public float minAltitude = 1;
        /*public float altitudePerMW = 1000;
        public float maxAltitudeWithPower = 10000;

        public float PowerRequirements() {
            float requirement = 0.001f + (maxAltitudeWithPower / altitudePerMW);
            MyAPIGateway.Utilities.ShowMessage("RepulsorCoil", "requirement: " + requirement);
            return requirement;
        }
        */

        #endregion

        #region power

        public readonly string POWER_PROPERTY_NAME = "RepulsorCoil_Power";

        public double defaultPower = 1;

        public double power
        {
            set
            {
                string id = GeneratePropertyId(POWER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(POWER_PROPERTY_NAME);
                double result = defaultPower;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultPower;
            }

        }

        #endregion

        #region lifecycle


        public double maxAltitude
        {
            get
            {
                return GetMaxAltitude(block.CubeGrid);
            }
        }

        public int coilsCount
        {
            get
            {
                if (!repulsorCoils.ContainsKey(block.CubeGrid.EntityId))
                {
                    return 0;
                }
                else
                {
                    return repulsorCoils[block.CubeGrid.EntityId].Count;
                }
            }
        }

        public override void Initialize()
        {

            List<RepulsorCoil> list;
            if (!repulsorCoils.ContainsKey(block.CubeGrid.EntityId))
            {
                list = new List<RepulsorCoil>();
            }
            else
            {
                list = repulsorCoils[block.CubeGrid.EntityId];
            }
            list.Add(this);
            repulsorCoils[block.CubeGrid.EntityId] = list;
            //MyAPIGateway.Utilities.ShowMessage(block.CubeGrid.CustomName, "repulsorCoils: " + repulsorCoils.Count + ", grid coils: " + list.Count + ", maxAltitude: " + maxAltitude);

        }

        public override void Destroy()
        {
            Clear();

            if (repulsorCoils.ContainsKey(block.CubeGrid.EntityId))
            {
                List<RepulsorCoil> list = repulsorCoils[block.CubeGrid.EntityId];
                list.Remove(this);
                //MyAPIGateway.Utilities.ShowMessage(block.CubeGrid.CustomName, "repulsorCoils: " + repulsorCoils.Count + ", grid coils: " + list.Count + ", maxAltitude: " + maxAltitude);
                if (list.Count == 0)
                {
                    repulsorCoils.Remove(block.CubeGrid.EntityId);
                }
                else
                {
                    repulsorCoils[block.CubeGrid.EntityId] = list;
                }
            }
            else
            {
                //MyAPIGateway.Utilities.ShowMessage(block.CubeGrid.CustomName, "repulsorCoils: " + repulsorCoils.Count + ", maxAltitude: " + maxAltitude);
            }

        }

        public void Clear()
        {
            power = defaultPower;
            useCoil = defaultUseCoil;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Repulsor Coil");
            customInfo.AppendLine("Use: " + (useCoil ? "On" : "Off"));
            customInfo.AppendLine("Tension: " + Math.Round(power, 1));
            customInfo.AppendLine("Max Altitude: " + Math.Round(maxAltitude, 1));
            customInfo.AppendLine("Gravity: " + Math.Round(logicComponent.rigidbody.gravity.Length(), 1));
            customInfo.AppendLine("Grid Mass: " + Math.Round(block.CubeGrid.Physics.Mass, 1) + " kg");
        }

        #endregion

        Vector3D desiredAcceleration;
        double altitude;

        public Vector3D GetLinearAcceleration(double physicsDeltaTime, double altitude)
        {

            //float power_ratio = resourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) * power_ratio_available;

            //if (!resourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)) {
            //  power_ratio = 0;
            //}


            desiredAcceleration = Vector3D.Zero;
            this.altitude = altitude;

            if (!useCoil)
            {
                return Vector3D.Zero;
            }

            if (double.IsNaN(altitude) || altitude > (maxAltitude + (block.WorldAABB.Size.Length() / 2)))
            {
                return Vector3D.Zero;
            }

            desiredAcceleration = (-logicComponent.rigidbody.gravity / (double)coilsCount) * power;

            //MyAPIGateway.Utilities.ShowMessage("acceleration:", acceleration.Length() + ", coilsCount: " + coilsCount + ", tension: " + tension);

            /*
                double max = cubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Large ? largeGridMaxDistance : smallGridMaxDistance;
                maxDistance += max;        
            */

            return desiredAcceleration;
        }

        public static double GetMaxAltitude(IMyCubeGrid grid)
        {
            double planetMaximumRadius = 30000;
            MyPlanet nearestPlanet = PlanetsList.GetNearestPlanet(grid);

            if (nearestPlanet != null)
            {
                planetMaximumRadius = nearestPlanet.MaximumRadius;
            }
            else
            {
                return float.NaN;
            }

            if (!repulsorCoils.ContainsKey(grid.EntityId))
            {
                return float.NaN;
            }
            else
            {
                List<RepulsorCoil> list = repulsorCoils[grid.EntityId];
                double size = grid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5;

                double power = 0;
                foreach (RepulsorCoil coil in list)
                {
                    if (!coil.useCoil)
                    {
                        continue;
                    }
                    power += coil.power;
                }
                double multiplier = Math.Pow(2, power);
                double max = MathHelper.Clamp(size * 10 * multiplier, 0, planetMaximumRadius);
                return max;
            }
        }

        public override void Debug()
        {

        }
    }
}
