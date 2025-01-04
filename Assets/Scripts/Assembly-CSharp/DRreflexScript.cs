using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DRreflexScript : MonoBehaviour
{
    public float wanderSpeed = 3.5f;
    public float chaseSpeed = 5.5f;
    public float coolDownTime = 30f;
    public AudioClip touchSound;
    public AudioSource audioSource;
    public Transform player;
    public AILocationSelectorScript wanderer;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool isCoolingDown = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Wander();
    }

    private void Update()
    {
        if (!isChasing && !isCoolingDown)
        {
            Vector3 direction = player.position - transform.position;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, float.PositiveInfinity) && hit.transform.CompareTag("Player"))
            {
                StartChase();
            }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Wander();
            }
        }
    }

    private void StartChase()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private void Wander()
    {
        agent.speed = wanderSpeed;
        wanderer.GetNewTargetHallway();
        agent.SetDestination(wanderer.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isChasing)
        {
            audioSource.PlayOneShot(touchSound);
            ResetAfterTouch();
        }
    }

    private void ResetAfterTouch()
    {
        isChasing = false;
        agent.speed = wanderSpeed;
        StartCoroutine(CoolDownCoroutine());
    }

    private IEnumerator CoolDownCoroutine()
    {
        isCoolingDown = true;
        Wander();
        yield return new WaitForSeconds(coolDownTime);
        isCoolingDown = false;
    }
}
