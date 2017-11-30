using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {


    public class Coil_UseCoilToggleAction : UIControlAction<RepulsorCoil> {

        public Coil_UseCoilToggleAction() : base("Coil_UseCoilToggleAction", "Use Coil On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            RepulsorCoil equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.useCoil = !equipment.useCoil;
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