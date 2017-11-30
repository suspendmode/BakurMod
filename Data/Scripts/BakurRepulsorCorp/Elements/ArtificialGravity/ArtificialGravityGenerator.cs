using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI;
using VRageMath;
using VRage.ModAPI;
using VRage.Game.Components;

namespace BakurRepulsorCorp {

    public class ArtificialGravityGenerator : LogicElement {

        static Separator<ArtificialGravityGenerator> separator;
        static Label<ArtificialGravityGenerator> label;

        public ArtificialGravityGenerator(LogicComponent block) : base(block) {
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo) {

        }

        #region gravity

        static ArtificialGravityGenerator_GravitySlider gravitySlider;
        static ArtificialGravityGenerator_IncraseGravityAction incraseGravityAction;
        static ArtificialGravityGenerator_DecraseGravityAction decraseGravityAction;

        public static string GRAVITY_PROPERTY_NAME = "ArtificialGravityGenerator_Gravity";

        public double defaultGravity = 1;

        public double gravity
        {
            set
            {
                string id = GeneratatePropertyId(GRAVITY_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratatePropertyId(GRAVITY_PROPERTY_NAME);
                double result = defaultGravity;
                if (GetVariable<double>(id, out result)) {
                    return result;
                }
                return defaultGravity;
            }
        }

        #endregion

        #region max radius

        static ArtificialGravityGenerator_MaxRadiusSlider maxRadiusSlider;
        static ArtificialGravityGenerator_IncraseMaxRadiusAction incraseMaxRadiusAction;
        static ArtificialGravityGenerator_DecraseMaxRadiusAction decraseMaxRadiusAction;

        public static string MAX_RADIUS_PROPERTY_NAME = "ArtificialGravityGenerator_MaxRadius";

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

        public BoundingSphereD boundingSphere;
        public List<IMyEntity> entitiesInRange = new List<IMyEntity>();

        public void UpdateGenerator(double physicsDeltaTime) {
            
            if (boundingSphere == null) {
                boundingSphere = new BoundingSphereD();
            }

            boundingSphere.Center = block.WorldMatrix.Translation;
            boundingSphere.Radius = maxRadius;

            entitiesInRange.Clear();
            entitiesInRange.AddRange(MyAPIGateway.Entities.GetEntitiesInSphere(ref boundingSphere));

            foreach (IMyEntity entity in entitiesInRange) {

                MyPhysicsComponentBase physics = entity.Physics;

                if (physics == null || physics == logicComponent.block.Physics || physics == logicComponent.block.CubeGrid.Physics) {
                    continue;
                }

                Vector3D direction = entity.WorldMatrix.Translation - logicComponent.block.CubeGrid.WorldMatrix.Translation;
                double distance = direction.Length();

                direction.Normalize();

                Vector3D desiredUp = logicComponent.rigidbody.gravityUp;

                Vector3D force = desiredUp * (logicComponent.rigidbody.gravity.Length() * gravity) * physics.Mass;

                physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, force, null, null);
            }
        }

        public override void Debug() {
            if (!logicComponent.debugEnabled) {
                return;
            }

            foreach (IMyEntity entity in entitiesInRange) {
                MyPhysicsComponentBase physics = entity.Physics;

                if (physics == null || physics == logicComponent.block.Physics || physics == logicComponent.block.CubeGrid.Physics) {
                    continue;
                }

                Vector3 from = entity.WorldMatrix.Translation;
                Vector3 to = entity.WorldMatrix.Translation - physics.Gravity.Length();
                DebugDraw.DrawLine(from, to, Color.DarkKhaki, 0.1f);
            }
        }

        public override void Initialize() {

            if (separator == null) {
                separator = new Separator<ArtificialGravityGenerator>("ArtificialGravityGenerator_Separator");
                separator.Initialize();
            }

            if (label == null) {
                label = new Label<ArtificialGravityGenerator>("ArtificialGravityGenerator_Label", "Artificial Gravity Generator");
                label.Initialize();
            }


            // max force

            if (gravitySlider == null) {
                gravitySlider = new ArtificialGravityGenerator_GravitySlider();
                gravitySlider.Initialize();
            }

            if (incraseGravityAction == null) {
                incraseGravityAction = new ArtificialGravityGenerator_IncraseGravityAction();
                incraseGravityAction.Initialize();
            }

            if (decraseGravityAction == null) {
                decraseGravityAction = new ArtificialGravityGenerator_DecraseGravityAction();
                decraseGravityAction.Initialize();
            }

            // max radius            

            if (maxRadiusSlider == null) {
                maxRadiusSlider = new ArtificialGravityGenerator_MaxRadiusSlider();
                maxRadiusSlider.Initialize();
            }

            if (incraseMaxRadiusAction == null) {
                incraseMaxRadiusAction = new ArtificialGravityGenerator_IncraseMaxRadiusAction();
                incraseMaxRadiusAction.Initialize();
            }

            if (decraseMaxRadiusAction == null) {
                decraseMaxRadiusAction = new ArtificialGravityGenerator_DecraseMaxRadiusAction();
                decraseMaxRadiusAction.Initialize();
            }
        }

        public override void Destroy() {
            Clear();
        }


        void Clear() {
            entitiesInRange.Clear();
        }

    }
}