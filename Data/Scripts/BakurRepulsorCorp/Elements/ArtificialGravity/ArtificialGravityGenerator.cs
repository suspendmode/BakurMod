using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class ArtificialGravityGenerator : LogicElement
    {

        public ArtificialGravityGenerator(LogicComponent block) : base(block)
        {
        }

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {

        }

        #region gravity

        public readonly string GRAVITY_PROPERTY_NAME = "ArtificialGravityGenerator_Gravity";

        public double defaultGravity = 1;

        public double gravity
        {
            set
            {
                string id = GeneratePropertyId(GRAVITY_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(GRAVITY_PROPERTY_NAME);
                double result = defaultGravity;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultGravity;
            }
        }

        #endregion

        #region max radius

        public readonly string MAX_RADIUS_PROPERTY_NAME = "ArtificialGravityGenerator_MaxRadius";

        public double defaultMaxRadius = 10;

        public double maxRadius
        {
            set
            {
                string id = GeneratePropertyId(MAX_RADIUS_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MAX_RADIUS_PROPERTY_NAME);
                double result = defaultMaxRadius;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxRadius;
            }
        }

        #endregion

        public BoundingSphereD boundingSphere;
        public List<IMyEntity> entitiesInRange = new List<IMyEntity>();

        public void UpdateGenerator(double physicsDeltaTime)
        {

            if (boundingSphere == null)
            {
                boundingSphere = new BoundingSphereD();
            }

            boundingSphere.Center = block.WorldMatrix.Translation;
            boundingSphere.Radius = maxRadius;

            entitiesInRange.Clear();
            entitiesInRange.AddRange(MyAPIGateway.Entities.GetEntitiesInSphere(ref boundingSphere));

            foreach (IMyEntity entity in entitiesInRange)
            {

                MyPhysicsComponentBase physics = entity.Physics;

                if (physics == null || physics == logicComponent.block.Physics || physics == logicComponent.block.CubeGrid.Physics)
                {
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

        public override void Debug()
        {
            if (!logicComponent.debugEnabled)
            {
                return;
            }

            foreach (IMyEntity entity in entitiesInRange)
            {
                MyPhysicsComponentBase physics = entity.Physics;

                if (physics == null || physics == logicComponent.block.Physics || physics == logicComponent.block.CubeGrid.Physics)
                {
                    continue;
                }

                Vector3 from = entity.WorldMatrix.Translation;
                Vector3 to = entity.WorldMatrix.Translation - physics.Gravity.Length();
                DebugDraw.DrawLine(from, to, Color.DarkKhaki, 0.1f);
            }
        }

        public override void Initialize()
        {

        }

        public override void Destroy()
        {
            Clear();
        }


        void Clear()
        {
            entitiesInRange.Clear();
        }

    }
}
