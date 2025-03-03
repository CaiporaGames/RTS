using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float rotationSpeed = 10f;
        foreach ((RefRW<LocalTransform> localTransform, RefRO<MoveSpeed> moveSpeed, RefRW<PhysicsVelocity> physicsVelocity) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>())
        {
            float3 targetPosition = MouseWorldPosition.instance.GetMouseHitPosition();
            float3 moveDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);

            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation,
                quaternion.LookRotation(moveDirection, math.up()), SystemAPI.Time.DeltaTime * rotationSpeed);
            physicsVelocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.speed;
            physicsVelocity.ValueRW.Angular = float3.zero;
        }
    }
}
