using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class Storm
    {

        Vector3D center;
        Vector3D position;
        Vector3D forward;


        public event Action<Storm> EndedEvent;

        float minSpeed = -0.0001f;
        float maxSpeed = 0.0001f;

        float size = 100;

        float pitchSpeed = 0;
        float yawSpeed = 0;
        float rollSpeed = 0;

        public void Update()
        {

            //MyAPIGateway.Utilities.ShowMessage("Storms", storms.Count + " storms");

            //DebugDraw.Line(center, position, Color.Red, 10);
            //DebugDraw.Line(center, center + Vector3D.Forward * radius * 2, Color.Yellow, 10);

            //DebugDraw.DrawSphere(position, Vector3D.Forward, Vector3.Up, Color.Red, 1000);
            //DebugDraw.Line(position, MyAPIGateway.Session.Player.GetPosition(), Color.Green, 10);
            //DebugDraw.Line(center, MyAPIGateway.Session.Camera.Position, Color.Green, 0.01f);

            orientation.X += pitchSpeed;
            orientation.Y += yawSpeed;
            orientation.Z += rollSpeed;
            orientation.X = MyMath.NormalizeAngle(orientation.X);
            orientation.Y = MyMath.NormalizeAngle(orientation.Y);
            orientation.Z = MyMath.NormalizeAngle(orientation.Z);
            UpdateNormalAndPosition();

            //MyAPIGateway.Utilities.ShowMessage("orientation", ((Vector3D)orientation * BakurMathHelper.Rad2Deg) + ", " + radius);
            //MyAPIGateway.Utilities.ShowMessage("storm", center + ", " + position);

            UpdateParticlesPosition();
            UpdateForces();
        }

        BoundingSphereD boundingSphere = new BoundingSphereD();

        void UpdateForces()
        {
            boundingSphere.Center = center;
            boundingSphere.Radius = radius;

            List<IMyEntity> entities = MyAPIGateway.Entities.GetEntitiesInSphere(ref boundingSphere);
            foreach (IMyEntity entity in entities)
            {

                MyPhysicsComponentBase physics = null;

                if (entity is IMyCubeGrid)
                {
                    IMyCubeGrid grid = entity as IMyCubeGrid;
                    if (grid.Physics == null || grid.Physics.IsStatic || grid.Physics.IsKinematic || !grid.Physics.Enabled)
                    {
                        continue;
                    }
                    physics = grid.Physics;
                }
                else if (entity is IMyFloatingObject)
                {
                    IMyFloatingObject obj = entity as IMyFloatingObject;
                    if (obj.Physics == null || obj.Physics.IsStatic || obj.Physics.IsKinematic || !obj.Physics.Enabled)
                    {
                        continue;
                    }
                    physics = obj.Physics;
                }
                else if (entity is IMyCharacter)
                {
                    IMyCharacter obj = entity as IMyCharacter;
                    if (obj.Physics == null || obj.Physics.IsStatic || obj.Physics.IsKinematic || !obj.Physics.Enabled)
                    {
                        continue;
                    }
                    physics = obj.Physics;

                }
                else
                {
                    continue;
                }

                if (physics == null)
                {
                    continue;
                }

                Vector3 distance = entity.GetPosition() - position;
                double normalizedDistance = 1 - BakurMathHelper.Clamp01(distance.Length() / size);
                Vector3 windVelocity = forward * (size / 3.6f / 100000) * normalizedDistance;
                double windForce = 0.5 * 1.2 * Math.Pow(windVelocity.Length(), 2) * entity.WorldAABB.Size.Length();

                double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
                MatrixD matrix = MatrixD.CreateFromYawPitchRoll(orientation.X, orientation.Y, orientation.Z);
                Vector3D direction = Vector3.Transform(Vector3D.Forward, matrix);
                Vector3D force = direction * physicsDeltaTime * windForce;
                //MyAPIGateway.Utilities.ShowMessage("Wind", force.Length() + " N, " + entity.Name + ", " + normalizedDistance + ", " + distance.Length());
                physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, force, null, null);
            }
        }

        void UpdateParticlesPosition()
        {

            MatrixD effectMatrix = particleEffect.WorldMatrix;
            effectMatrix.Translation = position;
            particleEffect.WorldMatrix = effectMatrix;
        }

        void UpdateNormalAndPosition()
        {
            MatrixD matrix = MatrixD.CreateFromYawPitchRoll(orientation.X, orientation.Y, orientation.Z);
            forward = Vector3.Transform(Vector3D.Forward * radius, matrix);
            position = center + forward;
        }

        Vector3 orientation = Vector3.Zero;

        double radius;

        public void Create(MyPlanet planet)
        {
            center = planet.PositionComp.GetPosition();
            radius = MyUtils.GetRandomDouble(planet.AverageRadius, planet.MaximumRadius);
            size = MyUtils.GetRandomFloat(500, 1000);

            orientation.X = (float)MyUtils.GetRandomDouble(-MathHelper.Pi, MathHelper.Pi);
            orientation.Y = (float)MyUtils.GetRandomDouble(-MathHelper.Pi, MathHelper.Pi);
            orientation.Z = (float)MyUtils.GetRandomDouble(-MathHelper.Pi, MathHelper.Pi);

            pitchSpeed = (float)MyUtils.GetRandomDouble(minSpeed, maxSpeed);
            yawSpeed = (float)MyUtils.GetRandomDouble(minSpeed, maxSpeed);
            rollSpeed = (float)MyUtils.GetRandomDouble(minSpeed, maxSpeed);

            UpdateNormalAndPosition();
            CreateParticleEffect(planet);
            UpdateParticlesPosition();
            boundingSphere = new BoundingSphereD(position, radius);
        }

        MyParticleEffect particleEffect;

        void CreateParticleEffect(MyPlanet planet)
        {

            int particle = 9999;

            MyParticlesManager.TryCreateParticleEffect(particle, out particleEffect);
            MatrixD effectMatrix = MatrixD.CreateWorld(position, Vector3D.CalculatePerpendicularVector(forward), forward);
            effectMatrix.Translation = position;

            //particleEffect.Velocity = block.CubeGrid.Physics.LinearVelocity;
            //particleEffect.WorldMatrix = effectMatrix;

            //particleEffect.set = planet.Render.NearFlag;

            particleEffect.UserScale = 5;
            //particleEffect.EnableLods = true;
            //particleEffect.UserBirthMultiplier = 0.5f;
            particleEffect.UserRadiusMultiplier = size;
            particleEffect.WorldMatrix = effectMatrix;
            particleEffect.Play();
        }
    }

    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class Storms : MySessionComponentBase
    {
        /*
        public int maxStorms = 2;
        public int stormDuration = 10;
        public List<Storm> storms = new List<Storm>();

        public List<MyPlanet> planets = new List<MyPlanet>();

        bool initialized;

        public override void UpdateAfterSimulation() {

            if (!initialized) {
                if (MyAPIGateway.Session == null)
                    return;
                initialized = true;
            } else {
                UpdatePlanets();
                UpdateStorms();
            }
        }

        void UpdateStorms() {
            if (storms.Count < maxStorms && planets.Count > 0) {
                CreateStorm();
            }
            foreach (Storm storm in storms) {
                storm.Update();
            }
        }

        void CreateStorm() {
            Storm storm = new Storm();
            storm.EndedEvent += OnStormEnded;
            storm.Create(GetRandomPlanet());
            storms.Add(storm);
        }

        MyPlanet GetRandomPlanet() {
            int index = MyUtils.GetRandomInt(planets.Count - 1);
            return planets[index];
        }

        void OnStormEnded(Storm storm) {
            storm.EndedEvent -= OnStormEnded;
            storms.Remove(storm);
        }

        HashSet<IMyEntity> entitiesHelper = new HashSet<IMyEntity>();

        void UpdatePlanets() {
            MyAPIGateway.Entities.GetEntities(entitiesHelper, delegate (IMyEntity entity) {
                if (entity is MyPlanet) {
                    MyPlanet planet = (MyPlanet)entity;
                    if (!planets.Contains(planet) && planet.HasAtmosphere) {
                        planets.Add(planet);
                    }
                    if (planet.Closed || planet.MarkedForClose) {
                        planets.Remove(planet);
                    }
                }
                return false;
            });
        }
        */
    }
}
