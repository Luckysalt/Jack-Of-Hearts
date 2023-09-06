using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private void OnEnable()
    {
        actor.OnIdle.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        actor.OnIdle.RemoveListener(SetToCurrentState);
    }
    protected override void StartState()
    {
        actor.inputAxis = Vector2.zero;
    }
}