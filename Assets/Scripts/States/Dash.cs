using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : State
{
    private Vector3 dashTarget;
    protected override void StartState()
    {
        base.StartState();
        actor.OnDashAnim.Invoke();
        stateLifeTime = actor.dashTime;
        actor.rigidbody.velocity = Vector3.zero;

        SetDashTarget();
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        Vector3 newPosition = MathsUtils.LerpSmoothing(transform.position, dashTarget, actor.dashSpeed, Time.fixedDeltaTime);

        actor.rigidbody.MovePosition(newPosition);
        //actor.rigidbody.AddForce(actor.lookDirection * actor.dashSpeed, ForceMode.Impulse);

        stateLifeTime -= Time.fixedDeltaTime;
        if (stateLifeTime <= 0) EndState();
    }
    protected override void EndState()
    {
        base.EndState();
        actor.rigidbody.velocity = Vector3.zero;

        switch (actor.playerInputBuffer)
        {
            case ActorSO.InputBuffer.Attack:
                actor.OnAttack.Invoke();
                actor.playerInputBuffer = ActorSO.InputBuffer.Empty;
                break;
            case ActorSO.InputBuffer.Dash:
                actor.OnDash.Invoke();
                actor.playerInputBuffer = ActorSO.InputBuffer.Empty;
                break;
            case ActorSO.InputBuffer.Empty:
                if (actor.keybinds.Player.Move.IsInProgress()) actor.OnWalk.Invoke();
                else actor.OnIdle.Invoke();
                break;
        }
    }
    private void SetDashTarget()
    {
        float distance = actor.dashDistance;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, actor.lookDirection, out hit, Mathf.Infinity, actor.antiClippingDetection))
        {
            if (distance > hit.distance)
            {
                distance = hit.distance;
            }
        }

        dashTarget = transform.position + actor.lookDirection * distance;
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