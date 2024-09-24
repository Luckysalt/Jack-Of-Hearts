using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public ActorSO player;
    private UnityAction<EventReference, Vector3> playOneShot;

    private void Awake()
    {
        playOneShot = new UnityAction<EventReference, Vector3>((sound, worldPos) => PlayOneShot(sound, worldPos));
    }
    private void OnEnable()
    {
        player.OnPlayOneShot.AddListener(playOneShot);
    }
    private void OnDisable()
    {
        player.OnPlayOneShot.RemoveListener(playOneShot);
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
