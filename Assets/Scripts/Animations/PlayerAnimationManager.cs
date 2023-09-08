using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Animator))]
public class PlayerAnimationManager : MonoBehaviour
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
        actor.OnIdleAnim.AddListener(setIdle);
        actor.OnWalkAnim.AddListener(setWalk);
    }
    private void OnDisable()
    {
        actor.OnIdleAnim.RemoveListener(setIdle);
        actor.OnWalkAnim.RemoveListener(setWalk);
    }

    private void SetIsWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
}