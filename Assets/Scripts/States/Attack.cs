using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    private Vector3 attackDashTarget;
    protected override void StartState()
    {
        base.StartState();

        actor.OnAttackAnim.Invoke();

        stateLifeTime = actor.attackTime;
        actor.rigidbody.velocity = Vector3.zero;
        actor.lookDirection = actor.aimDirection;

        SetAttackDashTarget();
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        Vector3 newPosition = MathsUtils.LerpSmoothing(transform.position, attackDashTarget, actor.attackDashSpeed, Time.fixedDeltaTime);

        actor.rigidbody.MovePosition(newPosition);

        //Debug.DrawLine(transform.position, attackDashTarget,Color.black);

        stateLifeTime -= Time.fixedDeltaTime;
        if (stateLifeTime <= 0) EndState();
    }
    protected override void EndState()
    {
        base.EndState();
        actor.rigidbody.velocity = Vector3.zero;

        switch(actor.playerInputBuffer)
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
    private void SetAttackDashTarget()
    {
        float distance = actor.attackDashDistance;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, actor.aimDirection, out hit, Mathf.Infinity, actor.antiClippingDetection))
        {
            if(distance > hit.distance)
            {
                distance = hit.distance;
            }
        }

        attackDashTarget = transform.position + actor.aimDirection * distance;
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