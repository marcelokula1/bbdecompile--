using System;
//using Rewired;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class NotebookScript : MonoBehaviour
{
	// Token: 0x0600098F RID: 2447 RVA: 0x00023FFB File Offset: 0x000223FB
	private void Start()
	{
		//this.playerInput = ReInput.players.GetPlayer(0);
		this.up = true;
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x00024018 File Offset: 0x00022418
	private void Update()
	{
		if (this.gc.mode == "endless")
		{
			if (this.respawnTime > 0f)
			{
				if ((base.transform.position - this.player.position).magnitude > 60f)
				{
					this.respawnTime -= Time.deltaTime;
				}
			}
			else if (!this.up)
			{
				base.transform.position = new Vector3(base.transform.position.x, 4f, base.transform.position.z);
				this.up = true;
				this.audioDevice.Play();
			}
		}
		if ((Input.GetMouseButtonDown(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)) && Time.timeScale != 0f & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && (raycastHit.transform.tag == "Notebook"))
			{
				base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
				this.up = false;
				this.respawnTime = 120f;
				this.gc.CollectNotebook();
				if (this.noMath)
                {
					this.gc.player.stamina = 100;

					if (this.gc.notebooks == 1 & !this.gc.spoopMode)
		            {
			            this.gc.quarter.SetActive(true);
			            this.gc.tutorBaldi.PlayOneShot(this.gc.aud_Prize);
		            }

				    if (this.gc.notebooks == 2)
                    {
                        this.gc.ActivateSpoopMode();
                    }

					if (this.gc.spoopMode)
					{
                        this.bsc.GetAngry(1f);
                    }

                    if (this.gc.notebooks == this.gc.maxNotebooks & this.gc.mode == "story")
                    {

                        this.audioDevice.PlayOneShot(this.gc.aud_AllNotebooks, 0.8f);
                    }
				}
				else
				{
                      if (this.gc.SkipYCTP == false)
                      {
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.learningGame);
				    gameObject.GetComponent<MathGameScript>().gc = this.gc;
				    gameObject.GetComponent<MathGameScript>().baldiScript = this.bsc;
				    gameObject.GetComponent<MathGameScript>().playerPosition = this.player.position;
                      }
                     else 
                      {
                     gc.DeactivateLearningGame(gameObject);
				    if (this.gc.notebooks == 2)
                    {
                        this.gc.ActivateSpoopMode();
                    }

					if (this.gc.spoopMode)
					{
                        this.bsc.GetAngry(1f);
                    }
                      }
				}
			}
		}
	}

	// Token: 0x0400066A RID: 1642
	public float openingDistance;

	// Token: 0x0400066B RID: 1643
	public GameControllerScript gc;

	// Token: 0x0400066C RID: 1644
	public BaldiScript bsc;

	// Token: 0x0400066D RID: 1645
	public float respawnTime;

	// Token: 0x0400066E RID: 1646
	public bool up;

	// Token: 0x0400066F RID: 1647
	public Transform player;

	// Token: 0x04000670 RID: 1648
	public GameObject learningGame;

	// Token: 0x04000671 RID: 1649
	public AudioSource audioDevice;

	// Token: 0x04000672 RID: 1650
	public bool noMath = false;

	// Token: 0x04000673 RID: 1651
	//private Player playerInput;
}
