using UnityEngine;
using System.Collections.Generic;

public class Lighting : MonoBehaviour
{
    public Color shadowColor = new Color(0.47f, 0.47f, 0.47f, 1f);
    public float minPower = 3f;
    public float maxPower = 5f;
    public float intensity = 1.0f;

    public List<GameObject> excludedObjects = new List<GameObject>();
    public List<GameObject> excludedParentObjects = new List<GameObject>();

    public bool IsExcluded(GameObject obj)
    {      
        if (this.excludedObjects.Contains(obj))
            return true;

        Transform parent = obj.transform;
        while (parent != null)
        {
            if (this.excludedParentObjects.Contains(parent.gameObject))
                return true;
            parent = parent.parent;
        }

        return false;
    }

    public bool isChildExcluded(GameObject obj)
    {
        if (this.excludedObjects.Contains(obj))
        {
            return true;
        }
            
        return false;
    }

    public Color GetColorAtPosition(Vector3 position)
    {
        float distance = Vector3.Distance(base.transform.position, position);
        if (distance <= this.minPower)
        {
            Color initialColor = this.shadowColor * this.intensity;
            return initialColor;
        }
        else if (distance <= maxPower)
        {
            float lerpValue = 1f - ((distance - this.minPower) / (this.maxPower - this.minPower));
            return Color.Lerp(Color.white, this.shadowColor, lerpValue) * this.intensity;
        }
        else
        {
            return Color.white;
        }
    }

    public float CalculateInfluence(Vector3 position)
    {
        float distance = Vector3.Distance(base.transform.position, position);
        if (distance <= this.minPower)
        {
            return 1f;
        }
        else if (distance <= this.maxPower)
        {
            return 1f - ((distance - this.minPower) / (this.maxPower - this.minPower));
        }
        else
        {
            return 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
            this.DrawGizmos();
        #endif
    }

    private void DrawGizmos()
    {
        Gizmos.color = shadowColor;
        Gizmos.DrawWireSphere(base.transform.position, this.minPower);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(base.transform.position, this.maxPower);
    }
}
