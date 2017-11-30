using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Game.ModAPI;

namespace BakurRepulsorCorp
{

    public abstract class BaseControl<TEquipment> where TEquipment : EquipmentBase
    {

        public IMyTerminalControl control;

        protected bool initialized = false;

        public string controlId;
        public string title;
        public string description;

        public BaseControl(string controlId, string title, string description)
        {
            this.controlId = controlId;
            this.title = title;
            this.description = description;
            //  MyLog.Default.WriteLine("Control contruct : " + name);
            //MyAPIGateway.Utilities.ShowMessage("BaseControl", "controlId:" + controlId + ", title:" + title);
        }

        public virtual void Initialize()
        {

            if (!initialized)
            {

                control = CreateControl();

                MyAPIGateway.TerminalControls.AddControl<IMyUpgradeModule>(control);
                //MyAPIGateway.Utilities.ShowMessage("BaseControl", "Initialize, " + controlId);
                initialized = true;
                control.UpdateVisual();
                RefreshControl();
            }
        }

        public virtual void Destroy()
        {

            if (initialized)
            {

                if (control != null)
                {
                    //MyAPIGateway.Utilities.ShowMessage("BaseControl", "Destroy, " + controlId);

                    DestroyControl(control);
                    MyAPIGateway.TerminalControls.RemoveControl<IMyUpgradeModule>(control);
                    control = null;
                }

                initialized = false;
            }
        }


        protected abstract IMyTerminalControl CreateControl();

        protected virtual void DestroyControl(IMyTerminalControl control)
        {
        }

        protected virtual bool Enabled(IMyTerminalBlock block)
        {
            if (block == null)
            {
                //MyAPIGateway.Utilities.ShowMessage("BaseControl", "!Visible, block == null");
                return false;
            }
            BakurBlock component = block.GameLogic.GetAs<BakurBlock>();
            if (component == null)
            {
                MyAPIGateway.Utilities.ShowMessage("BaseControl", "!Visible, component == null");
                return false;
            }
            return true;
        }

        protected virtual bool Visible(IMyTerminalBlock block)
        {

            if (block == null)
            {
                //MyAPIGateway.Utilities.ShowMessage("BaseControl", "!Visible, block == null");
                return false;
            }
            BakurBlock component = block.GameLogic.GetAs<BakurBlock>();
            if (component == null)
            {
                //MyAPIGateway.Utilities.ShowMessage("BaseControl", "!Visible, component == null");
                return false;
            }
            TEquipment equipment = component.GetEquipment<TEquipment>();
            if (equipment == null)
            {
                //MyAPIGateway.Utilities.ShowMessage("BaseControl", "!Visible, equipment == null");
                return false;
            }

            //MyAPIGateway.Utilities.ShowMessage("BaseControl", "Visible");
            return true;
        }

        protected BakurBlock GetComponent(IMyCubeBlock block)
        {
            BakurBlock component = block.GameLogic.GetAs<BakurBlock>();
            return component;
        }

        protected TEquipment GetEquipment(IMyCubeBlock block)
        {
            BakurBlock component = GetComponent(block);
            TEquipment equipment = component.GetEquipment<TEquipment>();
            return equipment;
        }

        public virtual void RefreshControl()
        {
            //MyAPIGateway.Utilities.ShowMessage("BaseControl", "RefreshControl");

            control.UpdateVisual();
        }
    }
}
