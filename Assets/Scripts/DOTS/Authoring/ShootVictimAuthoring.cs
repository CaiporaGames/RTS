using Unity.Entities;
using UnityEngine;

public class ShootVictimAuthoring : MonoBehaviour
{
    [SerializeField] private Transform hitLocalTransform = null;
    public class Baker : Baker<ShootVictimAuthoring>
    {
        public override void Bake(ShootVictimAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootVictim
            {
                hitLocalPosition = authoring.hitLocalTransform.localPosition
            });
        }
    }
}
