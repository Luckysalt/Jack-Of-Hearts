using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    private void OnEnable()
    {
        actor.OnAttack.AddListener(SetToCurrentState);
    }
    private void OnDisable()
    {
        actor.OnAttack.RemoveListener(SetToCurrentState);
    }
}