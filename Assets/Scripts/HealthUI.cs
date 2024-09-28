using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private float healthChangeSpeed = 1f;
    private Controller controller;
    private Line healthShape;
    private float displayedHealth;
    private float targetHealth = 1;

    private void Awake()
    {
        controller = GetComponentInParent<Controller>();
        healthShape = GetComponent<Line>();
    }
    private void Update()
    {
        displayedHealth = MathsUtils.LerpSmoothing(displayedHealth, targetHealth, healthChangeSpeed, Time.deltaTime);
        healthShape.End = displayedHealth * Vector3.right;
    }
    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        targetHealth = currentHealth / maxHealth;
    }

    private void OnEnable()
    {
        controller.onHealthUIChange.AddListener((currentHealth, maxHealth) => UpdateHealthUI(currentHealth, maxHealth));
    }
    private void OnDisable()
    {
        controller.onHealthUIChange.RemoveListener((currentHealth, maxHealth) => UpdateHealthUI(currentHealth, maxHealth));
    }
}
