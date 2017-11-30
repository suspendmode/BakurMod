using Sandbox.ModAPI;

namespace BakurRepulsorCorp {

    public class Compensator_UseYawSwitch : Switch<AngularInertialCompensator> {

        public Compensator_UseYawSwitch() : base("Compensator_UseYawSwitch", "Use Yaw", "Use Yaw (false/true)", "On", "Off") {
        }

        protected override bool GetValue(AngularInertialCompensator equipment) {
            return equipment.useYaw;
        }

        protected override void SetValue(AngularInertialCompensator equipment, bool value) {
            equipment.useYaw = value;
        }

        protected override bool Visible(IMyTerminalBlock block) { if (!base.Visible(block)) {return false;}
            AngularInertialCompensator equipment = GetEquipment(block);
            if (equipment == null) {
                return false;
            }
            return equipment.useAngularCompensator;
        }
    }
}