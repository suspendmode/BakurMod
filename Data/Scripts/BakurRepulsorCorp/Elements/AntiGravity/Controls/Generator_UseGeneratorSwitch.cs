namespace BakurRepulsorCorp
{

    public class Generator_UseGeneratorSwitch : Switch<AntiGravityGenerator>
    {

        public Generator_UseGeneratorSwitch() : base("Generator_UseGeneratorSwitch", "Use Generator", "Use Generator (On/Off)", "On", "Off")
        {
        }

        protected override bool GetValue(AntiGravityGenerator equipment)
        {
            return equipment.useGenerator;
        }

        protected override void SetValue(AntiGravityGenerator equipment, bool value)
        {
            equipment.useGenerator = value;
        }
    }
}
