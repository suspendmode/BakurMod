namespace BakurRepulsorCorp {

    public class AngularGenerator_UseAngularGeneratorSwitch : 
        Switch<RepulsorAngularGenerator> {

        public AngularGenerator_UseAngularGeneratorSwitch() : base("AngularGenerator_UseAngularGeneratorSwitch", "Use Angular Generator", "Use Angular Generator (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorAngularGenerator equipment) {
            return equipment.logicComponent.enabled && equipment.useAngularGenerator;
        }

        protected override void SetValue(RepulsorAngularGenerator equipment, bool value) {
            equipment.useAngularGenerator = value;
        }
    }
}