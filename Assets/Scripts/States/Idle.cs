using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    protected override void StartState()
    {
        base.StartState();
        actor.OnIdleAnim.Invoke();
        controller.moveDirection = Vector3.zero;
    }
    private void OnEnable()
    {
        actor.OnIdle.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        actor.OnIdle.RemoveListener(SetToCurrentState);
    }
}