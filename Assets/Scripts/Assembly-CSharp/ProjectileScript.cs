using System;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private PlayerScript player;

    private GameObject cm;

    public bool pickedUp;

    public bool Thrown;

    private Rigidbody rb;

    private void Start()
    {
        rb = base.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        cm = GameObject.FindWithTag("MainCamera");
        base.GetComponent<BsodaSparyScript>().enabled = false;
    }

    private void Update()
    {
        if (this.pickedUp)
        {
            if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
            {
                this.pickedUp = false;
                this.Thrown = true;
                this.player.holdingObject = false;
            }
            if (this.Thrown)
            {
                this.GetComponent<BsodaSparyScript>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !this.pickedUp & !this.Thrown)
        {
            if (!this.player.holdingObject)
            {
                this.player.holdingObject = true;
                this.pickedUp = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (this.pickedUp && !this.Thrown)
        {
            if (!Input.GetButton("Look Behind"))
            {
                base.transform.position = this.player.transform.position + this.player.transform.forward * 3f + Vector3.up * -2f;
                base.transform.rotation = this.cm.transform.rotation;
            }
            else
            {
                base.transform.position = this.player.transform.position + this.player.transform.forward * -3f + Vector3.up * -2f;
                base.transform.rotation = this.cm.transform.rotation;
            }
        }
    }
}
