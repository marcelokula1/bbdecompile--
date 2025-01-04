using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020000C9 RID: 201
public class BaldiScript : MonoBehaviour
{
    private void Start()
    {
        this.baldiAudio = GetComponent<AudioSource>();
        this.agent = GetComponent<NavMeshAgent>();
        this.timeToMove = this.baseTime;
        this.Wander();

        if (PlayerPrefs.GetInt("Rumble") == 1)
        {
            this.rumble = true;
        }
    }

    private void Update()
    {
        if (this.timeToMove > 0f)
        {
            this.timeToMove -= Time.deltaTime;
        }
        else
        {
            this.Move();
        }

        if (this.coolDown > 0f)
        {
            this.coolDown -= Time.deltaTime;
        }

        if (this.baldiTempAnger > 0f)
        {
            this.baldiTempAnger -= 0.02f * Time.deltaTime;
        }
        else
        {
            this.baldiTempAnger = 0f;
        }

        if (this.antiHearingTime > 0f)
        {
            this.antiHearingTime -= Time.deltaTime;
        }
        else
        {
            this.antiHearing = false;
        }

        if (this.endless)
        {
            if (this.timeToAnger > 0f)
            {
                this.timeToAnger -= Time.deltaTime;
            }
            else
            {
                this.timeToAnger = this.angerFrequency;
                this.GetAngry(this.angerRate);
                this.angerRate += this.angerRateRate;
            }
        }

        if (this.EatingSoundDelay > 0f && this.AppleEating && this.startedEating && this.gc.mode == "Story")
        {
            this.EatingSoundDelay -= Time.deltaTime;
        }

        if (this.EatingSoundDelay <= 0f && this.AppleEating)
        {
            int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
            this.baldiAudio2.PlayOneShot(this.BAL_Crunch[num]);
            this.times = Mathf.Round(UnityEngine.Random.Range(0f, 50f));
            if (this.times <= 2f)
            {
                this.baldiAudio2.PlayOneShot(this.BAL_Yum);
            }
            this.EatingSoundDelay = 0.05f;
        }
    }

    private void FixedUpdate()
    {
        if (this.moveFrames > 0f)
        {
            this.moveFrames -= 1f;
            this.agent.speed = this.speed;
        }
        else
        {
            this.agent.speed = 0f;
        }

        Vector3 direction = this.player.position - transform.position;
        if (Physics.Raycast(transform.position + Vector3.up * 2f, direction, out RaycastHit hit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && hit.transform.CompareTag("Player"))
        {
            this.db = true;
            this.TargetPlayer();
        }
        else
        {
            this.db = false;
        }
    }

    private void Wander()
    {
        this.wanderer.GetNewTarget();
        this.agent.SetDestination(this.wanderTarget.position);
        this.coolDown = 1f;
        this.currentPriority = 0f;
    }

    public void TargetPlayer()
    {
        this.agent.SetDestination(this.player.position);
        this.coolDown = 1f;
        this.currentPriority = 0f;
    }

    private void Move()
    {
        if (transform.position == this.previous && this.coolDown < 0f)
        {
            this.Wander();
        }
        this.moveFrames = 10f;
        this.timeToMove = this.baldiWait - this.baldiTempAnger;
        this.previous = transform.position;
        this.baldiAudio.PlayOneShot(this.slap);
        this.baldiAnimator.SetTrigger("slap");
        if (this.rumble)
        {
            float distance = Vector3.Distance(transform.position, this.player.position);
            if (distance < this.vibrationDistance)
            {
                float motorLevel = 1f - distance / this.vibrationDistance;
            }
        }
        this.timeToMove = this.baldiWait - this.baldiTempAnger;
        this.AppleEating = false;
        this.startedEating = false;
        this.baldiAnimator.SetBool("EatingApple", false);
        this.previous = transform.position;
    }

    public void GetAngry(float value)
    {
        this.baldiAnger += value;
        if (this.baldiAnger < 0.5f)
        {
            this.baldiAnger = 0.5f;
        }
        this.baldiWait = -3f * this.baldiAnger / (this.baldiAnger + 2f / this.baldiSpeedScale) + 3f;
    }

    public void GetTempAngry(float value)
    {
        this.baldiTempAnger += value;
    }

    public void Hear(Vector3 soundLocation, float priority)
    {
        if (!this.antiHearing && priority >= this.currentPriority)
        {
            this.agent.SetDestination(soundLocation);
            this.currentPriority = priority;
            this.Baldicator.Play("Baldicator_Look", -1, 0f);
        }
        else
        {
            this.Baldicator.Play("Baldicator_Think", -1, 0f);
        }
    }

    public void ActivateAntiHearing(float t)
    {
        this.Wander();
        this.antiHearing = true;
        this.antiHearingTime = t;
    }

    public void Apple()
    {
        this.baldiAnimator.SetTrigger("Apple");
        this.timeToMove = Mathf.RoundToInt(UnityEngine.Random.Range(13, 18));
        this.baldiAudio.PlayOneShot(this.BAL_Apple);
        this.AppleEating = true;
        this.EatingSoundDelay = 0.01f;
    }


	// Token: 0x0400067F RID: 1663
	public bool db;

	// Token: 0x04000680 RID: 1664
	public float baseTime;

	// Token: 0x04000681 RID: 1665
	public float speed;

	// Token: 0x04000682 RID: 1666
	public float timeToMove;

	// Token: 0x04000683 RID: 1667
	public float baldiAnger;

	// Token: 0x04000684 RID: 1668
	public float baldiTempAnger;

	// Token: 0x04000685 RID: 1669
	public float baldiWait;

	// Token: 0x04000686 RID: 1670
	public float baldiSpeedScale;

	// Token: 0x04000687 RID: 1671
	private float moveFrames;

	// Token: 0x04000688 RID: 1672
	private float currentPriority;

	// Token: 0x04000689 RID: 1673
	public bool antiHearing;

	// Token: 0x0400068A RID: 1674
	public float antiHearingTime;

	// Token: 0x0400068B RID: 1675
	public float vibrationDistance;

	// Token: 0x0400068C RID: 1676
	public float angerRate;

	// Token: 0x0400068D RID: 1677
	public float angerRateRate;

	// Token: 0x0400068E RID: 1678
	public float angerFrequency;

	// Token: 0x0400068F RID: 1679
	public float timeToAnger;

	// Token: 0x04000690 RID: 1680
	public bool endless;

	// Token: 0x04000691 RID: 1681
	public Transform player;

	// Token: 0x04000692 RID: 1682
	public Transform wanderTarget;

	// Token: 0x04000693 RID: 1683
	public AILocationSelectorScript wanderer;

	// Token: 0x04000694 RID: 1684
	private AudioSource baldiAudio;

	// Token: 0x04000695 RID: 1685
	public AudioClip slap;

	// Token: 0x04000696 RID: 1686
	public AudioClip[] speech = new AudioClip[3];

	// Token: 0x04000697 RID: 1687
	public Animator baldiAnimator;

	// Token: 0x04000698 RID: 1688
	public float coolDown;

	// Token: 0x04000699 RID: 1689
	private Vector3 previous;

	// Token: 0x0400069A RID: 1690
	private bool rumble;

	// Token: 0x0400069B RID: 1691
	private NavMeshAgent agent;

     public Animator Baldicator;
     public GameControllerScript gc;
     public bool done;

    public bool AppleEating;    
    
    public bool startedEating;

    public float times;

    public float EatingSoundDelay;

    public AudioSource baldiAudio2;

    public AudioClip BAL_Apple;

    public AudioClip BAL_Yum;

    public AudioClip[] BAL_Crunch = new AudioClip[2];


}
