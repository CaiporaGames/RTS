using Unity.Entities;
using UnityEngine;

public class ShootAttackAuthoring : MonoBehaviour
{
    [SerializeField] private float maxTimer = 0.2f;
    [SerializeField] private float attackDamage = 5;
    [SerializeField] private float attackDistance = 5;
    [SerializeField] private Transform bulletSpawnOffset = null;
    public class Baker : Baker<ShootAttackAuthoring>
    {
        public override void Bake(ShootAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootAttack
            {
                maxTimer = authoring.maxTimer,
                attackDamage = authoring.attackDamage,
                attackDistance = authoring.attackDistance,
                bulletSpawnOffset = authoring.bulletSpawnOffset.localPosition
            });
        }
    }
}
