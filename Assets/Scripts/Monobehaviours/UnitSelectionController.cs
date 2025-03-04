using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class UnitSelectionController : MonoBehaviour
{
    [SerializeField] private MouseWorldPosition mouseWorldPosition = null;
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Vector3 mousePosition = mouseWorldPosition.GetMouseHitPosition();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover>().Build(entityManager);

            NativeArray<UnitMover> unitMovers = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);

            for(int i = 0; i < unitMovers.Length; i++)
            {
                UnitMover unitMover = unitMovers[i];
                unitMover.targetPosition = mousePosition;
                unitMovers[i] = unitMover;
            }

            entityQuery.CopyFromComponentDataArray(unitMovers);
        }
    }
}
