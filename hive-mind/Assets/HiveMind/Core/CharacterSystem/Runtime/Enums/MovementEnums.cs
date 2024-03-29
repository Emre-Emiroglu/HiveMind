namespace HiveMind.Core.CharacterSystem.Runtime.Enums
{
    public enum MovementTypes : int
    {
        Transform = 0,
        Rigidbody = 1
    }
    public enum RigidbodyMovementTypes : int
    {
        ExplosionForce = 0,
        Force = 1,
        ForceAtPosition = 2,
        RelativeForce = 3,
        RelativeTorque = 4,
        Torque = 5,
    }
    public enum MovementStatus : int
    {
        Walk = 0,
        Run = 1
    }
}
