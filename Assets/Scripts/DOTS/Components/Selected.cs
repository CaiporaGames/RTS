using Unity.Entities;

public struct Selected : IComponentData, IEnableableComponent
{
    public Entity visualEntity;
    public float selectedScale;
}
