using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Coil_UseCoilEnableAction : UIControlAction<RepulsorCoil> {

        public Coil_UseCoilEnableAction() : base("Coil_UseCoilEnableAction", "Enable Use Coil") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useCoil = true;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            RepulsorCoil component = GetEquipment(block);
            if (component == null) {
                return;
            }
            builder.Append(component.useCoil ? "Coil On" : "Coil Off");
        }
    }
}