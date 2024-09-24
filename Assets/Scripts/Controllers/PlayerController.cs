using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
	private float dashCounter = 0;
	private bool isDashReady = true;

	private float attackCounter = 0;
	private bool isAttackReady = true;

	private Camera mainCamera;
	protected override void Awake()
    {
		base.Awake();
		controllerType = ControllerType.Player;
		mainCamera = Camera.main;
		aimPlaneLayerMask = LayerMask.GetMask("AimPlane");

		SetUpInputActions();
	}
    private void Update()
    {
		UpdateCooldown();
		UpdateInputBuffer();
		currentState.UpdateState();
    }
    private void FixedUpdate()
    {
		currentState.FixedUpdateState();
    }
	private void OnEnable()
	{
		inputActions.Player.Enable();
	}
	private void OnDisable()
	{
		inputActions.Player.Disable();
	}
	private void SetUpInputActions()
	{
		inputActions = new Keybinds();

		inputActions.Player.Move.performed += ctx => HandleMovement(ctx, true);
		inputActions.Player.Move.canceled += ctx => HandleMovement(ctx, false);
		inputActions.Player.Dash.performed += ctx => HandleDash(ctx);
		inputActions.Player.Attack.performed += ctx => HandleAttack(ctx);
		inputActions.Player.MousePosition.performed += ctx => HandleAim(ctx);
	}
	private void HandleMovement(InputAction.CallbackContext ctx, bool isMoving)
	{
		moveDirection = GetDirection(ctx);
		if (IsInCombatState()) return;
		if (isMoving)
			actor.OnWalk.Invoke();
		else
			actor.OnIdle.Invoke();
	}

	private void HandleDash(InputAction.CallbackContext ctx)
	{
		if (IsInCombatState())
        {
			playerInputBuffer = InputBuffer.Dash;
			return;
		}

		if (!isDashReady) return;
		dashCounter = actor.dashCoolDown;
		isDashReady = false;
		actor.OnDash.Invoke();
	}
	private void HandleAttack(InputAction.CallbackContext ctx)
	{
		if (IsInCombatState())
		{
			playerInputBuffer = InputBuffer.Attack;
			return;
		}

		if (!isAttackReady) return;
		attackCounter = actor.attackCoolDown;
		isAttackReady = false;
		actor.OnAttack.Invoke();
	}
	private void HandleAim(InputAction.CallbackContext ctx)
    {
		Vector2 mousePosition = ctx.ReadValue<Vector2>();
		Ray ray = mainCamera.ScreenPointToRay(mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, aimPlaneLayerMask))
		{
			Vector3 aimPlanePoint = hit.point;
			float aimHeight = transform.position.y;

			Vector3 cameraPosition = mainCamera.transform.position;
			float t = (aimHeight - aimPlanePoint.y) / (cameraPosition.y - aimPlanePoint.y);
			
			aimTarget = Vector3.Lerp(aimPlanePoint, cameraPosition, t);

			aimDirection = new Vector3(aimTarget.x, 0, aimTarget.z) - new Vector3(transform.position.x, 0, transform.position.z);

			aimDirection = aimDirection.normalized;
		}
	}
	private Vector3 GetDirection(InputAction.CallbackContext ctx)
	{
		Vector2 inputAxis = ctx.ReadValue<Vector2>();
		Vector3 dir = new Vector3(inputAxis.x, 0, inputAxis.y);
		dir = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * dir; //adjusts the moveDirection so it stays the same regardless Camera angle
		return dir;
	}
	private bool IsInCombatState()
	{
		return currentState is Dash || currentState is Attack;
	}
	private void UpdateCooldown()
    {
		if (!(currentState is Dash))
		{
			if (!isDashReady) dashCounter -= Time.deltaTime;
			if (dashCounter <= 0) isDashReady = true;
		}
		if (!(currentState is Attack))
		{
			if (!isAttackReady) attackCounter -= Time.deltaTime;
			if (attackCounter <= 0) isAttackReady = true;
		}
	}
	private void UpdateInputBuffer()
    {
		if (!isAttackReady) return;
		if (currentState is Dash) return;
		if (!playerInputBuffer.Equals(InputBuffer.Attack)) return;

		actor.OnAttack.Invoke();
		attackCounter = actor.attackCoolDown;
		isAttackReady = false;
		playerInputBuffer = InputBuffer.Empty;
	}
}