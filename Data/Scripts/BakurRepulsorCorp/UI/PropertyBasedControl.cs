using VRage.Game.ModAPI;

namespace BakurRepulsorCorp
{

    public abstract class PropertyBasedControl<TEquipment, TValue> : BaseControl<TEquipment> where TEquipment : EquipmentBase
    {

        public PropertyBasedControl(string controlId, string title, string description) : base(controlId, title, description)
        {

        }

        protected TValue Getter(IMyCubeBlock block)
        {
            TEquipment equipment = GetEquipment(block);
            return GetValue(equipment);
        }

        protected void Setter(IMyCubeBlock block, TValue value)
        {
            TEquipment equipment = GetEquipment(block);
            SetValue(equipment, value);
        }

        protected abstract TValue GetValue(TEquipment equipment);

        protected abstract void SetValue(TEquipment equipment, TValue value);
    }
}
