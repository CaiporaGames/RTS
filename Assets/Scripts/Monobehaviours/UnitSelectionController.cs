using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class UnitSelectionController : MonoBehaviour
{
    public event EventHandler OnSelectionAreaStarted;
    public event EventHandler OnSelectionAreaEnded;
    [SerializeField] private MouseWorldPosition mouseWorldPosition = null;
    [SerializeField] private Camera mainCamera = null;
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
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);
            NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Selected> selectedArray = entityQuery.ToComponentDataArray<Selected>(Allocator.Temp);

            for(int i = 0; i < entities.Length; i++)
            {
                entityManager.SetComponentEnabled<Selected>(entities[i], false);
                Selected selected = selectedArray[i];
                selected.onDeselected = true;
                entityManager.SetComponentData(entities[i], selected);
            }

            Rect selectionAreaRect = GetSelectionAreaRect();
            float selectionAreaSize = selectionAreaRect.width + selectionAreaRect.height;
            float multipleSelectionSizeMin = 40f;
            bool isMultipleSelection = selectionAreaSize > multipleSelectionSizeMin;

            if(isMultipleSelection)
            {
                entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Unit>().WithPresent<Selected>()
                    .Build(entityManager);
                entities = entityQuery.ToEntityArray(Allocator.Temp);
                NativeArray<LocalTransform> localTransforms = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

                for(int i = 0; i < localTransforms.Length; i++)
                {
                    LocalTransform localTransform = localTransforms[i];
                    Vector2 unitScreenPosition = mainCamera.WorldToScreenPoint(localTransform.Position);

                    if(selectionAreaRect.Contains(unitScreenPosition))
                    {
                        entityManager.SetComponentEnabled<Selected>(entities[i], true);
                        Selected selected = entityManager.GetComponentData<Selected>(entities[i]);
                        selected.onSelected = true;
                        entityManager.SetComponentData(entities[i], selected);
                    }
                }
            }
            else//SingleSelection
            {
                entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                PhysicsWorldSingleton physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
                UnityEngine.Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastInput raycastInput = new RaycastInput
                {
                    Start = cameraRay.GetPoint(0f),//camera origin
                    End = cameraRay.GetPoint(100f),//Far way point
                    Filter = new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = 1u << 6,
                        GroupIndex = 0
                    }
                };

                if(collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
                {
                    if(entityManager.HasComponent<Unit>(hit.Entity) && entityManager.HasComponent<Selected>(hit.Entity))
                    {
                        entityManager.SetComponentEnabled<Selected>(hit.Entity, true);
                        Selected selected = entityManager.GetComponentData<Selected>(hit.Entity);
                        selected.onSelected = true;
                        entityManager.SetComponentData(hit.Entity, selected);
                    }
                }
            }

        }

        if(Input.GetMouseButton(1))
        {
            Vector3 mousePosition = mouseWorldPosition.GetMouseHitPosition();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover, Selected>().Build(entityManager);

            NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<UnitMover> unitMovers = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            NativeArray<float3> movePositionArray = GenerateMovePositionArray(mousePosition, entities.Length);
            for(int i = 0; i < unitMovers.Length; i++)
            {
                UnitMover unitMover = unitMovers[i];
                unitMover.targetPosition = movePositionArray[i];
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

    private NativeArray<float3> GenerateMovePositionArray(float3 targetPosition, int positionCount)
    {
        NativeArray<float3> positionArray = new NativeArray<float3>(positionCount, Allocator.Temp);
        if(positionCount == 0) return positionArray;

        positionArray[0] = targetPosition;//Center of the circle.
        if(positionCount == 1) return positionArray;

        float ringSize = 2.2f;
        int ringAmount = 0;
        int positionIndex = 1;

        while(positionIndex < positionCount) //run over all selected units
        {
            int ringPositionCount = 3 + ringAmount * 2;//Amount of waypoint in the ring

            for(int i = 0; i < ringPositionCount; i++)
            {
                float angle = i * (math.PI2 / ringPositionCount);//angle between waypoints
                float3 ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ringAmount + 1), 0, 0));
                float3 ringPosition = targetPosition + ringVector;

                positionArray[positionIndex] = ringPosition;
                positionIndex++;

                if(positionIndex >= positionCount) break;//We did generate all waypoint for the ring
            }
            ringAmount++;
        }
        
        return positionArray;
    }
}
