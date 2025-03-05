using Unity.Entities;
using Unity.Mathematics;

public struct ShootAttack : IComponentData
{
    public float timer;
    public float maxTimer;
    public float attackDamage;
    public float attackDistance;
    public float3 bulletSpawnOffset;
}
