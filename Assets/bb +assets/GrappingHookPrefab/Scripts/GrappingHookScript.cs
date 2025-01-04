using UnityEngine;

public class GrappingHookScript : MonoBehaviour
{
    public PlayerScript ps;

    public CharacterController cc;

    public LineRenderer lineRenderer;

    public Rigidbody rb;

    Vector3[] positions = new Vector3[2];

    public float speed = 100f;

    float stopDistance = 5f;

    bool locked, broken;

    public float time, brokenTime;

    public AudioSource motorAudio;

    public AudioSource startAudio;

    public AudioClip start, clang, breakSound;

    public float MoveSpeed;

    public GameObject grap;

    public float movementLatency, deltaTime;

    Vector3 playerVelocity;

    private void Start()
    {
        startAudio.PlayOneShot(start);
        playerVelocity = (base.transform.position - ps.transform.position).normalized * MoveSpeed * movementLatency;
        cc.Move(playerVelocity * Time.deltaTime);
    }

    private void Update()
    {
        motorAudio.pitch += Time.deltaTime * Time.timeScale * 0.1f;
        if (!locked & !broken)
        {
            rb.velocity = base.transform.forward * speed * Time.timeScale;
        }
        else if (locked & !broken)
        {
            ps.grapping = true;
            time += Time.deltaTime * Time.timeScale;
            if (time > 10f)
            {
                motorAudio.Stop();
                startAudio.PlayOneShot(breakSound);
                broken = true;
                lineRenderer.gameObject.SetActive(false);
                ps.grapping = false;
            }
            if ((base.transform.position - ps.transform.position).magnitude <= stopDistance)
            {
                Destroy(gameObject);
                ps.grapping = false;
            }
            MoveSpeed += deltaTime * Time.deltaTime;
            if (MoveSpeed >= 100)
            {
                MoveSpeed = 100f;
            }
            playerVelocity = (base.transform.position - ps.transform.position).normalized * MoveSpeed * movementLatency;
            cc.Move(playerVelocity * Time.timeScale);
        }
        else
        {
            brokenTime += Time.deltaTime * Time.timeScale;
            if (brokenTime > 3f)
            {
                Destroy(gameObject);
                ps.grapping = false;
            }
        }

        positions[0] = base.transform.position;
        positions[1] = ps.transform.position - Vector3.up * 1f;
        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 0 || other.gameObject.layer == 8) & !locked & other.gameObject.tag != "Player" & other.gameObject.tag != "NPC" & !other.isTrigger)
        {
            time = 0f;
            startAudio.PlayOneShot(clang);
            locked = true;
            grap.SetActive(true);
            rb.velocity = Vector3.zero;
            motorAudio.Play();
        }
    }
}