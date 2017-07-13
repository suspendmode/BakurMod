using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp {

    public class BakurBlockEquipment : EquipmentBase {

        static Separator<BakurBlockEquipment> separator;
        static Label<BakurBlockEquipment> label;

        #region enabled

        static Component_EnabledSwitch enableToggle;
        static Component_EnabledToggleAction enableToggleAction;
        static Component_EnableAction enableAction;
        static Component_DisableAction disableAction;

        public static string ENABLED_PROPERTY_NAME = "Enabled";

        public bool defaultEnabled = true;

        public event Action EnabledChangedEvent;

        public bool enabled
        {
            set
            {
                string id = GeneratatePropertyId(ENABLED_PROPERTY_NAME);

                bool oldValue = defaultEnabled;
                if (GetVariable<bool>(id, out oldValue)) {
                    if (oldValue == value) {
                        return;
                    }
                }

                SetVariable<bool>(id, value);
                if (EnabledChangedEvent != null) {
                    EnabledChangedEvent();
                }
            }
            get
            {
                string id = GeneratatePropertyId(ENABLED_PROPERTY_NAME);
                bool result = defaultEnabled;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultEnabled;
            }

        }

        #endregion

        #region debugEnabled

        static Component_DebugEnabledSwitch debugEnableToggle;

        public static string DEBUG_ENABLED_PROPERTY_NAME = "DebugEnabled";

        public bool defaultDebugEnabled = false;

        public bool debugEnabled
        {
            set
            {
                string id = GeneratatePropertyId(DEBUG_ENABLED_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(DEBUG_ENABLED_PROPERTY_NAME);
                bool result = defaultDebugEnabled;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultDebugEnabled;
            }
        }

        #endregion

        public BakurBlockEquipment(BakurBlock component) : base(component) {
        }

        public override void Initialize() {

            EnabledChangedEvent += UpdateUI;

            if (separator == null) {
                separator = new Separator<BakurBlockEquipment>("BlockSeparator");
                separator.Initialize();
            }

            if (label == null) {
                label = new Label<BakurBlockEquipment>("BlockLabel", "Bakur Repulsor Corp");
                label.Initialize();
            }

            if (enableToggle == null) {
                enableToggle = new Component_EnabledSwitch();
                enableToggle.Initialize();
            }

            if (enableToggleAction == null) {
                enableToggleAction = new Component_EnabledToggleAction();
                enableToggleAction.Initialize();
            }

            if (enableAction == null) {
                enableAction = new Component_EnableAction();
                enableAction.Initialize();
            }

            if (disableAction == null) {
                disableAction = new Component_DisableAction();
                disableAction.Initialize();
            }

            if (debugEnableToggle == null) {
                debugEnableToggle = new Component_DebugEnabledSwitch();
                debugEnableToggle.Initialize();
            }
        }

        public override void Destroy() {
            EnabledChangedEvent -= UpdateUI;
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Suspension ==");
            customInfo.AppendLine("Enabled : " + enabled);
            customInfo.AppendLine("Debug Enabled : " + debugEnabled);
        }

        public override void Debug() {
            if (!component.debugEnabled) {
                return;
            }
        }

        protected virtual void UpdateUI() {

        }
    }
}