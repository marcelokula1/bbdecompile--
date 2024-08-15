using System.Collections;
using UnityEngine;

public class EventsController : MonoBehaviour
{
    public FloodEventMgr mgr; // Reference to the event manager

    public GameObject Monitor; 
    public AudioSource StartAnimSource; 
    public AudioSource EventAnnounceSource; 
    public AudioClip startAnim; 
    public AudioClip EventAnnounce; 
    public float slideDuration = 1.0f; 

    private Vector3 originalPosition; // Store the original position of the target GameObject

    private void Start()
    {
        if (Monitor != null)
        {
            originalPosition = Monitor.transform.position;
        }


        StartCoroutine(EventSequence());
    }

    private IEnumerator EventSequence()
    {
        while (true)
        {
            if (Monitor != null)
            {
                if (StartAnimSource != null && startAnim != null)
                {
                    StartAnimSource.clip = startAnim;
                    StartAnimSource.Play();
                }

               
                yield return StartCoroutine(SlideByOffset(new Vector3(0f, -200f, 0f), slideDuration));
            }


            if (EventAnnounceSource != null && EventAnnounce != null)
            {
                EventAnnounceSource.clip = EventAnnounce;
                EventAnnounceSource.Play();
            }


            mgr.StartEvent();

            yield return new WaitForSeconds(8f);

            yield return StartCoroutine(SlideByOffset(new Vector3(0f, 200f, 0f), slideDuration));


            float waitTime = Random.Range(30f, 60f);
            yield return new WaitForSeconds(waitTime);


            mgr.StopEvent();


            yield return new WaitForSeconds(60f);
        }
    }

    private IEnumerator SlideByOffset(Vector3 offset, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = Monitor.transform.position;
        Vector3 targetPosition = startingPosition + offset;

        while (elapsedTime < duration)
        {
            Monitor.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        Monitor.transform.position = targetPosition;
    }
}
