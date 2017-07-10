using Sandbox.ModAPI;
using System.Text;

namespace BakurRepulsorCorp {

    public class Component_EnabledToggleAction : BaseControlAction<BakurBlockEquipment> {

        public Component_EnabledToggleAction() : base("BakurBlockEquipment_BakurBlockEquipmentEnabledToggleAction", "Enabled On/Off") {
        }

        public override void Action(IMyTerminalBlock block) {
            BakurBlockEquipment equipment = GetEquipment(block);
            if (equipment == null) {
                return;
            }            
            equipment.enabled = !equipment.enabled;
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