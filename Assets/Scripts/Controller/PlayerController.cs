using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Idle))] //needs atleast 1 state to fall back on, idle is default
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Keybinds keybinds;
	[SerializeField] private ActorSO actor;

    private void Awake()
    {
		actor.currentState = GetComponent<Idle>(); //sets idle state to default
		actor.rigidbody = GetComponent<Rigidbody>();

		keybinds = new Keybinds();

		//gets input actions and sets the current state accordingly
		keybinds.Player.Move.performed += ctx =>
		{
			actor.inputAxis = ctx.ReadValue<Vector2>(); //passes input axis
			actor.OnWalk.Invoke();
		};
		keybinds.Player.Move.canceled += ctx => actor.OnIdle.Invoke();
	}
	private void OnEnable()
	{
		keybinds.Player.Enable();
	}
	private void OnDisable()
	{
		keybinds.Player.Disable();
	}
    private void Update()
    {
		actor.currentState.UpdateState();
    }
    private void FixedUpdate()
    {
		actor.currentState.FixedUpdateState();
    }
}