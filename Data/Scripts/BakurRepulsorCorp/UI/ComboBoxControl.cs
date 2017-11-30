using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace BakurRepulsorCorp
{

    public abstract class ComboBoxControl<TEquipment> : BaseControl<TEquipment> where TEquipment : EquipmentBase
    {

        protected List<MyTerminalControlComboBoxItem> content;

        public ComboBoxControl(
            string controlId,
            string title,
            string description,
            List<MyTerminalControlComboBoxItem> content = null)
            : base(controlId, title, description)
        {
            if (content != null)
            {
                this.content = content;
            }
        }


        protected override IMyTerminalControl CreateControl()
        {
            IMyTerminalControlCombobox comboBox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCombobox, IMyUpgradeModule>(controlId);
            comboBox.Getter = Getter;
            comboBox.Setter = Setter;
            comboBox.SupportsMultipleBlocks = true;
            comboBox.Visible = Visible;
            comboBox.ComboBoxContent = FillContent2;
            comboBox.Enabled = Enabled;
            comboBox.Title = VRage.Utils.MyStringId.GetOrCompute(title);
            comboBox.Tooltip = VRage.Utils.MyStringId.GetOrCompute(description);

            return comboBox;
        }

        private void FillContent2(List<MyTerminalControlComboBoxItem> obj)
        {
            obj.Clear();
            foreach (MyTerminalControlComboBoxItem item in content)
            {
                obj.Add(item);
            }
        }

        protected long Getter(IMyCubeBlock block)
        {
            TEquipment equipment = GetEquipment(block);
            return GetValue(equipment);
        }

        protected void Setter(IMyCubeBlock block, long index)
        {
            TEquipment equipment = GetEquipment(block);
            SetValue(equipment, index);
        }

        protected abstract long GetValue(TEquipment equipment);

        protected abstract void SetValue(TEquipment equipment, long index);

        /*
        public virtual void FillContent(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> items, List<MyTerminalControlListBoxItem> selected) {

            items.Clear();
            selected.Clear();
            foreach (MyTerminalControlListBoxItem value in content) {
                items.Add(value);
            }

            List<int> indices = new List<int>();
            indices = Getter(block);

            foreach (var index in indices) {
                selected.Add(items[(int)index]);
            }

            if (selected.Count == 0 && items.Count > 0) {
                selected.Add(items[0]);
                Setter(block, selected);
            }
        }
        */
        /*
        public virtual List<long> Getter(IMyTerminalBlock block)
        {
            var names = new List<string>();
            var indices = new List<long>();
            MyAPIGateway.Utilities.(GetVariable<List<string>>(block.EntityId.ToString() + InternalName, out names);
            if (names != null)
            {
                foreach (var name in names)
                {
                    var index = Values[InternalName][block].IndexOf(name);
                    if (index != -1)
                    {
                        indices.Add(index);
                    }
                }
            }
            return indices;
        }
        */


        /*
    public virtual List<string> GetterName(IMyTerminalBlock block) {

        var names = new List<string>();
        var indices = this.Getter(block);
        foreach (var index in indices) {
            if (index < content.Count) {
                names.Add(content[index].Text.String);
            }
        }

        if (names == null) {
            return new List<string>();
        }
        return names;
    }

    public virtual List<TValue> GetterObjects(IMyTerminalBlock block) {
        var names = new List<TValue>();
        var indices = Getter(block);
        foreach (var index in indices) {
            if (index < content.Count) {
                names.Add((TValue)content[index].UserData);
            }
        }

        if (names == null) {
            return new List<TValue>();
        }
        return names;
    }

    public abstract List<int> Getter(IMyTerminalBlock block);
    public abstract void Setter(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> selected);
    */
        /*
        public virtual List<int> Getter(IMyTerminalBlock block) {
            var indices = new List<int>();
            var xmlList = new List<string>();
            MyAPIGateway.Utilities.(GetVariable<List<string>>(block.EntityId.ToString() + controlId, out xmlList);
            if (xmlList != null) {
                foreach (var xml in xmlList) {
                    var obj = MyAPIGateway.Utilities.SerializeFromXML<TValue>(xml);
                    var index = Values.FindIndex((x) => x.UserData.Equals(obj));
                    if (index >= 0) {
                        indices.Add(index);
                    }
                }
            }

            return indices;
        }

        public virtual void Setter(IMyTerminalBlock block, List<MyTerminalControlListBoxItem> selected) {

            var xmlList = new List<string>();
            foreach (var item in selected) {
                try {
                    var xml = MyAPIGateway.Utilities.SerializeToXML<TValue>((TValue)item.UserData);
                    xmlList.Add(xml);
                } catch {
                }

            }
            SetVariable<List<string>>(block.EntityId.ToString() + controlId, xmlList);
        }
        */
    }
}
