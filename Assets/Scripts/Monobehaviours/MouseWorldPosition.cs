using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition instance {get; private set;}
    [SerializeField] private Camera mainCamera = null;

    void Start()
    {
        instance = this;
    }

    public Vector3 GetMouseHitPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
            if(hit.transform.CompareTag("Ground"))
                return ray.GetPoint(hit.distance);

        return Vector3.zero;
    }
}
