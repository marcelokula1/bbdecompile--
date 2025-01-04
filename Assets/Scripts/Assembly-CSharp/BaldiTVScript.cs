using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldiTVScript : MonoBehaviour
{
    void Update()
    {
        if (this.gc.notebooks == Notebooks & this.gc.mode == "story" & isBaldiTV == false)
        {
            BaldiTVAnimator.Play("BaldiTV");
            isBaldiTV = true;
            AudioEnd.Play();
        }
        if (isBaldiTV == true & AudioEnd.isPlaying == false & Audio.isPlaying == false & isBaldiTVEnd == false)
        {
            Audio.Play();
            isBaldiTVEnd = true;
        }
    }

    public GameControllerScript gc;

    public AudioSource Audio;

    public AudioSource AudioEnd;

    public Animator BaldiTVAnimator;

    public bool isBaldiTV;

    public bool isBaldiTVEnd;

    public int Notebooks;
}
