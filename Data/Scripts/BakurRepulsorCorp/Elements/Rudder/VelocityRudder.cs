using Sandbox.ModAPI;
using System;
using System.Text;
using VRage.Game.Components;
using VRageMath;

namespace BakurRepulsorCorp
{

    public class VelocityRudder : LogicElement
    {
        public VelocityRudder(LogicComponent block) : base(block)
        {
        }

        public double minLinearVelocity = 15;

        #region max angle

        public readonly string MAX_ANGLE_PROPERTY_NAME = "VelocityRudder_MaxAngle";

        public double defaultMaxDegrees = 1;
        public double maxAngle
        {
            set
            {
                string id = GeneratePropertyId(MAX_ANGLE_PROPERTY_NAME);
                SetVariable<double>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(MAX_ANGLE_PROPERTY_NAME);
                double result = defaultMaxDegrees;
                if (GetVariable<double>(id, out result))
                {
                    return result;
                }
                return defaultMaxDegrees;
            }

        }

        #endregion

        #region use velocity rudder

        public readonly string USE_VELOCITY_RUDDER_PROPERTY_NAME = "VelocityRudder_UseVelocityRudder";

        public bool defaultUseVelocityRudder = true;

        public bool useVelocityRudder
        {
            set
            {
                string id = GeneratePropertyId(USE_VELOCITY_RUDDER_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(USE_VELOCITY_RUDDER_PROPERTY_NAME);
                bool result = defaultUseVelocityRudder;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultUseVelocityRudder;
            }

        }



        #endregion

        #region reverse gear

        public readonly string REVERSE_GEAR_PROPERTY_NAME = "VelocityRudder_ReverseGear";

        public bool defaultReverseGear = false;

        public bool reverseGear
        {
            set
            {
                string id = GeneratePropertyId(REVERSE_GEAR_PROPERTY_NAME);
                SetVariable<bool>(id, value);
            }
            get
            {
                string id = GeneratePropertyId(REVERSE_GEAR_PROPERTY_NAME);
                bool result = defaultReverseGear;
                if (GetVariable<bool>(id, out result))
                {
                    return result;
                }
                return defaultReverseGear;
            }

        }

        #endregion

        #region lifecycle

        public override void Initialize()
        {

        }

        public override void Destroy()
        {

        }

        #endregion      

        #region visuals

        public override void AppendCustomInfo(IMyTerminalBlock block, StringBuilder customInfo)
        {
            customInfo.AppendLine();
            customInfo.AppendLine("== Velocity Rudder ==");
            customInfo.AppendLine("Enabled: " + useVelocityRudder);
            customInfo.AppendLine("Reverse Gear: " + reverseGear);
            customInfo.AppendLine("Max Degrees : " + maxAngle);
        }

        public override void Debug()
        {

            if (!logicComponent.debugEnabled)
            {
                return;
            }

            Vector3 blockPosition = block.GetPosition();

            Vector3D velocity = grid.Physics.LinearVelocity;
            Vector3D velocityDirection = velocity;
            velocityDirection.Normalize();

            Quaternion from = Quaternion.CreateFromForwardUp(block.WorldMatrix.Forward, block.WorldMatrix.Up);
            Quaternion to = Quaternion.CreateFromForwardUp(grid.Physics.LinearVelocity, Vector3.Up);

            DebugDraw.DrawLine(blockPosition, blockPosition + from.Forward * 100, Color.Red, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + to.Forward * 100, Color.Green, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + grid.Physics.LinearVelocity * 100, Color.Orange, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + rotatedVelocity * 100, Color.Blue, 0.05f);
            DebugDraw.DrawLine(blockPosition, blockPosition + velocityDirection * 100, Color.Magenta, 0.05f);
        }

        #endregion

        #region update

        public void UpdateAfterSimulation(double physicsDeltaTime)
        {


            if (useVelocityRudder)
            {
                UpdateVelocityRudder(physicsDeltaTime);
            }
        }


        public static double Angle(Quaternion a, Quaternion b)
        {
            double f = Quaternion.Dot(a, b);
            return MathHelper.ToDegrees((float)Math.Acos(Math.Min(Math.Abs(f), 1)) * 2);
        }

        void UpdateVelocityRudder(double physicsDeltaTime)
        {

            if (block == null)
            {
                return;
            }

            if (grid == null || grid.Physics == null)
            {
                return;
            }

            if (grid.IsStatic)
            {
                return;
            }

            MyPhysicsComponentBase physics = grid.Physics;
            if (physics == null)
            {
                return;
            }

            if (physics.LinearVelocity.Length() <= minLinearVelocity)
            {
                return;
            }

            double angle = MathHelper.ToDegrees(MyMath.AngleBetween(block.WorldMatrix.Forward, physics.LinearVelocity));
            if (Math.Abs(angle) < VelocityRudderSettings.minimumMaxAngle || angle > VelocityRudderSettings.maximumMaxAngle)
            {
                return;
            }

            Quaternion velocityForward = Quaternion.CreateFromForwardUp(physics.LinearVelocity, Vector3.Up);
            if (velocityForward == Quaternion.Identity || velocityForward.Equals(Quaternion.Identity))
            {
                return;
            }

            Quaternion currentForward = Quaternion.CreateFromForwardUp(block.WorldMatrix.Forward, block.WorldMatrix.Up);
            if (currentForward == Quaternion.Identity || currentForward.Equals(Quaternion.Identity))
            {
                return;
            }

            // rotate
            //Quaternion step = Quaternion.Lerp(currentForward, velocityForward, 0.1f * (float)physicsDeltaTime);
            //Quaternion step = BakurMathHelper.RotateTowards(velocityForward, currentForward, maxAngle / physicsDeltaTime);
            //step -= currentForward;
            //rotatedVelocity = Vector3.Transform(grid.Physics.LinearVelocity, step);

            //Quaternion next = Quaternion.CreateFromForwardUp(Vector3 block.WorldMatrix.Forward * physics.LinearVelocity.Length(), Vector3D.Up) - currentForward;
            //Quaternion step = Quaternion.Lerp(currentForward, next, 0.05f);
            //velocityForward, 0.05f) -currentForward;
            Vector3D velocityDirection = physics.LinearVelocity;
            velocityDirection.Normalize();
            physics.LinearVelocity = Vector3.Lerp(velocityDirection, block.WorldMatrix.Forward, (float)maxAngle * (float)BakurMathHelper.Deg2Rad) * physics.LinearVelocity.Length();

        }

        Vector3D rotatedVelocity;


        #endregion
    }
}
#region old code
/*
        Vector3D predictedVelocity = Vector3D.Zero;
        predictedVelocity += grid.WorldMatrix.Right * velocity.X;
        predictedVelocity += grid.WorldMatrix.Up * velocity.Y;
        predictedVelocity += grid.WorldMatrix.Forward * velocity.Z;

        float num = Angle(from, to);
        if (num == 0f) {
            grid.Physics.LinearVelocity = velocity;
        } else {
            float t = Math.Min(1f, maxDegrees / num);
            grid.Physics.LinearVelocity = Vector3.Lerp(velocity, predictedVelocity, t);
        }

Quaternion currentForward = attachedRigidbody.rotation;
Quaternion velocityForward = velocity.sqrMagnitude > 0.001f ? Quaternion.LookRotation(velocity.normalized, up) : Quaternion.identity;
Quaternion step = Quaternion.RotateTowards(currentForward, velocityForward, velocityRotationRate * deltaTime);
velocity = Vector3.Lerp(velocity, step * Vector3.forward, velocityStep * deltaTime);

attachedRigidbody.velocity = velocity;
*/


/*
Vector3 direction = reverseGear ? block.WorldMatrix.Backward : block.WorldMatrix.Forward;

Vector3 velocity = grid.Physics.LinearVelocity * Vector3.Dot(grid.Physics.LinearVelocity, direction);
if (velocity.Equals(Vector3.Zero)) {
    return;
}

Vector3 velocityDirection = grid.Physics.LinearVelocity;
velocityDirection.Normalize();
velocityDirection *= Vector3.Dot(velocityDirection, direction);

if (velocityDirection.Equals(Vector3.Zero)) {
    return;
}
*/
//Matrix current = grid.WorldMatrix.GetOrientation();

//Quaternion currentForward = attachedRigidbody.rotation;
//Quaternion velocityForward = velocity.sqrMagnitude > 0.001f ? Quaternion.LookRotation(velocity.normalized, up) : Quaternion.identity;
//Quaternion step = Quaternion.RotateTowards(currentForward, velocityForward, velocityRotationRate * deltaTime);
//velocity = Vector3.Lerp(velocity, step * Vector3.forward, velocityStep * deltaTime);

//attachedRigidbody.velocity = velocity;


//Quaternion from = Quaternion.CreateFromForwardUp(velocityDirection, component.rigidbody.gravityUp);
//Quaternion to = Quaternion.CreateFromForwardUp(block.WorldMatrix.Forward, component.rigidbody.gravityUp);            
/*
Vector3 direction = reverseGear ? block.WorldMatrix.Backward : block.WorldMatrix.Forward;
Quaternion from = grid.Physics.LinearVelocity.LengthSquared() > 0.001f ? Quaternion.CreateFromForwardUp(grid.Physics.LinearVelocity, Vector3.Up) : Quaternion.Identity;
Quaternion to = Quaternion.CreateFromForwardUp(direction, block.WorldMatrix.Up);
Quaternion step = BakurMathHelper.RotateTowards(from, to, maxAngle * physicsDeltaTime) - to;

Vector3 rotatedVelocity = Vector3D.Transform(grid.Physics.LinearVelocity, step);
//Vector3 velocity = Vector3.Lerp(grid.Physics.LinearVelocity, Vector3.Transform(Vector3.Forward, step), (float)(maxAngle * physicsDeltaTime));
grid.Physics.LinearVelocity = rotatedVelocity;
*/
//Vector3 blockPosition = block.GetPosition();

//Quaternion velocityForward = Quaternion.CreateFromForwardUp(velocity, up);

//Quaternion step = from - RotateTowards(from, to, maxDegrees);                        
//float angle = BakurMathHelper.Angle(from, to);
// Quaternion step = currentForward - BakurMathHelper.RotateTowards(currentForward, velocityForward, maxAngle);
//Quaternion step = Quaternion.Slerp(from, to, maxRotationAngle) - from;

//MyAPIGateway.Utilities.ShowMessage("Flight", "maxSpeed: " + maxSpeed + ", normalized: " + normalizedVelocity);
//Matrix rotationStep = Matrix.CreateFromQuaternion(step);
// rotatedVelocity = Vector3D.Transform(grid.Physics.LinearVelocity, step);
// grid.Physics.LinearVelocity = rotatedVelocity;

//Vector3 rotatedVelocity = velocity;
//Vector3.RotateAndScale(ref velocity, ref rotationStep, out rotatedVelocity);
#endregion
