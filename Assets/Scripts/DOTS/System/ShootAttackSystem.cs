using Unity.Burst;
using Unity.Entities;

partial struct ShootAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((var shootAttack, var target) in SystemAPI.Query<RefRW<ShootAttack>, RefRO<Target>>())
        {
            if(target.ValueRO.targetEntity == Entity.Null) continue;

            shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;            
            if(shootAttack.ValueRO.timer > 0f) continue;

            shootAttack.ValueRW.timer = shootAttack.ValueRO.maxTimer;

            RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
            float damageAmount = 1;
            targetHealth.ValueRW.health -= damageAmount;
        }
    }
}