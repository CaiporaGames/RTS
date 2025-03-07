using Unity.Entities;

public struct EntitiesReference : IComponentData
{
    public Entity bulletEntity;
    public Entity zombieEntity;
}
