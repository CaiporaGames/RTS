using Unity.Entities;
using UnityEngine;

public class SelectedAuthoring : MonoBehaviour
{
    public GameObject visualGameobject;
    public float selectedScale;

    public class Baker : Baker<SelectedAuthoring>
    {
        public override void Bake(SelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Selected
            {
                visualEntity = GetEntity(authoring.visualGameobject, TransformUsageFlags.Dynamic),
                selectedScale = authoring.selectedScale
            });
            SetComponentEnabled<Selected>(entity, false);
        }
    }
}
