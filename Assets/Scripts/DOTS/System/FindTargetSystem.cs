using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct FindTargetSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHits = new NativeList<DistanceHit>(Allocator.Temp);

        foreach((var localTransform, var findTarget) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<FindTarget>>())
        {
            distanceHits.Clear();
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << 6,
                GroupIndex = 0
            };
            collisionWorld.OverlapSphere
            (
                localTransform.ValueRO.Position, 
                findTarget.ValueRO.findRange,
                ref distanceHits,
                collisionFilter
            );
        }
    }
}
