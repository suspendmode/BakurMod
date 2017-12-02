using Sandbox.ModAPI;
using System;
using System.Text;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class AntiGravityGenerator : LogicElement
    {
        public AntiGravityGenerator(LogicComponent component) : base(component)
        {
        }

        #region use generator

        public readonly string USE_GENERATOR_PROPERTY_NAME = "Generator_UseGenerator";

        public bool defaultUseGenerator = true;

        public bool useGenerator
        {
            set
            {
                string id = GeneratePropertyId(USE_GENERATOR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_GENERATOR_PROPERTY_NAME);
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

        public readonly string POWER_PROPERTY_NAME = "Generator_Power";

        public double defaultPower = 1;

        public double power
        {
            set
            {
                string id = GeneratePropertyId(POWER_PROPERTY_NAME);
                SetVariable<double>(id, MathHelper.Clamp(value, 0, 1));
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

        public double load = 0;

        #region lifecycle

        public override void Initialize()
        {
            AntiGravityGenerators.Add(this as AntiGravityGenerator);

            //MyAPIGateway.Utilities.ShowMessage(block.CubeGrid.CustomName, "repulsorCoils: " + repulsorCoils.Count + ", grid coils: " + list.Count + ", maxAltitude: " + maxAltitude);           
        }

        public override void Destroy()
        {
            AntiGravityGenerators.Remove(this as AntiGravityGenerator);
            Clear();
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
            customInfo.AppendLine("Load: " + Math.Round(load, 1));
            customInfo.AppendLine("Mass: " + Math.Round(logicComponent.rigidbody.gridMass, 1));
            customInfo.AppendLine("Desired Mass: " + Math.Round(desiredMass, 1));
            customInfo.AppendLine("Max Mass: " + Math.Round(maxMass, 1));
            customInfo.AppendLine("Desired Force: " + Math.Round(desiredForce.Length(), 1));
        }

        #endregion

        public Vector3D desiredForce = Vector3D.Zero;
        public double desiredMass = 0;
        public double maxMass = 0;

        public override void Debug()
        {

        }
    }
}
