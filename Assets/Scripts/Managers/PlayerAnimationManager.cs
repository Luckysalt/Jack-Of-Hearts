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

    private UnityAction<bool, int> setAttack;
    private List<string> attacks = new List<string>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        setIdle = new UnityAction(() => SetIsWalking(false));
        setWalk = new UnityAction(() => SetIsWalking(true));

        setAttack = new UnityAction<bool, int>((isAttacking, attackID) => SetIsAttacking(isAttacking, attackID));

        attacks.Add("Attack_01");
        attacks.Add("Attack_02");
        attacks.Add("Attack_03");
    }
    private void OnEnable()
    {
        actor.OnIdleAnim.AddListener(setIdle);
        actor.OnWalkAnim.AddListener(setWalk);
        actor.OnDashAnim.AddListener(TriggerDash);
        actor.onAttackAnim.AddListener(setAttack);
    }
    private void OnDisable()
    {
        actor.OnIdleAnim.RemoveListener(setIdle);
        actor.OnWalkAnim.RemoveListener(setWalk);
        actor.OnDashAnim.RemoveListener(TriggerDash);
        actor.onAttackAnim.RemoveListener(setAttack);

    }

    private void SetIsWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
    private void TriggerDash()
    {
        animator.SetTrigger("Dash");
    }
    private void SetIsAttacking(bool isAttacking, int attackID)
    {
        animator.SetBool(attacks[attackID],isAttacking);
    }
}