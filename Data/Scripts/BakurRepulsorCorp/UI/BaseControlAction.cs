using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Text;

namespace BakurRepulsorCorp
{

    public abstract class BaseControlAction<TEquipment, TBlockType> where TEquipment : EquipmentBase where TBlockType : IMyFunctionalBlock
    {

        public IMyTerminalAction action;

        protected bool initialized = false;

        public string actionId;
        public string description;
        public string icon;

        public BaseControlAction(
              string actionId,
            string description,
            string icon = null)
        {
            this.actionId = actionId;
            this.description = description;
            this.icon = icon;
        }

        public virtual void Initialize()
        {

            if (!initialized)
            {

                // ui

                action = CreateAction();
                MyAPIGateway.TerminalControls.AddAction<TBlockType>(action);
                initialized = true;
            }
        }

        public virtual void Destroy()
        {

            if (initialized)
            {
                // ui

                if (action != null)
                {
                    MyAPIGateway.TerminalControls.RemoveAction<IMyTerminalBlock>(action);
                    DestroyAction(action);
                    action = null;
                }

                initialized = false;
            }
        }

        protected virtual IMyTerminalAction CreateAction()
        {
            IMyTerminalAction action = MyAPIGateway.TerminalControls.CreateAction<TBlockType>(actionId);
            action.Action = Action;
            action.Name = new StringBuilder(description);
            action.Enabled = Visible;
            action.Writer = Writer;
            if (icon != null)
            {
                action.Icon = icon;
            }
            return action;
        }

        public virtual void DestroyAction(IMyTerminalAction action)
        {

        }

        public abstract void Action(IMyTerminalBlock block);

        public abstract void Writer(IMyTerminalBlock block, StringBuilder builder);

        protected virtual bool Visible(IMyTerminalBlock block)
        {
            if (block == null)
            {
                return false;
            }
            BakurBlock component = block.GameLogic.GetAs<BakurBlock>();
            if (component == null)
            {
                return false;
            }
            TEquipment equipment = component.GetEquipment<TEquipment>();
            if (equipment == null)
            {
                return false;
            }
            return true;
        }

        protected BakurBlock GetComponent(IMyTerminalBlock block)
        {
            BakurBlock component = block.GameLogic.GetAs<BakurBlock>();
            return component;
        }

        protected TEquipment GetEquipment(IMyTerminalBlock block)
        {
            BakurBlock component = GetComponent(block);
            TEquipment equipment = component.GetEquipment<TEquipment>();
            return equipment;
        }
    }
}
