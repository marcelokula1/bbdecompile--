using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class BsodaSparyScript : MonoBehaviour
{
	// Token: 0x06000034 RID: 52 RVA: 0x00002D98 File Offset: 0x00000F98
	private void Start()
	{
		if (this.shouldRotate)
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			eulerAngles.z = Mathf.Round(UnityEngine.Random.Range(0f, 359f));
			base.transform.eulerAngles = eulerAngles;
		}
		this.rb = base.GetComponent<Rigidbody>(); //Get the RigidBody
		this.rb.velocity = base.transform.forward * this.speed; //Move forward
		this.lifeSpan = 30f; //Set the lifespan
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002E18 File Offset: 0x00001018
	private void Update()
	{
		this.rb.velocity = base.transform.forward * this.speed; //Move forward
		this.lifeSpan -= Time.deltaTime; // Decrease the lifespan variable
		if (this.lifeSpan < 0f) //When the lifespan timer ends, destroy the BSODA
		{
			UnityEngine.Object.Destroy(base.gameObject, 0f);
		}
	}

	// Token: 0x04000046 RID: 70
	public float speed;

	// Token: 0x04000047 RID: 71
	private float lifeSpan;

	// Token: 0x04000048 RID: 72
	private Rigidbody rb;

	// Token: 0x04000049 RID: 73
	public bool shouldRotate;
}
