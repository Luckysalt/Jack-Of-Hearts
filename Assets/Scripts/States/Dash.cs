using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Movement
{
    protected override void StartState()
    {
        base.StartState();
        actor.OnDashAnim.Invoke();
        stateLifeTime = actor.dashTime;
        actor.rigidbody.velocity = Vector3.zero;
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        actor.rigidbody.AddForce(actor.lookDirection * actor.dashForce, ForceMode.Impulse);

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
        actor.OnDash.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        actor.OnDash.RemoveListener(SetToCurrentState);
    }
}