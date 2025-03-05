using Unity.Entities;
using UnityEngine;

public class FindTargetAuthoring : MonoBehaviour
{
    [SerializeField] private float findRange = 5f;
    [SerializeField] private float maxTimer = 5f;
    [SerializeField] private Faction faction;
    public class Baker : Baker<FindTargetAuthoring>
    {
        public override void Bake(FindTargetAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FindTarget
            {
                findRange = authoring.findRange,
                faction = authoring.faction,
                maxTimer = authoring.maxTimer
            });
        }
    }
}
