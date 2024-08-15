using System;
using UnityEngine;

[ExecuteInEditMode]
public class PickupAnimationScript : MonoBehaviour
{
	private const float UpdateFrequency =
		400;
	
	private float time;

	private float waitTime;
	
	[SerializeField]
	private float amplitude = 0.5f;
	
	[SerializeField]
	private float centerY = 1f;
	
	private new Transform transform;

	private void Start()
	{
		transform = base.transform;
	}

	private void OnWillRenderObject()
	{
		var cameraCurrent = Camera.current;
		
		time += Time.deltaTime;
		
		if (time < waitTime) return;
		time = 0;

		try
		{
			var distance = Vector3.Distance(transform.position, cameraCurrent.transform.position);
			if (distance > 125)
			{
				return;
			}
			
			waitTime = distance / UpdateFrequency;
		}
		catch
		{
		}

		transform.localPosition = Vector3.up *
			                          (Mathf.Sin(Time.time * 2) * amplitude + centerY);
	}
}