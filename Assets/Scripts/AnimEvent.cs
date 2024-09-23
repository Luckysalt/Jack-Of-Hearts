using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FMODUnity;

public class AnimEvent : MonoBehaviour
{
    public VisualEffect slash01;
    public VisualEffect slash02;
    public VisualEffect slash03;
    public VisualEffect poof;

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
        poof.Play();
    }
    public void PlaySlashOneShot()
    {
        AudioManager.instance.PlayOneShot(slashSound, transform.position);
    }
}
