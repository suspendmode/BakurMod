using Sandbox.ModAPI;
using System.Text;
using VRage.Game.ModAPI;

namespace BakurRepulsorCorp
{

    public abstract class EquipmentBase
    {

        public bool isInitialized;

        #region properties

        BakurBlock _component;
        public BakurBlock component
        {
            get
            {
                return _component;
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

        public EquipmentBase(BakurBlock component)
        {
            this._component = component;
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
            return component.GetVariable<T>(id, out result);
        }

        public void SetVariable<T>(string id, T value)
        {
            component.SetVariable<T>(id, value);
        }

        public abstract void Debug();
    }
}
