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
			actor.OnWalk.Invoke();
		};
		actor.keybinds.Player.Move.canceled += ctx =>
		{ 
			if (actor.currentState.GetType() == typeof(Dash)) return;
			actor.OnIdle.Invoke();
		};
		actor.keybinds.Player.Dash.performed += ctx =>
		{
			if (!isDashReady) return;
			dashCounter = actor.dashCoolDown;
			isDashReady = false;
			actor.OnDash.Invoke();
		};
		actor.keybinds.Player.Attack.performed += ctx => actor.OnAttack.Invoke();
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
		if(!isDashReady) dashCounter -= Time.deltaTime;
		if (dashCounter <= 0) isDashReady = true;	

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