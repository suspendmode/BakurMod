using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Text;
using VRage.Utils;

namespace BakurRepulsorCorp {

    public abstract class Slider<TEquipment> : PropertyBasedControl<TEquipment, float> where TEquipment : EquipmentBase {

        public float min;
        public float max;

        public Slider(           
            string controlId,
            string title,
            string description,
            float min = 0,
            float max = 1)
            : base(controlId, title, description) {

            this.min = min;
            this.max = max;
        }

        protected override IMyTerminalControl CreateControl() {
            IMyTerminalControlSlider slider = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSlider, IMyTerminalBlock>(controlId);
            slider.Visible = Visible;
            slider.SetLimits(min, max);
            slider.Getter = Getter;
            slider.Setter = Setter;
            slider.Enabled = Enabled;
            slider.SupportsMultipleBlocks = true;
            slider.Writer = Writer;
            slider.Title = MyStringId.GetOrCompute(title);
            slider.Tooltip = MyStringId.GetOrCompute(description);            
            return slider;
        }

        protected override void DestroyControl(IMyTerminalControl control) {
            /*
            IMyTerminalControlSlider slider = (IMyTerminalControlSlider)control;
            slider.Visible = null;            
            slider.Getter = null;
            slider.Setter = null;
            slider.Enabled = null;            
            slider.Writer = null;
            */
            base.DestroyControl(control);
        }

        public virtual void Writer(IMyTerminalBlock block, StringBuilder builder) {
            if (!Visible(block)) {
                return;
            }
            builder.Clear();
            builder.Append(Getter(block).ToString());
        }        
    }
}