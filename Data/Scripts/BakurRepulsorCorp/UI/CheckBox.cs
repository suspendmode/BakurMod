using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Utils;

namespace BakurRepulsorCorp
{

    public abstract class CheckBox<TEquipment> : PropertyBasedControl<TEquipment, bool> where TEquipment : EquipmentBase
    {

        public CheckBox(
            string controlId,
            string title,
            string description)
            : base(controlId, title, description)
        {
        }

        protected override IMyTerminalControl CreateControl()
        {
            IMyTerminalControlCheckbox checkbox =
                MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCheckbox, IMyUpgradeModule>(controlId);
            checkbox.Visible = Visible;
            checkbox.Tooltip = MyStringId.GetOrCompute(description);
            checkbox.Getter = Getter;
            checkbox.Setter = Setter;
            checkbox.Title = MyStringId.GetOrCompute(title);
            return checkbox;
        }
    }
}
