using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class Rigidbody
    {

        BakurBlock bakurBlock;

        public Rigidbody(BakurBlock bakurBlock)
        {
            this.bakurBlock = bakurBlock;
        }

        public IMyCubeGrid grid
        {
            get
            {
                return bakurBlock.block == null ? null : bakurBlock.block.CubeGrid;
            }
        }

        #region gravity

        public double gridMass
        {
            get
            {
                return grid == null ? 0 : grid.Physics == null ? 0 : grid.Physics.Mass;
            }
        }

        public Vector3D gravity
        {
            get
            {
                return grid == null ? Vector3D.Zero : grid.Physics == null ? Vector3D.Zero : (Vector3D)grid.Physics.Gravity;
            }
        }

        public Vector3D gravityUp
        {
            get
            {
                Vector3D up = -gravity;
                up.Normalize();
                return up;
            }
        }

        public Vector3D gravityDown
        {
            get
            {
                Vector3D up = gravity;
                up.Normalize();
                return up;
            }
        }

        public bool IsInGravity
        {
            get
            {
                return gravity.LengthSquared() > MyMathConstants.EPSILON_SQUARED;
            }
        }

        #endregion

        #region force

        public void AddLinearVelocity(Vector3D velocity)
        {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddLinearAcceleration(acceleration);
        }

        public void AddLinearVelocity(Vector3D velocity, Vector3D point)
        {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddLinearAcceleration(acceleration, point);
        }

        public void AddAngularVelocity(Vector3D velocity)
        {
            double physicsDeltaTime = MyEngineConstants.PHYSICS_STEP_SIZE_IN_SECONDS;
            Vector3D acceleration = velocity / physicsDeltaTime;
            AddAngularAcceleration(acceleration);
        }

        public void AddLinearAcceleration(Vector3D acceleration)
        {

            double mass = grid.Physics.Mass;
            AddForce(acceleration * mass);
        }

        public void AddLinearAcceleration(Vector3D acceleration, Vector3D point)
        {

            double mass = grid.Physics.Mass;
            AddForce(acceleration * mass, point);
        }

        public void AddAngularAcceleration(Vector3D acceleration)
        {

            //Vector3D pseudoInertiaTensor = grid.Physics.Mass * grid.WorldAABB.Extents;

            double size = grid.GridSizeEnum == MyCubeSize.Large ? 2.5 : 0.5;

            Vector3D pseudoInertiaTensor = 0.5f * grid.Physics.Mass * grid.WorldAABB.Size.Length() * size * Vector3D.One;
            // MyAPIGateway.Utilities.ShowMessage("Torque", "Extents: " + Math.Round(tensor.X, 2) + "," + Math.Round(tensor.Y, 2) + "," + Math.Round(tensor.Z, 2) + ", " + Math.Round(tensor.Length(), 2));

            AddTorque(acceleration * BakurMathHelper.Deg2Rad * pseudoInertiaTensor);
        }

        public void AddForce(Vector3D force)
        {
            if (double.IsNaN(force.X) || double.IsNaN(force.Y) || double.IsNaN(force.Z) || force == Vector3.Zero)
            {
                return;
            }
            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localForce = Vector3D.Transform(force, invWorldRot);
            grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, localForce, grid.Physics.CenterOfMassWorld, null);
        }

        public void AddForce(Vector3D force, Vector3D position)
        {
            if (double.IsNaN(force.X) || double.IsNaN(force.Y) || double.IsNaN(force.Z) || force == Vector3.Zero)
            {
                return;
            }

            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localForce = Vector3D.Transform(force, invWorldRot);
            grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, localForce, position, null);
        }

        public void AddTorque(Vector3D torque)
        {
            if (double.IsNaN(torque.X) || double.IsNaN(torque.Y) || double.IsNaN(torque.Z) || torque == Vector3.Zero)
            {
                return;
            }

            MatrixD invWorldRot = grid.PositionComp.WorldMatrixInvScaled.GetOrientation();
            Vector3D localTorque = Vector3D.Transform(torque, invWorldRot);

            // MyAPIGateway.Utilities.ShowMessage("Torque", "torque: " + Math.Round(localTorque.X, 2) + "," + Math.Round(localTorque.Y, 2) + "," + Math.Round(localTorque.Z, 2) + ", " + Math.Round(localTorque.Length(), 2));

            grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, null, grid.Physics.CenterOfMassWorld, localTorque);
        }

        #endregion

    }
}
