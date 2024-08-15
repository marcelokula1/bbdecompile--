using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainController : MonoBehaviour
{
    public Transform playerTransform;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.name == "WaterFountain" && Vector3.Distance(playerTransform.position, hit.transform.position) <= 10f)
                {
                    FountainScript waterScript = hit.collider.gameObject.GetComponent<FountainScript>();
                    if (waterScript != null)
                    {
                        waterScript.Use();
                    }
                }
            }
        }
    }
}
