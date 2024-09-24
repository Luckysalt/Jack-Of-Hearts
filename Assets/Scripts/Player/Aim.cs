using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private ActorSO actor;

    private InputAction mousePositionAction;
    private Camera mainCamera;

    [Header("Aim")]
    [SerializeField] private bool aim;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform aimingTransform;

    [Header("Gizmos")]
    [SerializeField] private bool gizmo_cameraRay = false;
    [SerializeField] private bool gizmo_target = false;
    [SerializeField] private bool gizmo_ignoredHeightTarget = false;
    [SerializeField] private bool gizmo_projectedTarget = false;

    private void Awake()
    {
        mainCamera = Camera.main;

        groundMask = LayerMask.GetMask("AimGround");

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
            Vector3 groundPoint = position;
            Vector3 aimPoint = new Vector3(position.x, aimingTransform.position.y,position.z);

            Vector3 cameraPosition = mainCamera.transform.position;
            float t = (aimPoint.y - groundPoint.y) / (cameraPosition.y - groundPoint.y);
            Vector3 aimTarget = Vector3.Lerp(groundPoint, cameraPosition, t);

            Vector3 aimDirection = new Vector3(aimTarget.x, 0, aimTarget.z) - new Vector3(aimingTransform.position.x, 0, aimingTransform.position.z);

            actor.aimTarget = aimTarget;
            actor.aimDirection = aimDirection.normalized;
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
            Vector3 hitPositionIgnoredHeight = new Vector3(hit.point.x, aimingTransform.position.y, hit.point.z);

            if (gizmo_target)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hit.point, 0.5f);
                Gizmos.DrawLine(aimingTransform.position, hitPosition);
            }

            if (gizmo_ignoredHeightTarget)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(hitPositionIgnoredHeight, 0.5f);
                Gizmos.DrawLine(aimingTransform.position, hitPositionIgnoredHeight);
            }

            if(gizmo_projectedTarget)
            {

                Vector3 groundPoint = hitPosition;
                Vector3 aimPoint = hitPositionIgnoredHeight;

                Vector3 cameraPosition = mainCamera.transform.position;
                float t = (aimPoint.y - groundPoint.y) / (cameraPosition.y - groundPoint.y);
                Vector3 aimTarget = Vector3.Lerp(groundPoint, cameraPosition, t);

                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(aimTarget, 0.5f);
                Gizmos.DrawLine(aimingTransform.position, aimTarget);
            }
        }
    }
}