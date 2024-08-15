using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLockerScript : MonoBehaviour
{
    public GameObject player;
    public BaldiScript baldi;
    public bool hidden;
    public AudioSource audioSource;
    public AudioClip slam;
    public Transform playerCamera;
    public Transform lockerCamera;
    public GameObject baldicator;
    public GameObject lockerUi;
    public int silentUses;

    void Update()
    {
        if(hidden)
        {
            if(silentUses <= 0)
            {
                baldi.Hear(base.transform.position, 78);
            }
        }
    }

    public void Hide()
    {
        baldicator.SetActive(false);
        lockerUi.SetActive(true);
        SwitchToLockerCamera();
        player.SetActive(false);
        Debug.Log("Player is in locker.");
        silentUses--;
        if(silentUses <= 0)
        {
            audioSource.PlayOneShot(slam);
        }
        hidden = true;
    }

    public void UnHide()
    {
        baldicator.SetActive(true);
        lockerUi.SetActive(false);
        SwitchToPlayerCamera();
        player.SetActive(true);
        Debug.Log("Player exited locker.");
        if(silentUses <= 0)
        {
            audioSource.PlayOneShot(slam);
        }
        hidden = false;
    }

    private void SwitchToLockerCamera()
    {
        //playerCamera.gameObject.SetActive(false);
        lockerCamera.gameObject.SetActive(true);
    }
    private void SwitchToPlayerCamera()
    {
        //playerCamera.gameObject.SetActive(true);
        lockerCamera.gameObject.SetActive(false);
    }
}
