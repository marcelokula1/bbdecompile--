using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public struct BaseItem 
{
  public string ItemName; 
  public Sprite ItemTexture; 
}


// Token: 0x020000C0 RID: 192
public class GameControllerScript : MonoBehaviour
{
	// Token: 0x06000964 RID: 2404 RVA: 0x00021AC4 File Offset: 0x0001FEC4
	private void Start()
	{
          UnityEngine.AI.NavMeshAgent agent = principal.GetComponent<UnityEngine.AI.NavMeshAgent>();
		this.cullingMask = this.PlayerCamera.cullingMask; // Changes cullingMask in the Camera
		this.audioDevice = base.GetComponent<AudioSource>(); //Get the Audio Source
		this.audioQueue = base.GetComponent<AudioQueueScript>(); //Get the Audio Source
		this.mode = PlayerPrefs.GetString("CurrentMode"); //Get the current mode
		if (this.mode == "endless") //If it is endless mode
		{
			this.baldiScrpt.endless = true; //Set Baldi use his slightly changed endless anger system
			this.AAC.endless = true; //Set Arts & Crafter anger to be infinite
		}
		this.schoolMusic.Play(); //Play the school music
		this.LockMouse(); //Prevent the mouse from moving
		this.UpdateNotebookCount(); //Update the notebook count
		this.itemSelected = 0; //Set selection to item slot 0(the first item slot)
		this.gameOverDelay = 0.5f;
          if (IsStudentHere == true)
          {
          Student.SetActive(true);
          }
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00021B5C File Offset: 0x0001FF5C
	private void Update()
	{
		if (!this.learningActive)
		{
			if (Singleton<InputManager>.Instance.GetActionKey(InputAction.PauseOrCancel))
			{
				if (!this.gamePaused)
				{
					this.PauseGame();
				}
				else
				{
					this.UnpauseGame();
				}
			}
			if (Input.GetKeyDown(KeyCode.Y) & this.gamePaused)
			{
				this.ExitGame();
			}
			else if (Input.GetKeyDown(KeyCode.N) & this.gamePaused)
			{
				this.UnpauseGame();
			}
			if (!this.gamePaused & Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
			}
			if ((Input.GetMouseButtonDown(1) || Singleton<InputManager>.Instance.GetActionKey(InputAction.UseItem)) && Time.timeScale != 0f)
			{
				this.UseItem();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                this.ChangeItemSelection(-1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                this.ChangeItemSelection(1);
            }
			for (int i = 0; i < this.item.Length; i++)
            {
                bool keyCode = Singleton<InputManager>.Instance.GetActionKey(InputAction.Slot0 + 0 + i);
                if ((keyCode))
                {
                    this.itemSelected = i;
                    this.UpdateItemSelection();
                    break;
                }
            }
		}
		else
		{
			if (Time.timeScale != 0f)
			{
				Time.timeScale = 0f;
			}
		}
		if (this.player.stamina < 0f & !this.warning.activeSelf)
		{
			this.warning.SetActive(true); //Set the warning text to be visible
		}
		else if (this.player.stamina > 0f & this.warning.activeSelf)
		{
			this.warning.SetActive(false); //Set the warning text to be invisible
		}
		if (this.player.gameOver)
		{
			if (this.mode == "endless" && this.notebooks > PlayerPrefs.GetInt("HighBooks") && !this.highScoreText.activeSelf)
			{
				this.highScoreText.SetActive(true);
			}
			Time.timeScale = 0f;
			this.gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
			this.PlayerCamera.farClipPlane = this.gameOverDelay * 400f; //Set camera farClip 
			this.audioDevice.PlayOneShot(this.aud_buzz);
			if (PlayerPrefs.GetInt("Rumble") == 1)
			{

			}
			if (this.gameOverDelay <= 0f)
			{
				if (this.mode == "endless")
				{
					if (this.notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", this.notebooks);
					}
					PlayerPrefs.SetInt("CurrentBooks", this.notebooks);
				}
				Time.timeScale = 1f;
				SceneManager.LoadScene("GameOver");
			}
		}
		if (this.finaleMode && !this.audioDevice.isPlaying && this.exitsReached == 3)
		{
			this.audioDevice.clip = this.aud_MachineLoop;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00021F8C File Offset: 0x0002038C
	private void UpdateNotebookCount()
	{
		if (this.mode == "story")
		{
			this.notebookCount.text = this.notebooks.ToString() + "/" + this.maxNotebooks;
			//this.notebookCount.text = "Notebooks Left: " + (this.maxNotebooks - this.notebooks).ToString();
		}
		else
		{
			this.notebookCount.text = this.notebooks.ToString() + " Notebooks";
			//this.notebookCount.text = "Notebooks Found: " + this.notebooks.ToString();
		}
		if (this.notebooks == this.maxNotebooks & this.mode == "story")
		{
			this.ActivateFinaleMode();
		}
          else if (this.notebooks == this.maxNotebooks & this.mode == "NULL")
		{
			this.ActivateFinaleMode();
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00022024 File Offset: 0x00020424
	public void CollectNotebook()
	{
		this.notebooks++;
		this.UpdateNotebookCount();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0002203A File Offset: 0x0002043A
	public void LockMouse()
	{
		if (!this.learningActive)
		{
			this.cursorController.LockCursor(); //Prevent the cursor from moving
			this.mouseLocked = true;
			this.reticle.SetActive(true);
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00022065 File Offset: 0x00020465
	public void UnlockMouse()
	{
		this.cursorController.UnlockCursor(); //Allow the cursor to move
		this.mouseLocked = false;
		this.reticle.SetActive(false);
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x00022085 File Offset: 0x00020485
	public void PauseGame()
	{
		if (!this.learningActive)
		{
			{
				this.UnlockMouse();
			}
			Time.timeScale = 0f;
			this.gamePaused = true;
			AudioListener.pause = true;
			this.pauseMenu.SetActive(true);
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x000220C5 File Offset: 0x000204C5
	public void ExitGame()
	{
		AudioListener.pause = false;
		SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x000220D1 File Offset: 0x000204D1
	public void UnpauseGame()
	{
		Time.timeScale = 1f;
		this.gamePaused = false;
		AudioListener.pause = false;
		this.pauseMenu.SetActive(false);
		this.LockMouse();
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x000220F8 File Offset: 0x000204F8
public void ActivateSpoopMode()
{
    this.spoopMode = true; //Tells the game its time for spooky
    this.entrance_0.Lower(); //Lower all the exits
    this.entrance_1.Lower();
    this.entrance_2.Lower();
    this.entrance_3.Lower();
    this.baldiTutor.SetActive(false); //Turns off Baldi(The one that you see at the start of the game)
    this.quarter.SetActive(false);
    this.crafters.SetActive(true); //Turns on Crafters
    this.playtime.SetActive(true); //Turns on Playtime
    this.gottaSweep.SetActive(true); //Turns on Gotta Sweep
    this.bully.SetActive(true); //Turns on Bully
    this.firstPrize.SetActive(true); //Turns on First-Prize
    //this.TestEnemy.SetActive(true); //Turns on Test-Enemy
    this.audioDevice.PlayOneShot(this.aud_Hang); //Plays the hang sound
    this.learnMusic.Stop(); //Stop all the music
    this.schoolMusic.Stop();

    if (BaldiSpawns == true)
    {
        this.baldi.SetActive(true); //Turns on Baldi
        this.baldiScrpt.speed = BaldiSpeed;
        this.baldiScrpt.timeToMove = TimeToMoveBaldi;
        this.baldiScrpt.baldiAnger = BaldiAnger;
        this.baldiScrpt.baldiSpeedScale = BaldiSpeedScale;
        if (this.mode == "NULL")
        {
          NullSprite.SetActive(true);
          BaldiSprite.SetActive(false);
          this.quarter.SetActive(false);
          this.crafters.SetActive(false); //Turns on Crafters
          this.playtime.SetActive(false); //Turns on Playtime
          this.gottaSweep.SetActive(false); //Turns on Gotta Sweep
          this.bully.SetActive(false); //Turns on Bully
          this.firstPrize.SetActive(false); //Turns on First-Prize
          //this.TestEnemy.SetActive(false); //Turns on Test-Enemy
        }
    } 
    else 
    {
        Debug.Log("baldi is not active");
    }
    
    if (PrincipalSpawns == true)
    {
        this.principal.SetActive(true); //Turns on Principal
        UnityEngine.AI.NavMeshAgent agent = this.principal.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = PrincipalSpeed; // Adjust the speed of the NavMeshAgent
        }
        else
        {
            Debug.LogWarning("i forgot");
        }
    }
    else 
    {
        Debug.Log("principal is not active");
    }
    if (FloodEvent == true)
    {
     FloodController.SetActive(true);
    }
}


	// Token: 0x0600096E RID: 2414 RVA: 0x000221BF File Offset: 0x000205BF
	private void ActivateFinaleMode()
	{
		this.finaleMode = true;
		this.entrance_0.Raise(); //Raise all the enterances(make them appear)
		this.entrance_1.Raise();
		this.entrance_2.Raise();
		this.entrance_3.Raise();
          if (SchoolhouseEscapeMusicExist == true)
          {
           audioSource = gameObject.AddComponent<AudioSource>();
           audioSource.clip = SchoolhouseEscapeMusic;
           audioSource.volume = 0.5f;
           audioSource.loop = false;
           audioSource.Play();
          }
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x000221F4 File Offset: 0x000205F4
	public void GetAngry(float value) //Make Baldi get angry
	{
		if (!this.spoopMode)
		{
			this.ActivateSpoopMode();
		}
		this.baldiScrpt.GetAngry(value);
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x00022214 File Offset: 0x00020614
	public void ActivateLearningGame()
	{
		//this.camera.cullingMask = 0; //Sets the cullingMask to nothing
		this.learningActive = true;
		this.UnlockMouse(); //Unlock the mouse
		this.tutorBaldi.Stop(); //Make tutor Baldi stop talking
		if (!this.spoopMode) //If the player hasn't gotten a question wrong
		{
			base.StartCoroutine(audioQueue.FadeOut(this.schoolMusic, 0.25f)); //Fade out the school music
			this.learnMusic.Play(); //Start playing the learn music
		}
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x00022278 File Offset: 0x00020678
	public void DeactivateLearningGame(GameObject subject)
	{
		this.PlayerCamera.cullingMask = this.cullingMask; //Sets the cullingMask to Everything
		this.learningActive = false;
		UnityEngine.Object.Destroy(subject);
		this.LockMouse(); //Prevent the mouse from moving
		if (this.player.stamina < 100f) //Reset Stamina
		{
			this.player.stamina = 100f;
		}
		if (!this.spoopMode) //If it isn't spoop mode, play the school music
		{
			this.schoolMusic.Play();
			base.StartCoroutine(audioQueue.FadeOut(this.learnMusic, 0.25f)); 
		}
		if (this.notebooks == 1 & !this.spoopMode) // If this is the players first notebook and they didn't get any questions wrong, reward them with a quarter
		{
			this.quarter.SetActive(true);
			this.tutorBaldi.PlayOneShot(this.aud_Prize);
		}
		else if (this.notebooks == this.maxNotebooks & this.mode == "story") // Plays the all 7 notebook sound
		{
			this.audioDevice.PlayOneShot(this.aud_AllNotebooks, 0.8f);
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x00022360 File Offset: 0x00020760
	private void ChangeItemSelection(int change)
    {
        this.itemSelected += change;
        if (this.itemSelected < 0)
        {
            this.itemSelected = (this.item.Length - 1);
        }
        if (this.itemSelected > (this.item.Length - 1))
        {
            this.itemSelected = 0;
        }
        this.UpdateItemSelection();
    }

	// Token: 0x06000974 RID: 2420 RVA: 0x00022425 File Offset: 0x00020825
	private void UpdateItemSelection()
    {
        for (int i = 0; i < this.itemSlotBgs.Length; i++)
        {
            if (this.itemSlotBgs[i] != this.itemSlotBgs[this.itemSelected])
            {
                this.itemSlotBgs[i].color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                this.itemSlotBgs[i].color = new Color(1f, 0f, 0f, 1f);
            }
        }
        this.UpdateItemName();
    }

	// Token: 0x06000975 RID: 2421 RVA: 0x0002245C File Offset: 0x0002085C
	public void CollectItem(int item_ID)
    {
        for (int i = 0; i < this.item.Length; i++)
        {
            if (this.item[i] == 0)
            {
                this.item[i] = item_ID;
                this.itemSlot[i].texture = this.itemTextures[item_ID];
                this.UpdateItemName();
                return;
            }
        }
        this.item[itemSelected] = item_ID;
        this.itemSlot[itemSelected].texture = this.itemTextures[item_ID];
        this.UpdateItemName();
    }

	// Token: 0x06000976 RID: 2422 RVA: 0x00022528 File Offset: 0x00020928
	private void UseItem()
	{
		if (this.item[this.itemSelected] != 0)
		{
			if (this.item[this.itemSelected] == 1)
			{
				this.player.stamina = this.player.maxStamina * 2f;
				this.audioDevice.PlayOneShot(this.aud_Crunch);
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 2)
			{
				Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
				{
					raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 3)
			{
				Ray ray2 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit2;
				if (Physics.Raycast(ray2, out raycastHit2) && (raycastHit2.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit2.transform.position) <= 10f))
				{
					DoorScript component = raycastHit2.collider.gameObject.GetComponent<DoorScript>();
					if (component.DoorLocked)
					{
						component.UnlockDoor();
						component.OpenDoor();
						this.ResetItem();
					}
				}
			}
			else if (this.item[this.itemSelected] == 4)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.bsodaSpray, this.playerTransform.position, this.cameraTransform.rotation);
				this.ResetItem();
				this.player.ResetGuilt("drink", 1f);
				this.audioDevice.PlayOneShot(this.aud_Soda);
			}
			else if (this.item[this.itemSelected] == 5)
			{
				Ray ray3 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit3;
				if (Physics.Raycast(ray3, out raycastHit3))
				{
					if (raycastHit3.collider.name == "BSODAMachine" & Vector3.Distance(playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						ResetItem();
						this.audioDevice.PlayOneShot(this.aud_Drop);
						CollectItem(4);
						if (this.useEmptyMachine)
                        {
                            raycastHit3.collider.gameObject.name = "EmptyMachine";
                            raycastHit3.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = OutOfBsoda;
                        }
					}
					else if (raycastHit3.collider.name == "ZestyMachine" & Vector3.Distance(playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						ResetItem();
						this.audioDevice.PlayOneShot(this.aud_Drop);
						CollectItem(1);
						if (this.useEmptyMachine)
                        {
                            raycastHit3.collider.gameObject.name = "EmptyMachine";
                            raycastHit3.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = OutOfZesty;
                        }
					}
					else if (raycastHit3.collider.name == "PayPhone" & Vector3.Distance(playerTransform.position, raycastHit3.transform.position) <= 10f)
					{
						raycastHit3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
						this.audioDevice.PlayOneShot(this.aud_Drop);
						ResetItem();
					}
				}
			}
			else if (this.item[this.itemSelected] == 6)
			{
				Ray ray4 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit4;
				if (Physics.Raycast(ray4, out raycastHit4) && (raycastHit4.collider.name == "TapePlayer" & Vector3.Distance(this.playerTransform.position, raycastHit4.transform.position) <= 10f))
				{
					raycastHit4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 7)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.alarmClock, this.playerTransform.position, this.cameraTransform.rotation);
				gameObject.GetComponent<AlarmClockScript>().baldi = this.baldiScrpt;
				this.ResetItem();
			}
			else if (this.item[this.itemSelected] == 8)
			{

				Ray ray5 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit5;

				if (Physics.Raycast(ray5, out raycastHit5) && (raycastHit5.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f))
				{
					raycastHit5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
					this.ResetItem();
					this.audioDevice.PlayOneShot(this.aud_Spray);
				}

                else if (raycastHit5.collider.name == "BlueLocker" && Vector3.Distance(this.playerTransform.position, raycastHit5.transform.position) <= 10f) 
                {
                    BlueLockerScript blueLocker = raycastHit5.collider.gameObject.GetComponent<BlueLockerScript >();
                    blueLocker.silentUses = 4;
                    this.ResetItem();
                    this.audioDevice.PlayOneShot(this.aud_Spray);
                }


			}
			else if (this.item[this.itemSelected] == 9)
			{
				Ray ray6 = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
				RaycastHit raycastHit6;
				if (this.player.jumpRope)
				{
					this.player.DeactivateJumpRope();
					this.playtimeScript.Disappoint();
					this.audioDevice.PlayOneShot(this.aud_Snip);
					this.ResetItem();
				}
				else if (Physics.Raycast(ray6, out raycastHit6) && raycastHit6.collider.name == "1st Prize")
				{
					this.firstPrizeScript.GoCrazy();
					this.audioDevice.PlayOneShot(this.aud_Snip);
					this.ResetItem();
				}
			}
			else if (this.item[this.itemSelected] == 10)
			{
				this.player.ActivateBoots();
				base.StartCoroutine(this.BootAnimation());
				this.ResetItem();
			}
		}
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00022B40 File Offset: 0x00020F40
	private IEnumerator BootAnimation()
	{
		float time = 15f;
		float height = 375f;
		Vector3 position = default(Vector3);
		this.boots.gameObject.SetActive(true);
		while (height > -375f)
		{
			height -= 375f * Time.deltaTime;
			time -= Time.deltaTime;
			position = this.boots.localPosition;
			position.y = height;
			this.boots.localPosition = position;
			yield return null;
		}
		position = this.boots.localPosition;
		position.y = -375f;
		this.boots.localPosition = position;
		this.boots.gameObject.SetActive(false);
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		this.boots.gameObject.SetActive(true);
		while (height < 375f)
		{
			height += 375f * Time.deltaTime;
			position = this.boots.localPosition;
			position.y = height;
			this.boots.localPosition = position;
			yield return null;
		}
		position = this.boots.localPosition;
		position.y = 375f;
		this.boots.localPosition = position;
		this.boots.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00022B5B File Offset: 0x00020F5B
	private void ResetItem()
	{
		this.item[this.itemSelected] = 0;
		this.itemSlot[this.itemSelected].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00022B8B File Offset: 0x00020F8B
	public void LoseItem(int id)
	{
		this.item[id] = 0;
		this.itemSlot[id].texture = this.itemTextures[0];
		this.UpdateItemName();
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00022BB1 File Offset: 0x00020FB1
	private void UpdateItemName()
	{
		this.itemText.text = this.itemNames[this.item[this.itemSelected]];
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00022BD4 File Offset: 0x00020FD4
	public void ExitReached()
	{
		this.exitsReached++;
		if (this.exitsReached == 1)
		{
			this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
			this.audioDevice.clip = this.aud_MachineQuiet;
			this.itemText.color = Color.white;
			this.notebookCount.color = Color.white;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
			RenderSettings.ambientLight = color;
			RenderSettings.skybox = skyBoxRed; //Make skybox red
			//RenderSettings.fog = true;
              if (SchoolhouseEscapeMusicExist == true)
              { 
               audioSource.Stop();
              }
              else
              {
               Debug.Log("exit music is not here");
              }
		}
		if (this.exitsReached == 2) //Play a sound
		{
			this.audioDevice.volume = 0.8f;
			this.audioDevice.clip = this.aud_MachineStart;
			this.audioDevice.loop = true;
			this.audioDevice.Play();
		}
		if (this.exitsReached == 3) //Play a even louder sound
		{
			this.audioDevice.clip = this.aud_MachineRev;
			this.audioDevice.loop = false;
			this.audioDevice.Play();
		}
        if (this.exitsReached == 4)
        {
            this.audioDevice.Stop();
            RenderSettings.ambientLight = Color.white; // IF YOU DONT WANT THE AMBIENCE COLOR OF YOUR SCENE CHANGE, REMOVE THIS LINE
            this.audioDevice.PlayOneShot(this.aud_Switch, 0.8f);
            this.BossFightStart();
        }
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00022CC1 File Offset: 0x000210C1
	public void DespawnCrafters()
	{
		this.crafters.SetActive(false); //Make Arts And Crafters Inactive
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00022CD0 File Offset: 0x000210D0
	public void Fliparoo()
	{
		this.player.height = 6f;
		this.player.fliparoo = 180f;
		this.player.flipaturn = -1f;
		Camera.main.GetComponent<CameraScript>().offset = new Vector3(0f, -1f, 0f);
	}
    public void BossFightStart()
    {
        this.BossFight = true;
        this.debugMode = true;
        this.player.runSpeed = this.player.walkSpeed;
        this.baldiScrpt.enabled = false;
        this.cs.baldiAgent.Warp(new Vector3(-35f, this.cs.baldi.position.y, 335f)); // Teleport Baldi to X: -35, baldi's Y, Z: 335
        this.ns.agent.isStopped = true;
        this.audioDevice.PlayOneShot(this.NullPreIntro);
        this.bossFightMusic.clip = this.Preintro_BossM;
        this.bossFightMusic.loop = true;
        this.bossFightMusic.Play();
        GameObject[] Items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject Item in Items)
        {
            Item.SetActive(false);
        }
        this.player.hud.enabled = false;
        this.LoseItem(0);
        this.LoseItem(1);
        this.LoseItem(2);
    }
public void PlayerIncrease(float setSpeed)
    {
        this.player.walkSpeed += setSpeed;
        this.player.runSpeed += setSpeed;
    }
private void BossFightBegin()
    {
        this.ns.agent.speed = 20f;
        this.BossFight = true;
        this.ns.agent.isStopped = false;
        this.debugMode = false;
        this.ns.enabled = true;
        this.bossFightMusic.clip = this.Boss_LoopMusic;
        this.bossFightMusic.loop = true;
        this.bossFightMusic.Play();
        this.player.walkSpeed = 19f;
        this.player.runSpeed = 19f;
    }
public void NullHit()
{
    this.health--;

    // Check if health is at half
    if (this.health == healthAfterHit / 2 &)
    {
        TransparentMaterial("Ceiling", transparent);
        TransparentMaterial("Floor", transparent);
    }

    if (this.health == healthAfterHit)
    {
        this.BossFight = false;
        this.audioDevice.Stop();
        this.audioDevice.PlayOneShot(this.NULL_Start);
        this.bossFightMusic.clip = this.Start_BossMusic;
        this.bossFightMusic.loop = true;
        this.bossFightMusic.Play();
        GameObject[] Projetiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject Projectile in Projetiles)
        {
            Destroy(Projectile);
        }
        this.player.holdingObject = false;
        base.StartCoroutine(this.AfterBossStart());
        return;
    }
    if (this.health < healthAfterHit)
    {
        if (this.BossFight)
        {
            PlayerIncrease(4f);
        }
        if (this.health == 1)
        {
            GameObject[] Projetiles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject Projectile in Projetiles)
            {
                Destroy(Projectile);
            }
            this.ProjectileSpawnr.maxObjects = 1f;
            this.ProjectileSpawnr.objects = 0f;
            this.ProjectileSpawnr.spawnCooldown = 5f;
            this.player.holdingObject = false;
        }
        if (this.health <= 0)
        {
            SceneManager.LoadScene("END"); // Change this to the scene you want to end up at after the fight is over!
        }
    }
}

private IEnumerator AfterBossStart()
    {
        yield return new WaitForSeconds(Start_BossMusic.length);
        this.BossFightBegin();
        yield break;
    }


private void TransparentMaterial(string layerName, Material material)
{
    int targetLayer = LayerMask.NameToLayer(layerName);
    GameObject[] allObjects = FindObjectsOfType<GameObject>();

    foreach (GameObject obj in allObjects)
    {
        if (obj.layer == targetLayer)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material = newMaterial;
            }
        }
    }
}

	// Token: 0x040005F7 RID: 1527
	public CursorControllerScript cursorController;

	// Token: 0x040005F8 RID: 1528
	public PlayerScript player;

	// Token: 0x040005F9 RID: 1529
	public Transform playerTransform;

	// Token: 0x040005FA RID: 1530
	public Transform cameraTransform;

	// Token: 0x040005FB RID: 1531
	public Camera PlayerCamera;

	// Token: 0x040005FC RID: 1532
	private int cullingMask;

	// Token: 0x040005FD RID: 1533
	public EntranceScript entrance_0;

	// Token: 0x040005FE RID: 1534
	public EntranceScript entrance_1;

	// Token: 0x040005FF RID: 1535
	public EntranceScript entrance_2;

	// Token: 0x04000600 RID: 1536
	public EntranceScript entrance_3;


	// Token: 0x0400060F RID: 1551
	public GameObject quarter;

	// Token: 0x04000611 RID: 1553
	public RectTransform boots;

	// Token: 0x04000612 RID: 1554
	public string mode;

	// Token: 0x04000613 RID: 1555
	public int notebooks;

	// Token: 0x04000724 RID: 8723
	public int maxNotebooks;

	// Token: 0x04000614 RID: 1556
	public GameObject[] notebookPickups;

	// Token: 0x04000615 RID: 1557
	public int failedNotebooks;

	// Token: 0x04000616 RID: 1558
	public bool spoopMode;

	// Token: 0x04000617 RID: 1559
	public bool finaleMode;

	// Token: 0x04000618 RID: 1560
	public bool debugMode;

	// Token: 0x04000619 RID: 1561
	public bool mouseLocked;

	// Token: 0x0400061A RID: 1562
	public int exitsReached;

	// Token: 0x04000622 RID: 1570
	public GameObject bsodaSpray;

	// Token: 0x04000623 RID: 1571
	public GameObject alarmClock;

	// Token: 0x04000624 RID: 1572
	public TMP_Text notebookCount;

	// Token: 0x04000625 RID: 1573
	public GameObject pauseMenu;

	// Token: 0x04000626 RID: 1574
	public GameObject highScoreText;

	// Token: 0x04000627 RID: 1575
	public GameObject warning;

	// Token: 0x04000628 RID: 1576
	public GameObject reticle;

	// Token: 0x0400062B RID: 1579
	private bool gamePaused;

	// Token: 0x0400062C RID: 1580
	private bool learningActive;

	// Token: 0x0400062D RID: 1581
	private float gameOverDelay;


	// Token: 0x0400063A RID: 1594
	public Material skyBoxRed;



	// Token: 0x0400063G RID: 1600
	public Material OutOfBsoda;

	// Token: 0x0400063H RID: 1601
	public Material OutOfZesty;

	// Token: 0x0400063I RID: 1602
	private AudioQueueScript audioQueue;

	// Token: 0x0400063J RID: 1603
	public bool useEmptyMachine = false;

	// Token: 0x0400063K RID: 1604
	//private Player playerInput;
    [Header("Game Settings")]

	// Token: 0x04000601 RID: 1537
	public GameObject baldiTutor;

	// Token: 0x04000602 RID: 1538
	public GameObject baldi;

	// Token: 0x04000603 RID: 1539
	public BaldiScript baldiScrpt;

	// Token: 0x04000607 RID: 1543
	public GameObject principal;

	// Token: 0x04000608 RID: 1544
	public GameObject crafters;

	// Token: 0x04000609 RID: 1545
	public GameObject playtime;

	// Token: 0x0400060A RID: 1546
	public PlaytimeScript playtimeScript;

	// Token: 0x0400060B RID: 1547
	public GameObject gottaSweep;

	// Token: 0x0400060C RID: 1548
	public GameObject bully;

	// Token: 0x0400060D RID: 1549
	public GameObject firstPrize;

	// Token: 0x0400060D RID: 1549
	public GameObject TestEnemy;

	// Token: 0x0400060E RID: 1550
	public FirstPrizeScript firstPrizeScript;

	// Token: 0x0400063B RID: 1595
	public CraftersScript AAC;

     public Image Notebooks;

     public bool SkipYCTP;
    

    [Header("Baldi Settings")]

     public bool BaldiSpawns;
     public float BaldiSpeed = 75f;
     public float TimeToMoveBaldi = 0f;
     public float BaldiAnger = 0f;
     public float BaldiSpeedScale = 0.65f;

    [Header("Principal Of the thing Settings")]

    public bool PrincipalSpawns;
    public float PrincipalSpeed;
    
    [Header("Item Stuff")]          // item 

	public string[] itemNames = new string[]
	{
		"Nothing",
		"Energy flavored Zesty Bar",
		"Swinging Door Lock",
		"Principal's Keys",
		"BSODA",
		"Quarter",
		"Baldi's Least Favorite Tape",
		"Alarm Clock",
		"WD-NoSquee",
		"Safety Scissors",
		"Big Ol' Boots"
	};

	// Token: 0x0400063F RID: 1599
	public Texture[] BigItemTextures = new Texture[10];
     
     	// Token: 0x0400061B RID: 1563
	public int itemSelected;

	// Token: 0x0400061C RID: 1564
	public int[] item = new int[3];

	// Token: 0x0400061D RID: 1565
	public RawImage[] itemSlot = new RawImage[3];

	// Token: 0x0400062A RID: 1578
	public Image[] itemSlotBgs = new Image[3];

	// Token: 0x0400061F RID: 1567
	public TMP_Text itemText;

	// Token: 0x04000621 RID: 1569
	public Texture[] itemTextures = new Texture[10];

    [Header("Audio Stuff")]                // audio

   	// Token: 0x0400063C RID: 1596
	public AudioClip aud_Crunch;

	// Token: 0x0400063D RID: 1597
	public AudioClip aud_Snip;

	// Token: 0x0400063E RID: 1598
	public AudioClip aud_Drop;

	// Token: 0x04000604 RID: 1540
	public AudioClip aud_Prize;

	// Token: 0x04000606 RID: 1542
	public AudioClip aud_AllNotebooks;
	// Token: 0x0400062E RID: 1582
	private AudioSource audioDevice;

	// Token: 0x0400062F RID: 1583
	public AudioClip aud_Soda;

	// Token: 0x04000630 RID: 1584
	public AudioClip aud_Spray;

	// Token: 0x04000631 RID: 1585
	public AudioClip aud_buzz;

	// Token: 0x04000632 RID: 1586
	public AudioClip aud_Hang;

	// Token: 0x04000633 RID: 1587
	public AudioClip aud_MachineQuiet;

	// Token: 0x04000634 RID: 1588
	public AudioClip aud_MachineStart;

	// Token: 0x04000635 RID: 1589
	public AudioClip aud_MachineRev;

	// Token: 0x04000636 RID: 1590
	public AudioClip aud_MachineLoop;

	// Token: 0x04000637 RID: 1591
	public AudioClip aud_Switch;

	// Token: 0x04000638 RID: 1592
	public AudioSource schoolMusic;

	// Token: 0x04000639 RID: 1593
	public AudioSource learnMusic;

	// Token: 0x04000610 RID: 1552
	public AudioSource tutorBaldi;

public BaseItem[] items;

    [Header("Bonus Settings")]

    public bool IsStudentHere;
    public GameObject Student;

    public bool ElevatorExits;
    
    public bool FloodEvent;
    public GameObject FloodController;
 
    public bool SchoolhouseEscapeMusicExist;
    public AudioClip SchoolhouseEscapeMusic;
    public Color color;
    AudioSource audioSource; 
    
    [Header("NULL Settings")]  

     public CraftersScript cs;

     public ProjectileSpawner ProjectileSpawnr;

     public NullScript ns;

     public int health;
     public int healthAfterHit;

     public bool BossFight;

     public AudioClip NullPreIntro;

     public AudioClip NULL_Start;

     public AudioClip Preintro_BossM;

     public AudioClip Start_BossMusic;

     public AudioClip Boss_LoopMusic;

     public AudioSource bossFightMusic;
     
     public GameObject NullSprite;
     public GameObject BaldiSprite;
     public Material transparent;
     public NullScript NullScript;
}
