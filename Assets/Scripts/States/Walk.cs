using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : Movement
{
    protected override void StartState()
    {
        actor.OnWalkAnim.Invoke();
        actor.moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * actor.moveDirection; //corrects the moveDirection accordingly to camera angle
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