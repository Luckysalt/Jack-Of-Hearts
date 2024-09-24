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
        player.OnIdleAnim.AddListener(setIdle);
        player.OnWalkAnim.AddListener(setWalk);
        player.OnDashAnim.AddListener(setDash);
        player.OnAttackAnim.AddListener(setAttack);
    }
    private void OnDisable()
    {
        player.OnIdleAnim.RemoveListener(setIdle);
        player.OnWalkAnim.RemoveListener(setWalk);
        player.OnDashAnim.RemoveListener(setDash);
        player.OnAttackAnim.RemoveListener(setAttack);
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