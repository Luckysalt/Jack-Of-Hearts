using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : State
{
    protected override void StartState()
    {
        base.StartState();
        actor.OnWalkAnimation.Invoke();
        currentEffect = Effect.Default;
    }
    private void OnEnable()
    {
        actor.OnWalk.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        actor.OnWalk.RemoveListener(SetToCurrentState);
    }
}