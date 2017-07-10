﻿namespace BakurRepulsorCorp {

    public class PointLift_UsePointLiftSwitch : SwitchControl<RepulsorPointLift> {

        public PointLift_UsePointLiftSwitch() : base("RepulsorPointLift_UsePointLift_UsePointLiftSwitch", "Use Repulsor Point Lift", "Use Repulsor Point Lift (false/true)", "On", "Off") {
        }

        protected override bool GetValue(RepulsorPointLift equipment) {
            return equipment.component.enabled && equipment.usePointLift;
        }

        protected override void SetValue(RepulsorPointLift equipment, bool value) {
            equipment.usePointLift = value;
        }
    }
}