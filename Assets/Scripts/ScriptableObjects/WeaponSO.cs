using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private float m_damage = 30f;

    public float damage => m_damage;
}
