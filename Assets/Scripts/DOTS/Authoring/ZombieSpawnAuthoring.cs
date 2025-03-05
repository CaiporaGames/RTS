using Unity.Entities;
using UnityEngine;

public class ZombieSpawnAuthoring : MonoBehaviour
{
    public class Baker : Baker<ZombieSpawnAuthoring>
    {
        public override void Bake(ZombieSpawnAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ZombieSpawn());
        }
    }
}
