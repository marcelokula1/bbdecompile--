using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLockerController : MonoBehaviour
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
                if (hit.collider.name == "BlueLocker" && Vector3.Distance(playerTransform.position, hit.transform.position) <= 10f)
                {
                    BlueLockerScript lockerScript = hit.collider.gameObject.GetComponent<BlueLockerScript>();
                    if (lockerScript != null & !lockerScript.hidden)
                    {
                        lockerScript.Hide();
                    }
                    else
                    {
                        lockerScript.UnHide();
                    }
                }
            }
        }
    }
}
