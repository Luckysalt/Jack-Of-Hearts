using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Animator))]
public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private ActorSO actor;
    private Animator animator;
    private UnityAction setIdle;
    private UnityAction setWalk;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        setIdle = new UnityAction(() => SetIsWalking(false));
        setWalk = new UnityAction(() => SetIsWalking(true));
    }
    private void OnEnable()
    {
        actor.OnIdle.AddListener(setIdle);
        actor.OnWalk.AddListener(setWalk);
    }
    private void OnDisable()
    {
        actor.OnIdle.RemoveListener(setIdle);
        actor.OnWalk.RemoveListener(setWalk);
    }

    private void SetIsWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
}