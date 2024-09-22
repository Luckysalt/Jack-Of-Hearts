using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    protected override void StartState()
    {
        base.StartState();
        stateLifeTime = .5f;
        actor.rigidbody.velocity = Vector3.zero;
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        // actor.rigidbody.AddForce(actor.lookDirection * actor.dashForce, ForceMode.Impulse);

        Debug.Log("Attack");
        stateLifeTime -= Time.fixedDeltaTime;
        if (stateLifeTime <= 0) EndState();
    }
    protected override void EndState()
    {
        base.EndState();
        actor.rigidbody.velocity = Vector3.zero;

        if (actor.keybinds.Player.Move.IsInProgress()) actor.OnWalk.Invoke();
        else actor.OnIdle.Invoke();
    }
    private void OnEnable()
    {
        actor.OnAttack.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        actor.OnAttack.RemoveListener(SetToCurrentState);
    }
}