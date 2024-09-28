using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private ActorSO player;
    private Animator animator;
    private UnityAction setIdle;
    private UnityAction setWalk;
    private UnityAction<bool> setDash;
    private UnityAction<bool, int> setAttack;

    private List<string> attacks = new List<string>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        setIdle = new UnityAction(() => SetIsWalking(false));
        setWalk = new UnityAction(() => SetIsWalking(true));

        setDash = new UnityAction<bool>((isDashing) => SetIsDashing(isDashing));
        setAttack = new UnityAction<bool, int>((isAttacking, attackID) => SetIsAttacking(isAttacking, attackID));

        attacks.Add("Attack_01");
        attacks.Add("Attack_02");
        attacks.Add("Attack_03");
    }
    private void OnEnable()
    {
        player.OnIdleAnimation.AddListener(setIdle);
        player.OnWalkAnimation.AddListener(setWalk);
        player.OnDashAnimation.AddListener(setDash);
        player.OnAttackAnimation.AddListener(setAttack);
    }
    private void OnDisable()
    {
        player.OnIdleAnimation.RemoveListener(setIdle);
        player.OnWalkAnimation.RemoveListener(setWalk);
        player.OnDashAnimation.RemoveListener(setDash);
        player.OnAttackAnimation.RemoveListener(setAttack);
    }

    private void SetIsWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
    private void SetIsDashing(bool isDashing)
    {
        animator.SetBool("isDashing", isDashing);
    }
    private void SetIsAttacking(bool isAttacking, int attackID)
    {
        animator.SetBool(attacks[attackID],isAttacking);
    }
}