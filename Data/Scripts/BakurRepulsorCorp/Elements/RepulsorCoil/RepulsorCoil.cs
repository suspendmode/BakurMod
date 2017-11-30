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

        static Separator<RepulsorCoil> coilSeparator;
        static Label<RepulsorCoil> coilLabel;

        #region use coil

        static Coil_UseCoilSwitch useCoilSwitch;
        static Coil_UseCoilToggleAction useCoilToggleAction;
        static Coil_UseCoilEnableAction useCoilEnableAction;
        static Coil_UseCoilDisableAction useCoilDisableAction;

        public static string USE_COIL_PROPERTY_NAME = "Coil_UseCoil";

        public bool defaultUseCoil = true;

        public bool useCoil
        {
            set
            {
                string id = GeneratatePropertyId(USE_COIL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_COIL_PROPERTY_NAME);
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

        static Coil_PowerSlider powerSlider;
        static Coil_IncrasePowerAction incrasePowerAction;
        static Coil_DecrasePowerAction decrasePowerAction;

        public static string POWER_PROPERTY_NAME = "RepulsorCoil_Power";

        public double defaultPower = 1;

        public double power
        {
            set
            {
                string id = GeneratatePropertyId(POWER_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(POWER_PROPERTY_NAME);
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

            if (coilSeparator == null)
            {
                coilSeparator = new Separator<RepulsorCoil>("Coil_CoilSeparator");
                coilSeparator.Initialize();
            }

            if (coilLabel == null)
            {
                coilLabel = new Label<RepulsorCoil>("Coil_CoilLabel", "Repulsor Coil");
                coilLabel.Initialize();
            }

            // use repulsor coil

            if (useCoilSwitch == null)
            {
                useCoilSwitch = new Coil_UseCoilSwitch();
                useCoilSwitch.Initialize();
            }

            if (useCoilToggleAction == null)
            {
                useCoilToggleAction = new Coil_UseCoilToggleAction();
                useCoilToggleAction.Initialize();
            }

            if (useCoilEnableAction == null)
            {
                useCoilEnableAction = new Coil_UseCoilEnableAction();
                useCoilEnableAction.Initialize();
            }

            if (useCoilDisableAction == null)
            {
                useCoilDisableAction = new Coil_UseCoilDisableAction();
                useCoilDisableAction.Initialize();
            }

            // power

            if (powerSlider == null)
            {
                powerSlider = new Coil_PowerSlider();
                powerSlider.Initialize();
            }
            if (incrasePowerAction == null)
            {
                incrasePowerAction = new Coil_IncrasePowerAction();
                incrasePowerAction.Initialize();
            }
            if (decrasePowerAction == null)
            {
                decrasePowerAction = new Coil_DecrasePowerAction();
                decrasePowerAction.Initialize();
            }
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

            //MyAPIGateway.Utilities.ShowMessage("GetMaxAltitude", "GetMaxAltitude");
            /*
            MyResourceSinkComponent resourceSink;

            component.block.Components.TryGet(out resourceSink);

            if (resourceSink == null) {
                MyAPIGateway.Utilities.ShowMessage("GetMaxAltitude", "resourceSink == null");
                return 0;
            }
            */
            //if (!resourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)) {
            //MyAPIGateway.Utilities.ShowMessage("GetMaxAltitude", "!resourceSink.IsPoweredByType");
            //return 0;
            //}

            //float suppliedRatio = resourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);
            // MyAPIGateway.Utilities.ShowMessage("GetMaxAltitude", "suppliedRatio: " + suppliedRatio);

            //            return suppliedRatio;

            //if (resourceSink != null) {
            //  resourceSink.Update();
            //}

            /*

                 */

            if (!repulsorCoils.ContainsKey(grid.EntityId))
            {
                return float.NaN;
            }
            else
            {
                List<RepulsorCoil> list = repulsorCoils[grid.EntityId];
                double size = grid.GridSizeEnum == MyCubeSize.Large ? 4 : 2;
                double max = MathHelper.Clamp(Math.Pow(size, list.Count * size), 0, 20000);
                return max;
            }
        }

        public override void Debug()
        {

        }
    }
}
