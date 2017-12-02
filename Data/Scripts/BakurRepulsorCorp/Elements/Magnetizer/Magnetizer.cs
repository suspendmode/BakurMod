using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class Magnetizer : LogicElement
    {

        public Magnetizer(LogicComponent component) : base(component)
        {
        }

        protected double desiredForce = 0;

        #region use magnetizer

        public readonly string USE_MAGNETIZER_PROPERTY_NAME = "Magnetizer_Use";

        public bool defaultUseMagnetizer = true;

        public bool useMagnetizer
        {
            set
            {
                string id = GeneratePropertyId(USE_MAGNETIZER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_MAGNETIZER_PROPERTY_NAME);
                bool result = defaultUseMagnetizer;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseMagnetizer;
            }

        }

        #endregion

        #region normalized force

        public readonly string NORMALIZED_FORCE_PROPERTY_NAME = "Magnetizer_NormalizedForce";

        public double defaultNormalizedForce = 1;

        public double normalizedForce
        {
            set
            {
                string id = GeneratePropertyId(NORMALIZED_FORCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(NORMALIZED_FORCE_PROPERTY_NAME);
                double result = defaultNormalizedForce;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultNormalizedForce;
            }

        }

        #endregion

        #region max force

        public readonly string MAX_FORCE_PROPERTY_NAME = "Magnetizer_MaxForce";

        public double defaultMaxForce = 10000;

        public double maxForce
        {
            set
            {
                string id = GeneratePropertyId(MAX_FORCE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MAX_FORCE_PROPERTY_NAME);
                double result = defaultMaxForce;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxForce;
            }
        }

        #endregion

        #region min radius

        public readonly string MIN_RADIUS_PROPERTY_NAME = "Magnetizer_MinRadius";

        public double defaultMinRadius = 10;

        public double minRadius
        {
            set
            {
                string id = GeneratePropertyId(MIN_RADIUS_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MIN_RADIUS_PROPERTY_NAME);
                double result = defaultMinRadius;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMinRadius;
            }
        }

        #endregion

        #region max radius

        public readonly string MAX_RADIUS_PROPERTY_NAME = "Magnetizer_MaxRadius";

        public double defaultMaxRadius = 100;

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

        #region lifecycle

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

        #endregion

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
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

        public void UpdateMagnetizer(double physicsDeltaTime)
        {

            if (boundingSphere == null)
            {
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
            foreach (IMyEntity entity in entitiesInRange)
            {

                MyPhysicsComponentBase physics = entity.Physics;

                if (physics == null || physics.IsStatic || physics.IsKinematic || physics.Enabled == false || physics == logicComponent.block.Physics || physics == logicComponent.block.CubeGrid.Physics)
                {
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

                physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, magnetForce, Vector3D.Zero, Vector3D.Zero);
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

                Vector3 from = logicComponent.block.WorldMatrix.Translation;
                Vector3 to = entity.WorldMatrix.Translation;
                DebugDraw.DrawLine(from, to, Color.LightSeaGreen, 0.1f);
            }
        }
    }
}
