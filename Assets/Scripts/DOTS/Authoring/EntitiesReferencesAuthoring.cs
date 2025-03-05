using Unity.Entities;
using UnityEngine;

public class EntitiesReferencesAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab = null;
    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReference
            {
                bulletEntity = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
