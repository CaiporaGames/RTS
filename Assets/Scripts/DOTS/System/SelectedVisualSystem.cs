using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SelectedVisualSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>())
        {   
            if(selected.ValueRO.onDeselected)
            {
                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                localTransform.ValueRW.Scale = 0f;
            }
            if(selected.ValueRO.onSelected)
            {
                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                localTransform.ValueRW.Scale = selected.ValueRO.selectedScale;
            }
            
        }
    }
}
