using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Game.ModAPI;

namespace BakurRepulsorCorp {

    public abstract class BaseControl<TEquipment> where TEquipment : EquipmentBase {

        public IMyTerminalControl control;

        protected bool initialized = false;

        public string controlId;
        public string title;
        public string description;

        public BaseControl(
            string controlId,
            string title,
            string description) {
            this.controlId = controlId;
            this.title = title;

            //  MyLog.Default.WriteLine("Control contruct : " + name);
        }

        public virtual void Initialize() {

            if (!initialized) {
                control = CreateControl();
                MyAPIGateway.TerminalControls.AddControl<IMyTerminalBlock>(control);
                // MyLog.Default.WriteLine("Initialize: " + controlId);
                initialized = true;
                control.UpdateVisual();
            }
        }

        public virtual void Destroy() {

            if (initialized) {

                if (control != null) {
                    //MyLog.Default.WriteLine("Destroy: " + controlId);
                    DestroyControl(control);
                    MyAPIGateway.TerminalControls.RemoveControl<IMyTerminalBlock>(control);
                    control = null;
                }

                initialized = false;
            }
        }


        protected abstract IMyTerminalControl CreateControl();

        protected virtual void DestroyControl(IMyTerminalControl control) {
        }

        protected virtual bool Enabled(IMyTerminalBlock block) {
            return true;
        }

        protected virtual bool Visible(IMyTerminalBlock block) {
            if (block == null) {
                return false;
            }
            BakurBlock component = block.GameLogic.GetAs<BakurBlock>();
            if (component == null) {
                return false;
            }
            TEquipment equipment = component.GetEquipment<TEquipment>();
            if (equipment == null) {
                return false;
            }
            return true;
        }

        protected BakurBlock GetComponent(IMyCubeBlock block) {
            BakurBlock component = block.GameLogic.GetAs<BakurBlock>();
            return component;
        }

        protected TEquipment GetEquipment(IMyCubeBlock block) {
            BakurBlock component = GetComponent(block);
            TEquipment equipment = component.GetEquipment<TEquipment>();
            return equipment;
        }

        public virtual void Update() {
            control.UpdateVisual();
        }
    }
}