using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShakeWall : MonoBehaviour
{
    private Stopwatch stopwatch;
    void Start()
    {
        stopwatch = new Stopwatch();
        StartCoroutine(NullBossShakeSchool());
    }

    IEnumerator NullBossShakeSchool()
    {
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>(FindObjectsOfType<MeshRenderer>());
        List<MeshRenderer> newRenderers = new List<MeshRenderer>(meshRenderers.ToArray());
        List<MeshFilter> filters = new List<MeshFilter>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            MeshFilter filter = renderer.GetComponent<MeshFilter>();
            if (filter != null)
            {
                filters.Add(filter);
            }
            else
            {
                newRenderers.Remove(renderer);
            }
        }

        meshRenderers = new List<MeshRenderer>(newRenderers.ToArray());

        while (true)
        {
            yield return new WaitForSeconds(1f / 2f / 1f);

            stopwatch.Start();

            for (int i = 0; i < meshRenderers.Count - 1; i++)
            {
                //The real MVP of this optimization!
                if (meshRenderers[i].isVisible) StartCoroutine(SchoolShaking(filters[i].mesh, 4f, 0.3f));
            }

            stopwatch.Stop();

            UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
        }
    }

    public IEnumerator SchoolShaking(Mesh mesh, float shakeAngle, float duration)
    {
        Vector3[] originalVertices = mesh.vertices;
        Vector3[] distortedVertices = new Vector3[originalVertices.Length];

        Quaternion originalRotation = Quaternion.identity;
        Quaternion targetRotation = Quaternion.Euler(
            UnityEngine.Random.Range(-shakeAngle, shakeAngle),
            UnityEngine.Random.Range(-shakeAngle, shakeAngle),
            UnityEngine.Random.Range(-shakeAngle, shakeAngle)
        );

        // Apply the rotation distortion to each vertex
        for (int i = 0; i < originalVertices.Length; i++)
        {
            distortedVertices[i] = targetRotation * originalVertices[i];
        }

        mesh.vertices = distortedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        float elapsedTime = 0f;

        // Gradually restore the vertices to the original positions
        while (elapsedTime <= duration)
        {
            for (int i = 0; i < originalVertices.Length; i++)
            {
                distortedVertices[i] = Vector3.Slerp(distortedVertices[i], originalVertices[i], elapsedTime / duration);
            }

            mesh.vertices = distortedVertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final vertices are exactly the original vertices
        mesh.vertices = originalVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}