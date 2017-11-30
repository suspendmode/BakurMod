using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using Havok;
using VRage.Game.Components;

namespace BakurRepulsorCorp {

    public class Magnetizer : LogicElement {

        public Magnetizer(LogicComponent component) : base(component) {
        }

        static Separator<Magnetizer> magnetizerSeparator;
        static Label<Magnetizer> magnetizerLabel;

        protected double desiredForce = 0;

        #region normalized force

        static Magnetizer_NormalizedForceSlider normalizedForceSlider;
        static Magnetizer_IncraseNormalizedForceAction incraseNormalizedForceAction;
        static Magnetizer_DecraseNormalizedForceAction decraseNormalizedForceAction;

        public static string NORMALIZED_FORCE_PROPERTY_NAME = "Magnetizer_NormalizedForce";

        public double defaultNormalizedForce = 0;

        public double normalizedForce
        {
            set
            {
                string id = GeneratatePropertyId(NORMALIZED_FORCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(NORMALIZED_FORCE_PROPERTY_NAME);
                double result = defaultNormalizedForce;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultNormalizedForce;
            }

        }

        #endregion

        #region max force

        static Magnetizer_MaxForceSlider maxForceSlider;
        static Magnetizer_IncraseMaxForceAction incraseMaxForceAction;
        static Magnetizer_DecraseMaxForceAction decraseMaxForceAction;

        public static string MAX_FORCE_PROPERTY_NAME = "Magnetizer_MaxForce";

        public double defaultMaxForce = Magnetizer_MaxForceSlider.maxForce;

        public double maxForce
        {
            set
            {
                string id = GeneratatePropertyId(MAX_FORCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MAX_FORCE_PROPERTY_NAME);
                double result = defaultMaxForce;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultMaxForce;
            }
        }

        #endregion

        #region min radius

        static Magnetizer_MinRadiusSlider minRadiusSlider;
        static Magnetizer_IncraseMinRadiusAction incraseMinRadiusAction;
        static Magnetizer_DecraseMinRadiusAction decraseMinRadiusAction;

        public static string MIN_RADIUS_PROPERTY_NAME = "Magnetizer_MinRadius";

        public double defaultMinRadius = Magnetizer_MinRadiusSlider.minRadius;

        public double minRadius
        {
            set
            {
                string id = GeneratatePropertyId(MIN_RADIUS_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MIN_RADIUS_PROPERTY_NAME);
                double result = defaultMinRadius;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultMinRadius;
            }
        }

        #endregion

        #region max radius

        static Magnetizer_MaxRadiusSlider maxRadiusSlider;
        static Magnetizer_IncraseMaxRadiusAction incraseMaxRadiusAction;
        static Magnetizer_DecraseMaxRadiusAction decraseMaxRadiusAction;

        public static string MAX_RADIUS_PROPERTY_NAME = "Magnetizer_MaxRadius";

        public double defaultMaxRadius = 10;

        public double maxRadius
        {
            set
            {
                string id = GeneratatePropertyId(MAX_RADIUS_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(MAX_RADIUS_PROPERTY_NAME);
                double result = defaultMaxRadius;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultMaxRadius;
            }
        }

        #endregion

        #region lifecycle

        public override void Initialize() {

            if (magnetizerSeparator == null) {
                magnetizerSeparator = new Separator<Magnetizer>("Magnetizer_magnetizerSeparator");
                magnetizerSeparator.Initialize();
            }

            if (magnetizerLabel == null) {
                magnetizerLabel = new Label<Magnetizer>("Magnetizer_MagnetizerLabel", "Magnetizer");
                magnetizerLabel.Initialize();
            }

            // normalized force

            if (normalizedForceSlider == null) {
                normalizedForceSlider = new Magnetizer_NormalizedForceSlider();
                normalizedForceSlider.Initialize();
            }

            if (incraseNormalizedForceAction == null) {
                incraseNormalizedForceAction = new Magnetizer_IncraseNormalizedForceAction();
                incraseNormalizedForceAction.Initialize();
            }

            if (decraseNormalizedForceAction == null) {
                decraseNormalizedForceAction = new Magnetizer_DecraseNormalizedForceAction();
                decraseNormalizedForceAction.Initialize();
            }

            // max force

            if (maxForceSlider == null) {
                maxForceSlider = new Magnetizer_MaxForceSlider();
                maxForceSlider.Initialize();
            }

            if (incraseMaxForceAction == null) {
                incraseMaxForceAction = new Magnetizer_IncraseMaxForceAction();
                incraseMaxForceAction.Initialize();
            }

            if (decraseMaxForceAction == null) {
                decraseMaxForceAction = new Magnetizer_DecraseMaxForceAction();
                decraseMaxForceAction.Initialize();
            }

            // min radius            

            if (minRadiusSlider == null) {
                minRadiusSlider = new Magnetizer_MinRadiusSlider();
                minRadiusSlider.Initialize();
            }

            if (incraseMinRadiusAction == null) {
                incraseMinRadiusAction = new Magnetizer_IncraseMinRadiusAction();
                incraseMinRadiusAction.Initialize();
            }

            if (decraseMinRadiusAction == null) {
                decraseMinRadiusAction = new Magnetizer_DecraseMinRadiusAction();
                decraseMinRadiusAction.Initialize();
            }

            // max radius            

            if (maxRadiusSlider == null) {
                maxRadiusSlider = new Magnetizer_MaxRadiusSlider();
                maxRadiusSlider.Initialize();
            }

            if (incraseMaxRadiusAction == null) {
                incraseMaxRadiusAction = new Magnetizer_IncraseMaxRadiusAction();
                incraseMaxRadiusAction.Initialize();
            }

            if (decraseMaxRadiusAction == null) {
                decraseMaxRadiusAction = new Magnetizer_DecraseMaxRadiusAction();
                decraseMaxRadiusAction.Initialize();
            }

        }

        public override void Destroy() {
            Clear();
        }


        void Clear() {
            entitiesInRange.Clear();
        }

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {
            customInfo.AppendLine();
            customInfo.AppendLine("== Magnetizer ==");
            customInfo.AppendLine("Radius : " + Math.Round(maxRadius, 1) + " m");
            customInfo.AppendLine("Force : " + Math.Round(desiredForce / 1000, 1) + " kN");
            customInfo.AppendLine("Entities In Range : " + entitiesInRange.Count);
            //customInfo.AppendLine("Filter : " + Math.Round(normalizedForce * 100, 1) + " %");            
        }

        #endregion


        public BoundingSphereD boundingSphere;
        public List<IMyEntity> entitiesInRange = new List<IMyEntity>();

        public void UpdateMagnetizer(double physicsDeltaTime) {

            if (boundingSphere == null) {
                boundingSphere = new BoundingSphereD();
            }

            boundingSphere.Center = block.WorldMatrix.Translation;
            boundingSphere.Radius = maxRadius;

            entitiesInRange.Clear();
            entitiesInRange.AddRange(MyAPIGateway.Entities.GetEntitiesInSphere(ref boundingSphere));


            //MyAPIGateway.Utilities.ShowMessage("Magnetizer", "entitiesInRange: " + entitiesInRange.Count);
            //return;
            desiredForce = normalizedForce * maxForce;

            //MyAPIGateway.Physics.GetCollisionLayer()
            //  MyAPIGateway.Physics.
            foreach (IMyEntity entity in entitiesInRange) {

                MyPhysicsComponentBase physics = entity.Physics;

                if (physics == null || physics.IsStatic || physics.IsKinematic || physics.Enabled == false || physics == logicComponent.block.Physics || physics == logicComponent.block.CubeGrid.Physics) {
                    continue;
                }

                //MyAPIGateway.Utilities.ShowMessage("Magnetizer", "Entity: " + grid.Name);
                //int layer = entity.Physics.RigidBody.Layer;
                Vector3D direction = entity.WorldMatrix.Translation - logicComponent.block.CubeGrid.WorldMatrix.Translation;
                double distance = direction.Length();

                direction.Normalize();
                //double mass1 = physics.Mass / 100;
                //double mass2 = component.block.CubeGrid.Physics.Mass / 100;

                Vector3D magnetForce = direction * desiredForce * BakurMathHelper.InverseLerp(minRadius, maxRadius, distance) * physics.Mass;
               // MyAPIGateway.Utilities.ShowMessage("Magnetizer", "Entity: " + physics.Entity.Name + ", magnetForce: " + Math.Round(magnetForce.Length(), 1) + " N");
                physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, magnetForce, null, null);
            }

            //FloatingObjectCollisionLayer
            //  ObjectDetectionCollisionLayer
            // VoxelCollisionLayer
            //CollisionLayerWithoutCharacter
            //if (strLayer == "LightFloatingObjectCollisionLayer") {

            //}

        }

        //MyAPIGateway.Utilities.ShowMessage(block.BlockDefinition.SubtypeId, "altitude:" + altitude + ", desiredAltitude:" + desiredAltitude + ", output:" + output + ", maxForce: " + maxForce + ", desiredForce: " + desiredForce);
        // MyAPIGateway.Utilities.ShowMessage("RepulosrLift", "distanceError: " + Math.Round(distanceError, 1) + ", output:" + Math.Round(output, 1) + ", outputForce:" + Math.Round(outputForce.Length(), 1));


        public override void Debug() {
            if (!logicComponent.debugEnabled) {
                return;
            }

            foreach (IMyEntity entity in entitiesInRange) {
                MyPhysicsComponentBase physics = entity.Physics;

                if (physics == null || physics == logicComponent.block.Physics || physics == logicComponent.block.CubeGrid.Physics) {
                    continue;
                }

                Vector3 from = logicComponent.block.WorldMatrix.Translation;
                Vector3 to = entity.WorldMatrix.Translation;
                DebugDraw.DrawLine(from, to, Color.LightSeaGreen, 0.1f);
            }
        }
    }
}