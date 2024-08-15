using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainScript : MonoBehaviour
{
    public PlayerScript player;

    public AudioSource audioSource;

    public AudioClip slurp;

    public void Use()
    {
        this.player.stamina = 100f;
        Debug.Log("Stamina Refilled.");
        audioSource.PlayOneShot(slurp);
    }
}
