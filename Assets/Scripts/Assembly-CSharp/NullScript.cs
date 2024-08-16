using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NullScript : MonoBehaviour
{
    public Transform player;

    public NavMeshAgent agent;

    public AudioClip Null_Hit;

    public AudioSource audioDevice;

    public bool IsHit;

    public float coolDown;

    public Sprite norm;

    public Sprite hit;

    public SpriteRenderer sr;

    public GameControllerScript gc;

    private void Start()
    {
        TargetPlayer();
    }

    void Update()
    {
        if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
        TargetPlayer();
    }

    private void FixedUpdate()
    {
        Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
        if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity, 3, QueryTriggerInteraction.Ignore) & raycastHit.transform.tag == "Player")
        {
            this.TargetPlayer();
        }
    }

    private void TargetPlayer()
    {
        agent.SetDestination(this.player.position);
		this.coolDown = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile" && other.GetComponent<ProjectileScript>().Thrown)
        {
            Destroy(other.gameObject);
            this.gc.debugMode = true;
            this.IsHit = true;
            this.sr.sprite = hit;
            this.audioDevice.PlayOneShot(this.Null_Hit);
            this.agent.isStopped = true;
            base.StartCoroutine(AfterHit());
        }
    }

    private IEnumerator AfterHit()
    {
        yield return new WaitForSeconds(Null_Hit.length);
        this.gc.debugMode = false;
        this.IsHit = false;
        this.sr.sprite = norm;
        this.agent.isStopped = false;
        this.agent.speed += 4f;
        this.gc.NullHit();
        yield break;
    }
}
