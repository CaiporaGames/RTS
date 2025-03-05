using Unity.Entities;

public struct FindTarget : IComponentData
{
    public float findRange;
    public Faction faction;
}
