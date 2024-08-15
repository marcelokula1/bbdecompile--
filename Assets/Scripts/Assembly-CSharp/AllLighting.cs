using UnityEngine;
using System.Collections.Generic;

public class AllLighting : MonoBehaviour
{
    private void Awake()
    {
        AllLighting.Instance = this;
        this.AllLightings = new List<Lighting>(FindObjectsOfType<Lighting>());
    }

    private void OnDestroy()
    {
        this.AllLightings.Clear();
    }

    public static AllLighting Instance;

    private List<Lighting> AllLightings = new List<Lighting>();

    public List<Lighting> Lights
    {
        get
		{
            return AllLightings;
		}
    }
}
