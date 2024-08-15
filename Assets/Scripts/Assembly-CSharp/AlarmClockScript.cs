using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class AlarmClockScript : MonoBehaviour
{
	// Token: 0x06000010 RID: 16 RVA: 0x00002331 File Offset: 0x00000531
	private void Start()
	{
		this.player = GameObject.Find("Player").transform;
		this.SetTime = 30;
		this.timeLeft = 30f;
		this.lifeSpan = 35f;
		this.ClockRender = GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002368 File Offset: 0x00000568
	private void Update()
	{
		if (this.timeLeft >= 0f) //If the time is greater then 0
		{
			this.timeLeft -= Time.deltaTime; //Decrease the time variable
		}
		else if (!this.rang) // If it has not been rang
		{
			this.Alarm(); // Start the alarm function
		}
		if (this.lifeSpan >= 0f) //If the time left in the lifespan is greater then 0
		{
			this.lifeSpan -= Time.deltaTime; //Decrease the time variable
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject, 0f); //Otherwise, if time is less then 0, destroy the alarm clock
		}
		RaycastHit raycastHit;
		if ((Input.GetMouseButtonDown(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f)), out raycastHit) && (raycastHit.collider == this.trigger & Vector3.Distance(this.player.position, base.transform.position) < 15f & !this.rang))
		{
			this.Set(); //Change clock time
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002460 File Offset: 0x00000660
	public void Set()
	{
		if (this.SetTime == 30)
		{
			this.ClockRender.sprite = sprites[2];
			this.SetTime = 45;
			this.timeLeft = 45f; //45 Seconds
			this.lifeSpan = 50f;
		}
		else if (this.SetTime == 45)
		{
			this.ClockRender.sprite = sprites[3];
			this.SetTime = 60;
			this.timeLeft = 60f; //60 Seconds
			this.lifeSpan = 65f;
		}
		else if (this.SetTime == 60)
		{
			this.ClockRender.sprite = sprites[0];
			this.SetTime = 15;
			this.timeLeft = 15f; //15 Seconds
			this.lifeSpan = 20f;
		}
		else if (this.SetTime == 15)
		{
			this.ClockRender.sprite = sprites[1];
			this.SetTime = 30;
			this.timeLeft = 30f; //30 Seconds
			this.lifeSpan = 35f;
		}
		this.WindAud.Play();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002564 File Offset: 0x00000764
	private void Alarm()
	{
		this.rang = true;
		base.gameObject.tag = "Untagged";
		if (this.baldi.isActiveAndEnabled)
		{
			this.baldi.Hear(base.transform.position, 8f);
		}
		this.audioDevice.clip = this.ring;
		this.audioDevice.loop = false;
		this.audioDevice.Play();
	}

	// Token: 0x0400000C RID: 12
	public CapsuleCollider trigger;

	// Token: 0x0400000D RID: 13
	public float timeLeft;

	// Token: 0x0400000E RID: 14
	private float lifeSpan;

	// Token: 0x0400000F RID: 15
	private bool rang;

	// Token: 0x04000010 RID: 16
	public BaldiScript baldi;

	// Token: 0x04000011 RID: 17
	public AudioClip ring;

	// Token: 0x04000012 RID: 18
	public AudioSource audioDevice;

	// Token: 0x04000013 RID: 19
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x04000014 RID: 20
	public Transform player;

	// Token: 0x04000015 RID: 21
	public int SetTime;

	// Token: 0x04000016 RID: 22
	public SpriteRenderer ClockRender;

	// Token: 0x04000017 RID: 23
	public AudioSource WindAud;
}
