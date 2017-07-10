using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp {

    public class RepulsorCoil : BakurBlockEquipment {

        public RepulsorCoil(BakurBlock component) : base(component, double.PositiveInfinity) {
        }

        static Separator<RepulsorCoil> coilSeparator;
        static Label<RepulsorCoil> coilLabel;

        public double maxDistance;
        public int enabledCoilsCount;

        #region use coil

        static Coil_UseCoilSwitch useCoilSwitch;
        static Coil_UseCoilToggleAction useCoilToggleAction;
        static Coil_UseCoilEnableAction useCoilEnableAction;
        static Coil_UseCoilDisableAction useCoilDisableAction;

        public static string USE_COIL_PROPERTY_NAME = "Coil_UseCoil";

        public bool defaultUseCoil = true;

        public bool useCoil
        {
            set
            {
                string id = GeneratatePropertyId(USE_COIL_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(USE_COIL_PROPERTY_NAME);
                bool result = defaultUseCoil;
                if (GetVariable<bool>(id, out result)) {
                    return result;
                }
                return defaultUseCoil;
            }
        }


        #endregion

        #region tension

        static Coil_TensionSlider tensionSlider;
        static Coil_IncraseTensionAction incraseTensionAction;
        static Coil_DecraseTensionAction decraseTensionAction;

        public static string TENSION_PROPERTY_NAME = "RepulsorCoil_Tension";

        public double defaultTension = 1;

        public double tension
        {
            set
            {
                string id = GeneratatePropertyId(TENSION_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(TENSION_PROPERTY_NAME);
                double result = defaultTension;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultTension;
            }

        }

        #endregion

        #region lifecycle

        public override void Initialize() {

            if (coilSeparator == null) {
                coilSeparator = new Separator<RepulsorCoil>("Coil_CoilSeparator");
                coilSeparator.Initialize();
            }

            if (coilLabel == null) {
                coilLabel = new Label<RepulsorCoil>("Coil_CoilLabel", "Repulsor Coil");
                coilLabel.Initialize();
            }

            // use repulsor coil

            if (useCoilSwitch == null) {
                useCoilSwitch = new Coil_UseCoilSwitch();
                useCoilSwitch.Initialize();
            }

            if (useCoilToggleAction == null) {
                useCoilToggleAction = new Coil_UseCoilToggleAction();
                useCoilToggleAction.Initialize();
            }

            if (useCoilEnableAction == null) {
                useCoilEnableAction = new Coil_UseCoilEnableAction();
                useCoilEnableAction.Initialize();
            }

            if (useCoilDisableAction == null) {
                useCoilDisableAction = new Coil_UseCoilDisableAction();
                useCoilDisableAction.Initialize();
            }

            // tension

            if (tensionSlider == null) {
                tensionSlider = new Coil_TensionSlider();
                tensionSlider.Initialize();
            }
            if (incraseTensionAction == null) {
                incraseTensionAction = new Coil_IncraseTensionAction();
                incraseTensionAction.Initialize();
            }
            if (decraseTensionAction == null) {
                decraseTensionAction = new Coil_DecraseTensionAction();
                decraseTensionAction.Initialize();
            }

            RepulsorCoilSession.AddRepulsorCoil(block.CubeGrid, this);
            base.Initialize();
        }

        public override void Destroy() {
            Clear();
            RepulsorCoilSession.RemoveRepulsorCoil(block.CubeGrid, this);
            base.Destroy();
        }

        public void Clear() {
            tension = defaultTension;
            useCoil = defaultUseCoil;
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            IMyCubeGrid grid = block.CubeGrid;
            customInfo.AppendLine();
            customInfo.AppendLine("== Repulsor Coil ==");
            customInfo.AppendLine("Use : " + (useCoil ? "On" : "Off"));
            customInfo.AppendLine("Tension : " + Math.Round(tension, 1));
            customInfo.AppendLine("Gravity : " + Math.Round(component.gravity.Length(), 1));
            customInfo.AppendLine("Grid Mass : " + Math.Round(block.CubeGrid.Physics.Mass, 1) + " kg");
        }

        #endregion

        public override bool isModifierEnabled => useCoil;

        public override void GetLinearAcceleration(double physicsDeltaTime, out Vector3D acceleration, out Vector3D position) {
            position = Vector3D.Zero;
            acceleration = (component.gravityUp / (double)enabledCoilsCount) * tension;
            Store(acceleration, position);
        }
    }
}