using Sandbox.ModAPI;
using System;
using System.Text;

namespace BakurRepulsorCorp
{

    public class DefaultLogicElement : LogicElement
    {

        #region enabled

        public readonly string ENABLED_PROPERTY_NAME = "Enabled";

        public bool defaultEnabled = true;

        public event Action EnabledChangedEvent;

        public bool enabled
        {
            set
            {
                string id = GeneratePropertyId(ENABLED_PROPERTY_NAME);

                bool oldValue = defaultEnabled;
                if (GetVariable<bool>(id, out oldValue))
                {
                    if (oldValue == value)
                    {
                        return;
                    }
                }

                SetVariable<bool>(id, value);
                if (EnabledChangedEvent != null)
                {
                    EnabledChangedEvent();
                }
                RefreshControls();
            }
            get
            {
                string id = GeneratePropertyId(ENABLED_PROPERTY_NAME);
                bool result = defaultEnabled;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultEnabled;
            }

        }

        protected virtual void RefreshControls()
        {
            //MyAPIGateway.Utilities.ShowMessage("BakurBlockElement", "RefreshControls");
        }

        #endregion

        #region debugEnabled

        public readonly string DEBUG_ENABLED_PROPERTY_NAME = "DebugEnabled";

        public bool defaultDebugEnabled = false;

        public bool debugEnabled
        {
            set
            {
                string id = GeneratePropertyId(DEBUG_ENABLED_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(DEBUG_ENABLED_PROPERTY_NAME);
                bool result = defaultDebugEnabled;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultDebugEnabled;
            }
        }

        #endregion

        public DefaultLogicElement(LogicComponent component) : base(component)
        {
        }

        public override void Initialize()
        {
            //MyAPIGateway.Utilities.ShowMessage("BakurBlockElement", "Initialize");

            EnabledChangedEvent += UpdateUI;

        }

        public override void Destroy()
        {
            EnabledChangedEvent -= UpdateUI;
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("Type: Component");
            customInfo.AppendLine("Enabled : " + enabled);
            customInfo.AppendLine("Debug Enabled : " + debugEnabled);
        }

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }
        }

        protected virtual void UpdateUI()
        {

        }
    }
}
