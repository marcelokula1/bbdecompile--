using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found in the scene.");
            return;
        }
        StartCoroutine(CameraFOVShake());
    }

    IEnumerator CameraFOVShake()
    {
        float defaultFOV = 60f;
        float targetFOV = 90f;
        float duration = 0.1f;

        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Wait 0.5 seconds before activating the shake

            // Gradually increase the FOV to the target value
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                mainCamera.fieldOfView = Mathf.Lerp(defaultFOV, targetFOV, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure FOV reaches the target value
            mainCamera.fieldOfView = targetFOV;

            // Gradually return the FOV to the default value
            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                mainCamera.fieldOfView = Mathf.Lerp(targetFOV, defaultFOV, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure FOV returns to the default value
            mainCamera.fieldOfView = defaultFOV;
        }
    }
}