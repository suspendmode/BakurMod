namespace BakurRepulsorCorp {

    public class LinearGenerator_UseLinearGeneratorSwitch : SwitchControl<RepulsorLinearGenerator> {

        public LinearGenerator_UseLinearGeneratorSwitch() : base("LinearGenerator_UseLinearGeneratorSwitch", "Use Linear Generator", "Use Linear Generator (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorLinearGenerator equipment) {
            return equipment.component.enabled && equipment.useLinearGenerator;
        }

        protected override void SetValue(RepulsorLinearGenerator equipment, bool value) {
            equipment.useLinearGenerator = value;
        }
    }
}