using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : State
{
    protected override void StartState()
    {
        base.StartState();
        actor.OnWalkAnim.Invoke();
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