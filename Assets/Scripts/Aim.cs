using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;

    private InputAction mousePositionAction;
    private Camera mainCamera;

    [Header("Aim")]
    [SerializeField] private bool aim;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool ignoreHeight;
    [SerializeField] private Transform aimedTransform;

    [Header("Gizmos")]
    [SerializeField] private bool gizmo_cameraRay = false;
    [SerializeField] private bool gizmo_ground = false;
    [SerializeField] private bool gizmo_target = false;
    [SerializeField] private bool gizmo_ignoredHeightTarget = false;
    [SerializeField] private bool gizmo_projectedTarget = false;

    private void Awake()
    {
        mainCamera = Camera.main;

        groundMask = LayerMask.GetMask("Ground");

        mousePositionAction = actionAsset.FindAction("MousePosition");
    }
    private void OnEnable()
    {
        mousePositionAction.Enable();
    }

    private void OnDisable()
    {
        mousePositionAction.Disable();
    }
    private void Update()
    {
        Aiming();
    }
    private void Aiming()
    {
        if (aim == false)
        {
            return;
        }

        var (success, position) = GetMousePosition();
        if (success)
        {
            // Direction is usually normalized, 
            // but it does not matter in this case.
            Vector3 direction = position - aimedTransform.position;

            if (ignoreHeight)
            {
                // Ignore the height difference.
                direction.y = 0;
            }

            // Make the transform look at the mouse position.

            //aimedTransform.forward = direction;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        Vector2 mousePosition = mousePositionAction.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hit.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;

        Vector2 mousePosition = mousePositionAction.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            if (gizmo_cameraRay)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(ray.origin, hit.point);
                Gizmos.DrawWireSphere(ray.origin, 0.5f);
            }

            Vector3 hitPosition = hit.point;
            Vector3 hitGroundHeight = Vector3.Scale(hit.point, new Vector3(1, 0, 1)); ;
            Vector3 hitPositionIgnoredHeight = new Vector3(hit.point.x, aimedTransform.position.y, hit.point.z);

           // if (gizmo_ground)
           // {
           //     Gizmos.color = Color.red;
           //     Gizmos.DrawWireSphere(hitGroundHeight, 0.5f);
           //     Gizmos.DrawLine(hitGroundHeight, hitPosition);
           // }

            if (gizmo_target)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hit.point, 0.5f);
                Gizmos.DrawLine(aimedTransform.position, hitPosition);
            }

            if (gizmo_ignoredHeightTarget)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(hitPositionIgnoredHeight, 0.5f);
                Gizmos.DrawLine(aimedTransform.position, hitPositionIgnoredHeight);
            }

            if(gizmo_projectedTarget)
            {
                Vector3 cameraPosition = mainCamera.transform.position;
                float t = (hitPositionIgnoredHeight.y - hit.point.y) / (cameraPosition.y - hit.point.y);
                Vector3 aimTarget = Vector3.Lerp(hit.point, cameraPosition, t);
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(aimTarget, 0.5f);
                Gizmos.DrawLine(aimedTransform.position, aimTarget);
            }
        }
    }
}
