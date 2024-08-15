using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseAppearingScript : MonoBehaviour
{
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "Door" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 15f))
        {
            this.MouseCursor.SetActive(true);
        }
        else if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "Item" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            this.MouseCursor.SetActive(true);
        }
        else if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "Notebook" & Vector3.Distance(this.playerTransform.position, raycastHit.transform.position) <= 10f))
        {
            this.MouseCursor.SetActive(true);
        }
        else
        {
            this.MouseCursor.SetActive(false);
        }
    }
   
    public GameObject MouseCursor;

    public Transform playerTransform;
}