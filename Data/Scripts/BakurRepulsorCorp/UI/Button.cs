﻿using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;

namespace BakurRepulsorCorp
{

    public abstract class Button<TEquipment> : UIControl<TEquipment> where TEquipment : LogicElement
    {

        public Button(
            string controlId, string title, string description)
            : base(controlId, title, description)
        {
        }

        protected override IMyTerminalControl CreateControl()
        {
            IMyTerminalControlButton button = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlButton, IMyUpgradeModule>(controlId);
            button.Title = VRage.Utils.MyStringId.GetOrCompute(title);
            button.Action = OnAction;
            button.Enabled = Enabled;
            button.Visible = Visible;
            return button;
        }

        public abstract void OnAction(IMyTerminalBlock block);
    }
}