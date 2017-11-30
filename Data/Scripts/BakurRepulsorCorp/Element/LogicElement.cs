using Sandbox.ModAPI;
using System.Text;
using VRage.Game.ModAPI;

namespace BakurRepulsorCorp
{

    public abstract class LogicElement
    {

        public bool isInitialized;

        #region properties

        LogicComponent _logicComponent;
        public LogicComponent logicComponent
        {
            get
            {
                return _logicComponent;
            }
        }

        IMyTerminalBlock _block;

        public IMyTerminalBlock block
        {
            get
            {
                return _block;
            }
        }

        IMyCubeGrid _grid;
        public IMyCubeGrid grid
        {
            get
            {
                return _grid;
            }
        }

        #endregion

        public LogicElement(LogicComponent component)
        {
            this._logicComponent = component;
            this._block = (IMyTerminalBlock)component.Entity;
            this._grid = this._block.CubeGrid;
        }

        public abstract void Initialize();
        public abstract void Destroy();
        public abstract void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo);

        public string GeneratatePropertyId(string name)
        {
            return block.EntityId + "_" + name;
        }

        public bool GetVariable<T>(string id, out T result)
        {
            return logicComponent.GetVariable<T>(id, out result);
        }

        public void SetVariable<T>(string id, T value)
        {
            logicComponent.SetVariable<T>(id, value);
        }

        public abstract void Debug();
    }
}
