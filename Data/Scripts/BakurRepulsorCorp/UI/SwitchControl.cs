using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Text;

namespace BakurRepulsorCorp
{

    public abstract class SwitchControl<TEquipment> : PropertyBasedControl<TEquipment, bool> where TEquipment : EquipmentBase
    {

        protected string onButtonText;
        protected string offButtonText;

        // public string internalNameOnAction;
        // public string displayNameOnAction;

        // public string internalNameOffAction;
        // public string displayNameOffAction;

        // public string internalNameToggleAction;
        // public string displayNameToggleAction;

        public SwitchControl(
            string controlId,
            string title,
            string description,
            string onButton,
            string offButton)
            : base(controlId, title, description)
        {
            onButtonText = onButton;
            offButtonText = offButton;
        }

        protected override IMyTerminalControl CreateControl()
        {
            IMyTerminalControlOnOffSwitch switchControl = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlOnOffSwitch, IMyUpgradeModule>(controlId);
            switchControl.Enabled = Enabled;
            switchControl.Visible = Visible;
            switchControl.Getter = Getter;
            switchControl.Setter = Setter;
            switchControl.SupportsMultipleBlocks = true;
            switchControl.Title = VRage.Utils.MyStringId.GetOrCompute(title);
            switchControl.Tooltip = VRage.Utils.MyStringId.GetOrCompute(description);
            switchControl.OnText = VRage.Utils.MyStringId.GetOrCompute(onButtonText);
            switchControl.OffText = VRage.Utils.MyStringId.GetOrCompute(offButtonText);
            /*
            var onAction = MyAPIGateway.TerminalControls.CreateAction<T>(InternalNameOnAction);
            onAction.Action = OnAction;
            onAction.Name = new StringBuilder(DisplayNameOnAction);
            onAction.Writer = HotbarText;
            MyAPIGateway.TerminalControls.AddAction<T>(onAction);

            var offAction = MyAPIGateway.TerminalControls.CreateAction<T>(InternalNameOnAction);
            offAction.Action = OffAction;
            offAction.Name = new StringBuilder(DisplayNameOnAction);
            offAction.Writer = HotbarText;
            MyAPIGateway.TerminalControls.AddAction<T>(offAction);

            var toggleAction = MyAPIGateway.TerminalControls.CreateAction<T>(InternalNameOnAction);
            toggleAction.Action = ToggleAction;
            toggleAction.Name = new StringBuilder(DisplayNameOnAction);
            toggleAction.Writer = HotbarText;
            MyAPIGateway.TerminalControls.AddAction<T>(toggleAction);*/

            return switchControl;
        }

        protected override void DestroyControl(IMyTerminalControl control)
        {
            /*
            IMyTerminalControlOnOffSwitch switchControl = (IMyTerminalControlOnOffSwitch)control;
            switchControl.Enabled = null;
            switchControl.Visible = null;
            switchControl.Getter = null;
            switchControl.Setter = null;
            */
            base.DestroyControl(control);
        }

        public void ToggleAction(IMyTerminalBlock block)
        {
            Setter(block, !Getter(block));
        }

        void HotbarText(IMyTerminalBlock block, StringBuilder hotbarText)
        {
            hotbarText.Clear();
            hotbarText.Append(Getter(block) ? onButtonText : offButtonText);
        }
    }
}
