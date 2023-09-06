using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected ActorSO actor;
    protected virtual void SetToCurrentState() 
    {
        actor.currentState = this;
        StartState();
    }
    protected abstract void StartState();
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
}