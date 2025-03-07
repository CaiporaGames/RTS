using Unity.Entities;
using UnityEngine;

public class ZombieSpawnAuthoring : MonoBehaviour
{
    [SerializeField] private float maxTimer = 5f;
    [SerializeField] private float timer = 5f;
    public class Baker : Baker<ZombieSpawnAuthoring>
    {
        public override void Bake(ZombieSpawnAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ZombieSpawn
            {
                maxTimer = authoring.maxTimer,
                timer = authoring.timer
            });
        }
    }
}
