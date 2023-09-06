using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walk : State
{
    private void OnEnable()
    {
        actor.OnWalk.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        actor.OnWalk.RemoveListener(SetToCurrentState);
    }
    protected override void StartState()
    {
        actor.inputAxis = actor.callbackContext.ReadValue<Vector2>();
    }
}