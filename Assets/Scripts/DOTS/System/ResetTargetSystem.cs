using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct ResetTargetSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(var target in SystemAPI.Query<RefRW<Target>>())
            if(
                target.ValueRW.targetEntity != Entity.Null && (
                !SystemAPI.Exists(target.ValueRO.targetEntity) ||
                !SystemAPI.HasComponent<LocalTransform>(target.ValueRO.targetEntity))//This if because some strange bug. Entity exists but does not have transform.
            ) 
                target.ValueRW.targetEntity = Entity.Null;
    }
}
