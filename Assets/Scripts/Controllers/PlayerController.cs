using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Idle))] //needs atleast 1 state, idle is default
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private ActorSO actor;
	private float dashCounter = 0;
	private bool isDashReady = true;

	private float attackCounter = 0;
	private bool isAttackReady = true;

    private void Awake()
    {
		actor.currentState = GetComponent<Idle>(); //sets idle state to default
		actor.rigidbody = GetComponent<Rigidbody>();
		actor.keybinds = new Keybinds();

		//gets input actions and sets the current state accordingly
		actor.keybinds.Player.Move.performed += ctx =>
		{
			actor.moveDirection = GetDirection(ctx);
			if (actor.currentState.GetType() == typeof(Dash)) return;
			if (actor.currentState.GetType() == typeof(Attack)) return;
			actor.OnWalk.Invoke();
		};
		actor.keybinds.Player.Move.canceled += ctx =>
		{ 
			if (actor.currentState.GetType() == typeof(Dash)) return;
			if (actor.currentState.GetType() == typeof(Attack)) return;
			actor.OnIdle.Invoke();
		};
		actor.keybinds.Player.Dash.performed += ctx =>
		{
			if (actor.currentState.GetType() == typeof(Dash))
			{
				actor.playerInputBuffer = ActorSO.InputBuffer.Dash;
				return;
			}
			else if (actor.currentState.GetType() == typeof(Attack))
            {
				actor.playerInputBuffer = ActorSO.InputBuffer.Dash;
				return;
            }

			if (!isDashReady) return;
			dashCounter = actor.dashCoolDown;
			isDashReady = false;
			actor.OnDash.Invoke();
		};
		actor.keybinds.Player.Attack.performed += ctx =>
		{
			if (actor.currentState.GetType() == typeof(Dash))
            {
				actor.playerInputBuffer = ActorSO.InputBuffer.Attack;
				return;
            }
			else if (actor.currentState.GetType() == typeof(Attack))
			{
				actor.playerInputBuffer = ActorSO.InputBuffer.Attack;
				return;
			}

			if (!isAttackReady)	return;
			attackCounter = actor.attackCoolDown;
			isAttackReady = false;
			actor.OnAttack.Invoke();
		};
	}
	private Vector3 GetDirection(InputAction.CallbackContext ctx)
	{
		Vector2 inputAxis = ctx.ReadValue<Vector2>();
		Vector3 dir = new Vector3(inputAxis.x, 0, inputAxis.y);
		dir = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * dir; //adjusts the moveDirection so it stays the same regardless Camera angle
		return dir;
	}
    private void Update()
    {
		if (actor.currentState.GetType() != typeof(Dash))
        {
			if (!isDashReady) dashCounter -= Time.deltaTime;
			if (dashCounter <= 0)
			{
				isDashReady = true;
			}
		}
		if (actor.currentState.GetType() != typeof(Attack))
		{
			if (!isAttackReady) attackCounter -= Time.deltaTime;
			if (attackCounter <= 0)
            {
				isAttackReady = true;

				if(actor.playerInputBuffer.Equals(ActorSO.InputBuffer.Attack))
                {
					actor.OnAttack.Invoke();
					isAttackReady = false;
					actor.playerInputBuffer = ActorSO.InputBuffer.Empty;
                }
			}
		}
		actor.currentState.UpdateState();
    }
    private void FixedUpdate()
    {
		actor.currentState.FixedUpdateState();
    }
	private void OnEnable()
	{
		actor.keybinds.Player.Enable();
	}
	private void OnDisable()
	{
		actor.keybinds.Player.Disable();
	}
}