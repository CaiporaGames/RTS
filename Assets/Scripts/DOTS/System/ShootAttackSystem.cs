using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ShootAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReference entitiesReference = SystemAPI.GetSingleton<EntitiesReference>();
        foreach((var localTransform, var shootAttack, var target, var unitMover) in 
            SystemAPI.Query<RefRW<LocalTransform>, RefRW<ShootAttack>, RefRO<Target>, RefRW<UnitMover>>())
        {
            if(target.ValueRO.targetEntity == Entity.Null) continue;

            shootAttack.ValueRW.timer = shootAttack.ValueRO.maxTimer;       

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

            if(math.distance(localTransform.ValueRO.Position, targetLocalTransform.Position) > shootAttack.ValueRO.attackDistance)
            {
                unitMover.ValueRW.targetPosition = targetLocalTransform.Position;
            }
            else unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;

            float3 aimDirection = targetLocalTransform.Position - localTransform.ValueRO.Position;
            aimDirection = math.normalize(aimDirection);

            quaternion targetRotation = quaternion.LookRotation(aimDirection, math.up());
            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation,
                targetRotation, SystemAPI.Time.DeltaTime * unitMover.ValueRO.rotationSpeed);


            shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;            
            if(shootAttack.ValueRO.timer > 0f) continue;

            Entity bulletEntity = state.EntityManager.Instantiate(entitiesReference.bulletEntity); 
            float3 bulletSpawnPosition = localTransform.ValueRO.TransformPoint(shootAttack.ValueRO.bulletSpawnOffset);
            SystemAPI.SetComponent(bulletEntity, LocalTransform.FromPosition(bulletSpawnPosition));
        
            RefRW<Bullet> bulletBullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
            bulletBullet.ValueRW.bulletDamage = shootAttack.ValueRO.attackDamage;

            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;
        }
    }
}