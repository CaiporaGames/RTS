using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    public float deltaTime;
    private float reachedTargetDistance;
    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirection = unitMover.targetPosition - localTransform.Position;

        reachedTargetDistance = 2;
        if (HasReachedTarget(moveDirection, ref physicsVelocity)) return;
        
        moveDirection = math.normalize(unitMover.targetPosition - localTransform.Position);

        localTransform.Rotation = math.slerp(localTransform.Rotation,
            quaternion.LookRotation(moveDirection, math.up()), deltaTime * unitMover.rotationSpeed);
        physicsVelocity.Linear = moveDirection * unitMover.moveSpeed;
        physicsVelocity.Angular = float3.zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HasReachedTarget(float3 moveDirection, ref PhysicsVelocity physicsVelocity)
    {
        if (math.lengthsq(moveDirection) < reachedTargetDistance)
        {
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return true;
        }
        return false;
    }
}
