using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Controller controller;
    private Animator animator;

    private void Awake()
    {
        controller = gameObject.GetComponentInParent<Controller>();
        animator = gameObject.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        controller.OnHurt.AddListener(TriggerHurtAnimation);
    }
    private void OnDisable()
    {
        controller.OnHurt.RemoveListener(TriggerHurtAnimation);
    }
    private void TriggerHurtAnimation()
    {
        animator.SetTrigger("hurt");
    }
}
