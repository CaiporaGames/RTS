using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class UnitSelectionController : MonoBehaviour
{
    public event EventHandler OnSelectionAreaStarted;
    public event EventHandler OnSelectionAreaEnded;
    [SerializeField] private MouseWorldPosition mouseWorldPosition = null;
    private Vector2 selectStartMousePosition = Vector2.zero;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectStartMousePosition = Input.mousePosition;
            OnSelectionAreaStarted?.Invoke(this, EventArgs.Empty);
        }
        if(Input.GetMouseButtonUp(0))
        {
            OnSelectionAreaEnded?.Invoke(this, EventArgs.Empty);
        }

        if(Input.GetMouseButton(1))
        {
            Vector3 mousePosition = mouseWorldPosition.GetMouseHitPosition();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover, Selected>().Build(entityManager);

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

    public Rect GetSelectionAreaRect()
    {
        Vector2 selectEndMousePosition = Input.mousePosition;

        Vector2 lowerLeftCorner = new Vector2
        (
            Mathf.Min(selectStartMousePosition.x, selectEndMousePosition.x),
            Mathf.Min(selectStartMousePosition.y, selectEndMousePosition.y)
        );

        Vector2 upperRightCorner = new Vector2
        (
            Mathf.Max(selectStartMousePosition.x, selectEndMousePosition.x),
            Mathf.Max(selectStartMousePosition.y, selectEndMousePosition.y)
        );

        return new Rect(
            lowerLeftCorner.x, lowerLeftCorner.y,
            upperRightCorner.x - lowerLeftCorner.x, 
            upperRightCorner.y - lowerLeftCorner.y
        );
    }
}
