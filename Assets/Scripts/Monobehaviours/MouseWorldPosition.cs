using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = null;

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
