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

        foreach((var localTransform, var findTarget, var target) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<FindTarget>, RefRW<Target>>())
        {
            findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if(findTarget.ValueRW.timer > 0) continue;
            findTarget.ValueRW.timer = findTarget.ValueRO.maxTimer;

            distanceHits.Clear();
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << 6,
                GroupIndex = 0
            };
            if(collisionWorld.OverlapSphere
            (
                localTransform.ValueRO.Position, 
                findTarget.ValueRO.findRange,
                ref distanceHits,
                collisionFilter
            ))
            {
                foreach(DistanceHit distanceHit in distanceHits)
                {
                    if(!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))continue;
                    Unit unitTarget = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if(unitTarget.faction == findTarget.ValueRO.faction)
                    {
                        target.ValueRW.targetEntity = distanceHit.Entity;
                        break;
                    }
                }
            }
        }
    }
}
