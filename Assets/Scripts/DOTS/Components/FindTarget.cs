using Unity.Entities;

public struct FindTarget : IComponentData
{
    public float findRange;
    public Faction faction;
    public float timer;
    public float maxTimer;
}
