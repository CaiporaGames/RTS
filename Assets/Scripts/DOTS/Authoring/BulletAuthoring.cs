using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletDamage = 5f;

    public class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bullet
            {
                bulletDamage = authoring.bulletDamage,
                bulletSpeed = authoring.bulletSpeed
            });
        }
    }
}
