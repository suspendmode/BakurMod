using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using VRage.ModAPI;
using VRage.Utils;

namespace BakurRepulsorCorp {

    public class Copter_ModeComboBox : ComboBox<Copter> {

        public static List<string> modes = new List<string> {
            "hover","glide","freeglide","pitch","roll","cruise"
        };

        static List<MyTerminalControlComboBoxItem> modeItems = new List<MyTerminalControlComboBoxItem>();

        public Copter_ModeComboBox() : base("Copter_ResponsivitySlider", "Mode", "hover, glide, freeglide, pitch, roll, cruise", GetModeItems()) {

        }

        private static List<MyTerminalControlComboBoxItem> GetModeItems() {
            if (modeItems.Count == 0) {
                MyTerminalControlComboBoxItem item0 = new MyTerminalControlComboBoxItem();
                item0.Key = 0;
                item0.Value = MyStringId.GetOrCompute("Hover");
                modeItems.Add(item0);
                MyTerminalControlComboBoxItem item1 = new MyTerminalControlComboBoxItem();
                item1.Key = 1;
                item1.Value = MyStringId.GetOrCompute("Glide");
                modeItems.Add(item1);
                MyTerminalControlComboBoxItem item2 = new MyTerminalControlComboBoxItem();
                item2.Key = 2;
                item2.Value = MyStringId.GetOrCompute("Free Glide");
                modeItems.Add(item2);
                MyTerminalControlComboBoxItem item3 = new MyTerminalControlComboBoxItem();
                item3.Key = 3;
                item3.Value = MyStringId.GetOrCompute("Pitch");
                modeItems.Add(item3);
                MyTerminalControlComboBoxItem item4 = new MyTerminalControlComboBoxItem();
                item4.Key = 4;
                item4.Value = MyStringId.GetOrCompute("Roll");
                modeItems.Add(item4);
                MyTerminalControlComboBoxItem item5 = new MyTerminalControlComboBoxItem();
                item5.Key = 5;
                item5.Value = MyStringId.GetOrCompute("Cruise");
                modeItems.Add(item5);
            }
            return modeItems;
        }

        protected override long GetValue(Copter equipment) {
            MyStringId mode = MyStringId.GetOrCompute(equipment.mode);
            foreach (MyTerminalControlComboBoxItem modeItem in modeItems) {
                if (modeItem.Value.Equals(mode)) {
                    return modeItem.Key;
                }
            }
            return 0;
        }


        protected override void SetValue(Copter equipment, long index) {
            foreach (MyTerminalControlComboBoxItem modeItem in modeItems) {
                if (modeItem.Key.Equals(index)) {
                    equipment.mode = modeItem.Value.String;
                }
            }
        }

        protected override bool Visible(IMyTerminalBlock block) {
            if (!base.Visible(block)) {
                return false;
            }
            Copter equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.logicComponent.enabled && equipment.useCopter;
        }
    }

}