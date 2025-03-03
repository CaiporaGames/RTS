using Unity.Entities;
using UnityEngine;

public class MoveSpeedAuthoring : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    public class Baker : Baker<MoveSpeedAuthoring>
    {
        public override void Bake(MoveSpeedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveSpeed
            {
                speed = authoring.speed
            });
        }
    }
}
