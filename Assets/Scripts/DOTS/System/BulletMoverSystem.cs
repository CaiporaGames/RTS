using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct BulletMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
        SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach((var localTransform, var bullet, var target, var entity) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<Bullet>, RefRO<Target>>().WithEntityAccess())
            {
                if(target.ValueRO.targetEntity == Entity.Null)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                    continue;
                }

                LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
                ShootVictim shootVictim = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
                float3 targetPosition = targetLocalTransform.TransformPoint(shootVictim.hitLocalPosition);

                float3 moveDirection = targetPosition - localTransform.ValueRO.Position;
                moveDirection = math.normalize(moveDirection);

                localTransform.ValueRW.Position += moveDirection * bullet.ValueRO.bulletSpeed * SystemAPI.Time.DeltaTime;
                float destroyDistance = 0.2f;
                if(math.distancesq(localTransform.ValueRO.Position, targetPosition) < destroyDistance)
                {
                    RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                    targetHealth.ValueRW.health -= bullet.ValueRO.bulletDamage;

                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
    }
}
