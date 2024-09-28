using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackRange : MonoBehaviour
{
    public List<Controller> enemiesInRange = new List<Controller>();
    public UnityEvent<float> onDamage = new UnityEvent<float>();

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other == transform.parent) return; // ToDo Allies and Enemies list
        if (other.CompareTag("Actor"))
        {
            Controller actorController = other.GetComponent<Controller>();
            if (actorController == null) return;
            enemiesInRange.Add(actorController);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;
        if (other == transform.parent) return; // ToDo Allies and Enemies list
        if (other.CompareTag("Actor"))
        {
            Controller actorController = other.GetComponent<Controller>();
            if (actorController == null) return;
            enemiesInRange.Remove(actorController);
        }
    }

    private void DamageEnemiesinRange(float damage)
    {
        foreach(EnemyController enemy in enemiesInRange)
        {
            enemy.TakeDamage(damage);
        }
    }

    private void OnEnable()
    {
        onDamage.AddListener((damage) => DamageEnemiesinRange(damage));
    }
    private void OnDisable()
    {
        onDamage.RemoveListener((damage) => DamageEnemiesinRange(damage));
    }
}
