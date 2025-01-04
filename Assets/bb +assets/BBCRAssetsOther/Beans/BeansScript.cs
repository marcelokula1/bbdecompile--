using UnityEngine;
using UnityEngine.AI;
using System.Collections;
 
public class BeansScript : MonoBehaviour
{
    private NavMeshAgent agent;
 
    private bool chewing;
 
    private float cooldown;
 
    private AudioSource audioDevice;
 
    [SerializeField]
    private AudioClip[] aud_chewing = new AudioClip[5]; // When Beans see the player.
 
    [SerializeField]
    private AudioClip[] aud_skipping = new AudioClip[5]; // When Beans is wandering
 
    [SerializeField]
    private AudioClip[] aud_spitSounds = new AudioClip[5]; // When Beans spit.
 
    [SerializeField]
    private AudioClip[] aud_playerHit = new AudioClip[5]; // When the gum hits the player
 
    [SerializeField]
    private AudioClip[] aud_npcHit = new AudioClip[5]; // When the gum hits the NPC
 
    private AudioClip aud_spit; // When Beans spit.
 
    [SerializeField]
    private GameObject gumPrefab; // The prefab of the gum.
 
    public Sprite npcHitGumSprite; // The sprite when the gum hits the NPC
 
    public static Sprite spriteNPCGum; // This is for to get the sprite of the npcHitGumSprite.
 
    private GameObject gum; // The gum that will be used to modify the game object and store it.
 
    [SerializeField]
    private Animator anim; // The sprite animator
 
    [SerializeField]
    private Transform playerTransform;
 
    private bool SpottedPlayer; // Has Spotted the player? This will be used in the LateUpdate()
 
    private float LastAngularSpeed;
 
    /*
    NOTE TO THE ANIMATION:
    IF THE ANIMATION IS TOO FAST, LOWER THE SAMPLES
    IF THE ANIMATION IS ALREADY PERFECT AT 60 SAMPLES, KEEP IT.
    IF THE ANIMATION IS TOO SLOW, RAISE THE SAMPLES
    */
 
    private void Start()
    {
        spriteNPCGum = npcHitGumSprite;
        agent = GetComponent<NavMeshAgent>(); // Gets the component
        audioDevice = GetComponent<AudioSource>(); // Gets the component
        if (audioDevice == null) // If the Audio Source is empty
        {
            audioDevice = gameObject.AddComponent<AudioSource>(); // Creates the audio source
            audioDevice.spatialBlend = 1f;
            audioDevice.maxDistance = 90f;
            audioDevice.minDistance = 20f;
            audioDevice.rolloffMode = AudioRolloffMode.Linear;
        }
        if (anim == null) // Is the animator empty?
        {
            anim = GetComponentInChildren<Animator>(); // Try to get the component on the children
        }
        if (playerTransform == null) // Is the player transform empty?
        {
            playerTransform = GameObject.Find("Player").transform;
        }
        Wander();
    }
 
    private void Update()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
    }
 
    private void FixedUpdate()
    {
        agent.isStopped = chewing;
        if (!chewing)
        {
            if (agent.velocity.magnitude <= 0.4f)
            {
                Wander();
            }
            if (agent.velocity.magnitude <= 1f)
            {
                anim.SetBool("Running", false);
            }
            else
            {
                anim.SetBool("Running", true);
            }
        }
        Vector3 direction = playerTransform.position - base.transform.position;
        RaycastHit raycastHit;
        SpottedPlayer = Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && raycastHit.transform.tag == "Player";
        if (SpottedPlayer && cooldown <= 0f)
        {
            cooldown = 30f;
            chewing = true;
            StartCoroutine(Spit());
            LastAngularSpeed = agent.angularSpeed;
            agent.angularSpeed = 0f;
        }
    }
 
    private void LateUpdate()
    {
        if (SpottedPlayer)
        {
            transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        }
    }
 
    private void Wander()
    {
        AILocationSelectorScript wanderer = FindObjectOfType<AILocationSelectorScript>();
        wanderer.GetNewTargetHallway();
        agent.SetDestination(wanderer.transform.position);
        if (Random.Range(0f, 99f) >= 98f & !audioDevice.isPlaying)
        {
            audioDevice.PlayOneShot(aud_skipping[Random.Range(0, aud_skipping.Length)]);
        }
    }
 
    private IEnumerator Spit()
    {
        audioDevice.Stop();
        audioDevice.PlayOneShot(aud_chewing[Random.Range(0, aud_chewing.Length)]);
        anim.SetBool("Running", false);
        anim.SetTrigger("Chewing");
        yield return new WaitForSeconds(5f);
        Vector3 pos = transform.position;
        pos.y = gumPrefab.transform.position.y;
        gum = Object.Instantiate(gumPrefab, pos, transform.rotation);
        if (!gum.GetComponent<BsodaSparyScript>()) // If you already added the script, feel free to remove this whole line to improve performance.
        {
            gum.AddComponent<BsodaSparyScript>();
            gum.GetComponent<BsodaSparyScript>().speed = 20f;
        }
        gum.name = "Gum";
        audioDevice.PlayOneShot(aud_spit);
        anim.SetTrigger("Spit");
        chewing = false;
        agent.angularSpeed = LastAngularSpeed;
        yield return new WaitForSecondsRealtime(1f);
        audioDevice.PlayOneShot(aud_spitSounds[Random.Range(0, aud_spitSounds.Length)]);
        yield break;
    }
 
    public void SorryPlayer()
    {
        audioDevice.PlayOneShot(aud_playerHit[Random.Range(0, aud_playerHit.Length)]);
    }
 
    public void SorryNPC()
    {
        audioDevice.PlayOneShot(aud_npcHit[Random.Range(0, aud_npcHit.Length)]);
    }
}