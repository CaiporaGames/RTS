using Unity.Entities;

public struct ZombieSpawn : IComponentData
{
    public float maxTimer;
    public float timer;
}
