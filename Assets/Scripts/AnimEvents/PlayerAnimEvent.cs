using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FMODUnity;

public class PlayerAnimEvent : MonoBehaviour
{
    public ActorSO player;
    public VisualEffect slash01;
    public VisualEffect slash02;
    public VisualEffect slash03;
    public AttackRange attackRange;

    [SerializeField] private EventReference slashSound;
    public void PlaySlash01VFX()
    {
        slash01.Play();
    }
    public void PlaySlash02VFX()
    {
        slash02.Play();
    }
    public void PlaySlash03VFX()
    {
        slash03.Play();
    }
    public void PlaySlashOneShot()
    {
        player.OnPlayOneShot.Invoke(slashSound, transform.position);
    }
    public void DealDamage(WeaponSO weapon)
    {
        attackRange.onDamage.Invoke(weapon.damage);
    }
}