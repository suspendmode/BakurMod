using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class AntiGravityGenerator : LogicElement
    {
        static double largeBlockMaxMass = 1000000;
        static double smallBlockMaxMass = 100000;

        public static readonly Dictionary<long, List<AntiGravityGenerator>> generators = new Dictionary<long, List<AntiGravityGenerator>>();

        public AntiGravityGenerator(LogicComponent component) : base(component)
        {
        }

        static Separator<AntiGravityGenerator> generatorSeparator;
        static Label<AntiGravityGenerator> generatorLabel;

        #region use gemerator

        static Generator_UseGeneratorSwitch useGeneratorSwitch;
        static Generator_UseGeneratorToggleAction useGeneratorToggleAction;
        static Generator_UseGeneratorEnableAction useGeneratorEnableAction;
        static Generator_UseGeneratorDisableAction useGeneratorDisableAction;

        public static string USE_GENERATOR_PROPERTY_NAME = "Generator_UseGenerator";

        public bool defaultUseGenerator = true;

        public bool useGenerator
        {
            set
            {
                string id = GeneratatePropertyId(USE_GENERATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_GENERATOR_PROPERTY_NAME);
                bool result = defaultUseGenerator;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseGenerator;
            }
        }

        #endregion

        #region power

        static Generator_PowerSlider powerSlider;
        static Generator_IncrasePowerAction incrasePowerAction;
        static Generator_DecrasePowerAction decrasePowerAction;

        public static string POWER_PROPERTY_NAME = "Generator_Power";

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

        public double maxMass
        {
            get
            {
                return GetMaxMass(block.CubeGrid);
            }
        }

        public int generatorsCount
        {
            get
            {
                if (!generators.ContainsKey(block.CubeGrid.EntityId))
                {
                    return 0;
                }
                else
                {
                    return generators[block.CubeGrid.EntityId].Count;
                }
            }
        }

        public override void Initialize()
        {

            List<AntiGravityGenerator> list;
            if (!generators.ContainsKey(block.CubeGrid.EntityId))
            {
                list = new List<AntiGravityGenerator>();
            }
            else
            {
                list = generators[block.CubeGrid.EntityId];
            }
            list.Add(this);
            generators[block.CubeGrid.EntityId] = list;
            //MyAPIGateway.Utilities.ShowMessage(block.CubeGrid.CustomName, "repulsorCoils: " + repulsorCoils.Count + ", grid coils: " + list.Count + ", maxAltitude: " + maxAltitude);

            if (generatorSeparator == null)
            {
                generatorSeparator = new Separator<AntiGravityGenerator>("Generator_GeneratorSeparator");
                generatorSeparator.Initialize();
            }

            if (generatorLabel == null)
            {
                generatorLabel = new Label<AntiGravityGenerator>("Generator_GeneratorLabel", "Anti Gravity Generator");
                generatorLabel.Initialize();
            }

            // use repulsor coil

            if (useGeneratorSwitch == null)
            {
                useGeneratorSwitch = new Generator_UseGeneratorSwitch();
                useGeneratorSwitch.Initialize();
            }

            if (useGeneratorToggleAction == null)
            {
                useGeneratorToggleAction = new Generator_UseGeneratorToggleAction();
                useGeneratorToggleAction.Initialize();
            }

            if (useGeneratorEnableAction == null)
            {
                useGeneratorEnableAction = new Generator_UseGeneratorEnableAction();
                useGeneratorEnableAction.Initialize();
            }

            if (useGeneratorDisableAction == null)
            {
                useGeneratorDisableAction = new Generator_UseGeneratorDisableAction();
                useGeneratorDisableAction.Initialize();
            }

            // power

            if (powerSlider == null)
            {
                powerSlider = new Generator_PowerSlider();
                powerSlider.Initialize();
            }
            if (incrasePowerAction == null)
            {
                incrasePowerAction = new Generator_IncrasePowerAction();
                incrasePowerAction.Initialize();
            }
            if (decrasePowerAction == null)
            {
                decrasePowerAction = new Generator_DecrasePowerAction();
                decrasePowerAction.Initialize();
            }
        }

        public override void Destroy()
        {
            Clear();

            if (generators.ContainsKey(block.CubeGrid.EntityId))
            {
                List<AntiGravityGenerator> list = generators[block.CubeGrid.EntityId];
                list.Remove(this);
                //MyAPIGateway.Utilities.ShowMessage(block.CubeGrid.CustomName, "repulsorCoils: " + repulsorCoils.Count + ", grid coils: " + list.Count + ", maxAltitude: " + maxAltitude);
                if (list.Count == 0)
                {
                    generators.Remove(block.CubeGrid.EntityId);
                }
                else
                {
                    generators[block.CubeGrid.EntityId] = list;
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
            useGenerator = defaultUseGenerator;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Anti Gravity Generator");
            customInfo.AppendLine("Use: " + (useGenerator ? "On" : "Off"));
            customInfo.AppendLine("Power: " + Math.Round(power, 1));
            customInfo.AppendLine("Max Mass: " + Math.Round(maxMass, 1));
            customInfo.AppendLine("Desired Mass: " + Math.Round(desiredMass, 1));
            customInfo.AppendLine("Max Block Mass: " + Math.Round(GetMaxMass(grid), 1));
            customInfo.AppendLine("Gravity : " + Math.Round(logicComponent.rigidbody.gravity.Length(), 1));
            customInfo.AppendLine("Grid Mass : " + Math.Round(block.CubeGrid.Physics.Mass, 1) + " kg");
        }

        #endregion

        Vector3D desiredAcceleration;

        public double desiredMass = 0;

        public Vector3D GetLinearAcceleration()
        {

            desiredAcceleration = Vector3D.Zero;
            desiredMass = 0;

            if (!useGenerator)
            {
                return Vector3D.Zero;
            }


            int index = generators[grid.EntityId].IndexOf(this);
            double maxBlockMass = MaxBlockMass(grid);
            double maxMassAtIndex = (index + 1) * maxBlockMass;

            double mass = logicComponent.rigidbody.gridMass;
            if (mass >= maxMass)
            {
                desiredMass = maxBlockMass;
            }
            else
            {
                desiredMass = maxBlockMass - (mass - maxMassAtIndex);
            }
            desiredAcceleration = -logicComponent.rigidbody.gravity * desiredMass * power;

            //MyAPIGateway.Utilities.ShowMessage("acceleration:", acceleration.Length() + ", coilsCount: " + coilsCount + ", tension: " + tension);

            return desiredAcceleration;
        }

        public static double MaxBlockMass(IMyCubeGrid grid)
        {
            return grid.GridSizeEnum == MyCubeSize.Large ? largeBlockMaxMass : smallBlockMaxMass;
        }

        public static double GetMaxMass(IMyCubeGrid grid)
        {

            //MyAPIGateway.Utilities.ShowMessage("GetMaxAltitude", "GetMaxAltitude");

            if (!generators.ContainsKey(grid.EntityId))
            {
                return 0;
            }
            else
            {
                List<AntiGravityGenerator> list = generators[grid.EntityId];
                double max = list.Count * MaxBlockMass(grid);
                return max;
            }
        }

        public override void Debug()
        {

        }
    }
}
