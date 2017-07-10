using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Component_DisableAction : BaseControlAction<BakurBlockEquipment> {

        public Component_DisableAction() : base("BakurBlockEquipment_BakurBlockEquipmentDisableAction", "Disable") {
        }

        public override void Action(IMyTerminalBlock block) {
            BakurBlockEquipment equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            equipment.enabled = false;
        }

        public override void Writer(IMyTerminalBlock block, StringBuilder builder) {
            builder.Clear();
            BakurBlockEquipment equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }
            builder.Append(equipment.enabled ? "On" : "Off");
        }
    }
}