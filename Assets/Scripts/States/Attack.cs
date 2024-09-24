using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    private Vector3 attackDashTarget;
    protected override void StartState()
    {
        base.StartState();
        actor.OnAttackAnim.Invoke(true, actor.currentAttackID);

        stateLifeTime = actor.attackTime;
        controller.rigidBody.velocity = Vector3.zero;
        controller.lookDirection = controller.aimDirection;

        SetAttackDashTarget();
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        Vector3 newPosition = MathsUtils.LerpSmoothing(transform.position, attackDashTarget, actor.attackDashSpeed, Time.fixedDeltaTime);

        controller.rigidBody.MovePosition(newPosition);

        //Debug.DrawLine(transform.position, attackDashTarget,Color.black);

        stateLifeTime -= Time.fixedDeltaTime;
        if (stateLifeTime <= 0) EndState();
    }
    protected override void EndState()
    {
        base.EndState();

        actor.OnAttackAnim.Invoke(false,actor.currentAttackID);

        if (actor.currentAttackID >= 2)
        {
            actor.currentAttackID = 0;
        }
        else
        {
            actor.currentAttackID++;
        }

        controller.rigidBody.velocity = Vector3.zero;

        if (controller.controllerType.Equals(Controller.ControllerType.Player))
        {
            switch (controller.playerInputBuffer)
            {
                case Controller.InputBuffer.Attack:
                    actor.OnAttack.Invoke();
                    controller.playerInputBuffer = Controller.InputBuffer.Empty;
                    return;
                case Controller.InputBuffer.Dash:
                    actor.OnDash.Invoke();
                    controller.playerInputBuffer = Controller.InputBuffer.Empty;
                    return;
                case Controller.InputBuffer.Empty:
                    if (controller.inputActions.Player.Move.IsInProgress()) actor.OnWalk.Invoke();
                    else actor.OnIdle.Invoke();
                    return;
            }
        }
    }
    private void SetAttackDashTarget()
    {
        float distance = actor.attackDashDistance;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, controller.aimDirection, out hit, Mathf.Infinity, actor.antiClippingDetection))
        {
            if(distance > hit.distance)
            {
                distance = hit.distance;
            }
        }

        attackDashTarget = transform.position + controller.aimDirection * distance;
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