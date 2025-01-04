using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("AnalogMove") == 1)
        {
            this.sensitivityActive = true;
        }
        this.height = base.transform.position.y;
        this.stamina = this.maxStamina;
        this.playerRotation = base.transform.rotation;
        this.mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        this.principalBugFixer = 1;
        this.flipaturn = 1f;
        this.m_Camera = Camera.main;
        this.cameraScript = m_Camera.GetComponent<CameraScript>();
    }

    private void Update()
    {
        base.transform.position = new Vector3(base.transform.position.x, this.height, base.transform.position.z);
        this.MouseMove();
        this.PlayerMove();
        this.StaminaCheck();
        this.GuiltCheck();
        if (this.cc.velocity.magnitude > 0f)
        {
            this.gc.LockMouse();
        }
        if (this.jumpRope && (base.transform.position - frozenPosition).magnitude >= 1f && cameraScript.jumpHeight < 0.1f)
        {
            this.DeactivateJumpRope();
            this.playtime.Disappoint();
        }
        if (this.sweepingFailsave > 0f)
        {
            this.sweepingFailsave -= Time.deltaTime;
        }
        else
        {
            this.sweeping = false;
            this.hugging = false;
        }
    }

    private void MouseMove()
    {
        this.playerRotation.eulerAngles = new Vector3(this.playerRotation.eulerAngles.x, this.playerRotation.eulerAngles.y, this.fliparoo);
        this.playerRotation.eulerAngles += Vector3.up * Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.timeScale * this.flipaturn;
        base.transform.rotation = this.playerRotation;
    }

    private void PlayerMove()
    {
        Vector3 vector = Vector3.zero;
        Vector3 vector2 = Vector3.zero;
        if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveForward))
        {
            vector = base.transform.forward;
        }
        if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveBackward))
        {
            vector = -base.transform.forward;
        }
        if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveLeft))
        {
            vector2 = -base.transform.right;
        }
        if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveRight))
        {
            vector2 = base.transform.right;
        }

        if (this.stamina > 0f && Singleton<InputManager>.Instance.GetActionKey(InputAction.Run))
        {
            this.playerSpeed = this.runSpeed;
            this.sensitivity = 1f;
            if (this.cc.velocity.magnitude > 0.1f && !this.hugging && !this.sweeping)
            {
                this.ResetGuilt("running", 0.1f);
            }
        }
        else
        {
            this.playerSpeed = this.walkSpeed;
            this.sensitivity = this.sensitivityActive ? Mathf.Clamp((vector2 + vector).magnitude, 0f, 1f) : 1f;
        }

        this.playerSpeed *= Time.deltaTime;
        this.moveDirection = (vector + vector2).normalized * this.playerSpeed * this.sensitivity;

        if (this.sweeping && !this.bootsActive)
        {
            this.moveDirection += this.gottaSweep.velocity * Time.deltaTime * 0.3f;
        }
        else if (this.hugging && !this.bootsActive)
        {
            this.moveDirection = (this.firstPrize.velocity * 1.2f * Time.deltaTime + (new Vector3(this.firstPrizeTransform.position.x, this.height, this.firstPrizeTransform.position.z) + new Vector3((float)Mathf.RoundToInt(this.firstPrizeTransform.forward.x), 0f, (float)Mathf.RoundToInt(this.firstPrizeTransform.forward.z)) * 3f - base.transform.position)) * this.principalBugFixer;
        }
        else if (jumpRope && cameraScript.jumpHeight > 0.1f)
        {
            this.moveDirection *= jumpRopeSpeedMultiplier;
        }

        this.cc.Move(this.moveDirection);

        if ((!sweeping || bootsActive) && (!hugging || bootsActive) && jumpRope && cameraScript.jumpHeight > 0.1f)
        {
            frozenPosition = transform.position;
        }
    }

    private void StaminaCheck()
    {
        if (this.cc.velocity.magnitude > 0.1f)
        {
            if (Singleton<InputManager>.Instance.GetActionKey(InputAction.Run) && this.stamina > 0f)
            {
                this.stamina -= this.staminaRate * Time.deltaTime;
            }
            if (this.stamina < 0f && this.stamina > -5f)
            {
                this.stamina = -5f;
            }
        }
        else if (this.stamina < this.maxStamina)
        {
            this.stamina += this.staminaRate * Time.deltaTime;
        }
        this.staminaBar.value = this.stamina / this.maxStamina * 100f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Baldi" && !this.gc.debugMode)
        {
            this.gameOver = true;
            RenderSettings.skybox = this.blackSky;
            StartCoroutine(this.KeepTheHudOff());
        }
        else if (other.transform.name == "Playtime" && !this.jumpRope && this.playtime.playCool <= 0f)
        {
            this.ActivateJumpRope();
        }
        if (this.gc.item[0] != 15 && this.gc.item[1] != 15 && this.gc.item[2] != 15)
        {
            if (other.transform.name == "Baldi" && !this.gc.debugMode && !this.baldi.AppleEating)
            {
                this.gameOver = true;
            }
        }
        else if (other.transform.name == "Baldi" && !this.baldi.AppleEating)
        {
            this.Apple();
            this.AppleLose = false;
        }
    }

    private IEnumerator KeepTheHudOff()
    {
        while (this.gameOver)
        {
            this.hud.enabled = false;
            this.jumpRopeScreen.SetActive(false);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "Gotta Sweep")
        {
            this.sweeping = true;
            this.sweepingFailsave = 1f;
        }
        else if (other.transform.name == "1st Prize" && this.firstPrize.velocity.magnitude > 5f)
        {
            this.hugging = true;
            this.sweepingFailsave = 1f;
        }
        if (other.name == "Gum" && other.GetComponentInChildren<SpriteRenderer>().sprite != BeansScript.spriteNPCGum)
        {
            StartCoroutine(Stucked());
            Destroy(other.gameObject);
            FindObjectOfType<BeansScript>().SorryPlayer();
        }
        if (this.gc.item[0] != 15 && this.gc.item[1] != 15 && this.gc.item[2] != 15)
        {
            if (other.transform.name == "Baldi" && !this.gc.debugMode && !this.baldi.AppleEating && !this.gameOver)
            {
                this.gameOver = true;
                RenderSettings.skybox = this.blackSky;
                StartCoroutine(this.KeepTheHudOff());
            }
        }
        else if (other.transform.name == "Baldi" && !this.baldi.AppleEating)
        {
            this.Apple();
            this.AppleLose = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Office Trigger")
        {
            this.ResetGuilt("escape", this.door.lockTime);
        }
        else if (other.transform.name == "Gotta Sweep")
        {
            this.sweeping = false;
        }
        else if (other.transform.name == "1st Prize")
        {
            this.hugging = false;
        }
    }

    public void ResetGuilt(string type, float amount)
    {
        if (amount >= this.guilt)
        {
            this.guilt = amount;
            this.guiltType = type;
        }
    }

    private void GuiltCheck()
    {
        if (this.guilt > 0f)
        {
            this.guilt -= Time.deltaTime;
        }
    }

    public void ActivateJumpRope()
    {
        this.jumpRopeScreen.SetActive(true);
        this.jumpRope = true;
        this.frozenPosition = base.transform.position;
    }

    public void DeactivateJumpRope()
    {
        this.jumpRopeScreen.SetActive(false);
        this.jumpRope = false;
    }

    public void ActivateBoots()
    {
        this.bootsActive = true;
        StartCoroutine(this.BootTimer());
    }

    private IEnumerator BootTimer()
    {
        float time = 60f;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        this.bootsActive = false;
    }

    private IEnumerator Stucked()
    {
        gumScreen.SetActive(true);
        walkSpeed -= 8f;
        runSpeed -= 8f;
        playerSpeed = walkSpeed;
        yield return new WaitForSeconds(10f);
        walkSpeed += 8f;
        runSpeed += 8f;
        playerSpeed = walkSpeed;
        gumScreen.SetActive(false);
    }

    public void Apple()
    {
        this.baldi.Apple();
        for (int i = 0; i < 5; i++)
        {
            if (this.gc.item[i] == 15 && !this.AppleLose)
            {
                this.gc.LoseItem(i);
                this.AppleLose = true;
                break;
            }
        }
    }


	// Token: 0x040006E9 RID: 1769
	public GameControllerScript gc;

	// Token: 0x040006EA RID: 1770
	public BaldiScript baldi;

	// Token: 0x040006EB RID: 1771
	public DoorScript door;

	// Token: 0x040006EC RID: 1772
	public PlaytimeScript playtime;

	// Token: 0x040006ED RID: 1773
	public bool gameOver;

	// Token: 0x040006EE RID: 1774
	public bool jumpRope;

	// Token: 0x040006EF RID: 1775
	public bool sweeping;

	// Token: 0x040006F0 RID: 1776
	public bool hugging;

	// Token: 0x040006F1 RID: 1777
	public bool bootsActive;

	// Token: 0x040006F2 RID: 1778
	public int principalBugFixer;

	// Token: 0x040006F3 RID: 1779
	public float sweepingFailsave;

	// Token: 0x040006F4 RID: 1780
	public float fliparoo;

	// Token: 0x040006F5 RID: 1781
	public float flipaturn;

	// Token: 0x040006F6 RID: 1782
	private Quaternion playerRotation;

	// Token: 0x040006F7 RID: 1783
	public Vector3 frozenPosition;

	// Token: 0x040006F8 RID: 1784
	private bool sensitivityActive;

	// Token: 0x040006F9 RID: 1785
	private float sensitivity;

	// Token: 0x040006FA RID: 1786
	public float mouseSensitivity;

	// Token: 0x040006FB RID: 1787
	public float walkSpeed;

	// Token: 0x040006FC RID: 1788
	public float runSpeed;

	// Token: 0x040006FD RID: 1789
	public float slowSpeed;

	// Token: 0x040006FE RID: 1790
	public float maxStamina;

	// Token: 0x040006FF RID: 1791
	public float staminaRate;

	// Token: 0x04000700 RID: 1792
	public float guilt;

	// Token: 0x04000701 RID: 1793
	public float initGuilt;

	// Token: 0x04000704 RID: 1796
	private Vector3 moveDirection;

	// Token: 0x04000705 RID: 1797
	private float playerSpeed;

	// Token: 0x04000706 RID: 1798
	public float stamina;

	// Token: 0x04000707 RID: 1799
	public CharacterController cc;

	// Token: 0x04000708 RID: 1800
	public NavMeshAgent gottaSweep;

	// Token: 0x04000709 RID: 1801
	public NavMeshAgent firstPrize;

	// Token: 0x0400070A RID: 1802
	public Transform firstPrizeTransform;

	// Token: 0x0400070B RID: 1803
	public Slider staminaBar;

	// Token: 0x0400070C RID: 1804
	public float db;

	// Token: 0x0400070D RID: 1805
	public string guiltType;

	// Token: 0x0400070E RID: 1806
	public GameObject jumpRopeScreen;

	// Token: 0x04000710 RID: 1808
	public float height;

	// Token: 0x04000711 RID: 1809
	public Material blackSky;

	// Token: 0x04000712 RID: 1810
	public Canvas hud;

	// Token: 0x04000715 RID: 1813
	[SerializeField] private float jumpRopeSpeedMultiplier;

	// Token: 0x04000716 RID: 1814
    private Camera m_Camera;

	// Token: 0x04000717 RID: 1815
    private CameraScript cameraScript;
    public bool AppleLose;
    public bool holdingObject;
    public bool grapping;
    public GameObject gumScreen;
}
