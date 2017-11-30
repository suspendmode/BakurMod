using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace BakurRepulsorCorp
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_UpgradeModule), true, new string[] { "SmallBlockRepulsorCoil", "LargeBlockRepulsorCoil" })]
    public class RepulsorCoilBlock : BakurBlock
    {
        PlanetAltitudeSensor planetAltitudeSensor;
        RepulsorCoil repulsorCoil;

        protected override void Initialize()
        {
            base.Initialize();

            var textBox =
                MyAPIGateway.TerminalControls
                    .CreateControl<IMyTerminalControlTextbox, IMyCargoContainer>("test_terminal_textbox");
            textBox.Title = MyStringId.GetOrCompute("Example textbox");
            textBox.Tooltip = MyStringId.GetOrCompute("Check this field");

            textBox.Getter = GetterTextBox;
            textBox.Setter = SetterTextBox;

            textBox.Enabled = (MyObjectBuilder_TerminalBlock) => { return true; };
            textBox.Visible = (MyObjectBuilder_TerminalBlock) => { return true; };

            repulsorCoil = new RepulsorCoil(this);
            AddEquipment(repulsorCoil);

            planetAltitudeSensor = new PlanetAltitudeSensor(this);
            AddEquipment(planetAltitudeSensor);

            /*SetPowerRequirements(block, () => {
                return repulsorCoil.PowerRequirements();
            });*/
        }

        private StringBuilder GetterTextBox(IMyTerminalBlock myTerminalBlock)
        {
            return new StringBuilder();
        }

        private void SetterTextBox(IMyTerminalBlock myTerminalBlock, StringBuilder stringBuilder)
        {
            stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("example");
            stringBuilder.AppendLine("textbox");
        }

        protected override void Destroy()
        {

            base.Destroy();

            RemoveEquipment(repulsorCoil);
            repulsorCoil = null;

            RemoveEquipment(planetAltitudeSensor);
            planetAltitudeSensor = null;
        }

        Vector3D coilAcceleration;

        protected override void UpdateSimulation(double physicsDeltaTime)
        {
            coilAcceleration = Vector3D.Zero;

            if (!rigidbody.IsInGravity)
            {
                return;
            }

            planetAltitudeSensor.UpdateSensor(physicsDeltaTime);

            // coil

            double gridHalfSize = (planetAltitudeSensor.useBlockPosition ? (block.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5) : block.WorldAABB.Size.Length()) / 2;
            coilAcceleration = repulsorCoil.GetLinearAcceleration(physicsDeltaTime, planetAltitudeSensor.altitude - gridHalfSize);

            // apply

            rigidbody.AddLinearAcceleration(coilAcceleration);
        }

        protected override void Debug()
        {
            if (debugEnabled)
            {
                IMyCubeGrid grid = block.CubeGrid;
                DebugDraw.DrawLine(block.GetPosition(), block.GetPosition() + rigidbody.gravityUp * rigidbody.gravity.Length(), Color.DeepSkyBlue, 0.1f);
            }
        }

        protected override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Coil Block ==");
            customInfo.AppendLine("Use : " + (repulsorCoil.useCoil ? "On" : "Off"));
            base.AppendCustomInfo(block, customInfo);
        }

        protected override string[] soundIds
        {
            get
            {
                return new string[] { "repulsor_coil_working_start", "repulsor_coil_working_loop", "repulsor_coil_working_end" };
            }
        }

        protected override Guid blockGUID()
        {
            return new Guid("bf8957e0-500d-4356-9875-91db9dd4a912");
        }

    }

}
