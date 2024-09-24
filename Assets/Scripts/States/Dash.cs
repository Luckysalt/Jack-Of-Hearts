using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : State
{
    private Vector3 dashTarget;
    protected override void StartState()
    {
        base.StartState();
        actor.OnDashAnim.Invoke(true);
        stateLifeTime = actor.dashTime;
        controller.rigidBody.velocity = Vector3.zero;

        SetDashTarget();
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        Vector3 newPosition = MathsUtils.LerpSmoothing(transform.position, dashTarget, actor.dashSpeed, Time.fixedDeltaTime);

        controller.rigidBody.MovePosition(newPosition);

        stateLifeTime -= Time.fixedDeltaTime;
        if (stateLifeTime <= 0) EndState();
    }
    protected override void EndState()
    {
        base.EndState();
        controller.rigidBody.velocity = Vector3.zero;

        actor.OnDashAnim.Invoke(false);

        if(controller.controllerType.Equals(Controller.ControllerType.Player))
        {
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
                    if (controller.inputActions.Player.Move.IsInProgress()) actor.OnWalk.Invoke();
                    else actor.OnIdle.Invoke();
                    break;
            }
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