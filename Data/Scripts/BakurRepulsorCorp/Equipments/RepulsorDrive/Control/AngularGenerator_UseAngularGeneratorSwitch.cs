namespace BakurRepulsorCorp {

    public class AngularGenerator_UseAngularGeneratorSwitch : 
        SwitchControl<RepulsorAngularGenerator> {

        public AngularGenerator_UseAngularGeneratorSwitch() : base("AngularGenerator_UseAngularGeneratorSwitch", "Use Angular Generator", "Use Angular Generator (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorAngularGenerator equipment) {
            return equipment.component.enabled && equipment.useAngularGenerator;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, bool value) {
            equipment.useAngularGenerator = value;
        }
    }
}