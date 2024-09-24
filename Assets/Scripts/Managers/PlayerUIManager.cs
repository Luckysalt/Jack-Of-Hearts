using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerController player;
    public bool showAimGizmo = true;

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;

        if (showAimGizmo)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(player.aimTarget, 0.5f);
            Gizmos.DrawLine(player.transform.position, player.aimTarget);
        }
    }
}