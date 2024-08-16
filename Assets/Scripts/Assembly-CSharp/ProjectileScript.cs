using System;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private GameObject player;

    public bool pickedUp;

    public bool Thrown;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        this.GetComponent<BsodaSparyScript>().enabled = false;
    }

    private void Update()
    {
        if (pickedUp)
        {
            if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
            {
                pickedUp = false;
                Thrown = true;
                this.player.GetComponent<PlayerScript>().holdingObject = false;
            }
            if (this.Thrown)
            {
                this.GetComponent<BsodaSparyScript>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !pickedUp & !Thrown)
        {
            if (!this.player.GetComponent<PlayerScript>().holdingObject)
            {
                this.player.GetComponent<PlayerScript>().holdingObject = true;
                this.pickedUp = true;
            }
        }
    }

    private void LateUpdate()
	{
		if (this.pickedUp)
		{
			base.transform.position = this.player.transform.position + this.player.transform.forward * 2f + Vector3.up * -2f;
			base.transform.rotation = this.player.transform.rotation;
		}
	}
}
