using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : Controller
{
	//private float dashCounter = 0;
	//private bool isDashReady = true;

	//private float attackCounter = 0;
	//private bool isAttackReady = true;

	protected override void Awake()
    {
		base.Awake();
		currentHealth = actor.maxHealth;

		wantsToMove = false;
	}
    private void Update()
    {
		currentState.UpdateState();
    }
    private void FixedUpdate()
    {
		currentState.FixedUpdateState();
    }
}