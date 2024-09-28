using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : State
{
    protected override void StartState()
    {
        base.StartState();
        controller.rigidBody.velocity = Vector3.zero;
        currentEffect = Effect.Stunned;
    }
    protected override void EndState()
    {
        base.EndState();

        controller.rigidBody.velocity = Vector3.zero;

        if (controller.wantsToMove) actor.OnWalk.Invoke();
        else actor.OnIdle.Invoke();
    }
    private void OnEnable()
    {
        controller.OnHurt.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        controller.OnHurt.RemoveListener(SetToCurrentState);
    }
}