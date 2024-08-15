using System;
using UnityEngine;

// This script is licensed under a Creative Commons Attribution 4.0 International License.
// (https://creativecommons.org/licenses/by/4.0/)
// This means you MUST give me (YuraSuper2048) credit if you are using this.
// If you redistribute this script (modified or not) you should save this message as is. 

[ExecuteInEditMode, RequireComponent(typeof(SpriteRenderer))]
public class Billboard : MonoBehaviour
{
	// Change this to control frequency of billboarding
	// Less = more optimized, but will increase billboarding delays
	// More = less optimized, will reduce billboarding delays
	// (This actually is distance to player where update will take 1 second)
	private const float UpdateFrequency =
		300;

	// Time before last update
	private float time;

	// Checkmark for objects that must not be optimized (eg: fast moving objects)
	[SerializeField]
	private bool doNotOptimize;
	
	// Checkmark that enables X billboarding
	[SerializeField]
	private bool billboardX;

	// Time to wait before billboarding
	private float waitTime;

	// Needed to reduce the built-in unity calls
	private new Transform transform;

	private void Start()
	{
		// Making sure the sprite is set up correctly
		GetComponent<SpriteRenderer>().flipX = false;
		
		// Getting transform
		transform = base.transform;
	}

	private void OnBecameVisible()
	{
		var cameraCurrent = Camera.current;
		
		if(billboardX)
			transform.rotation = Quaternion.Euler(cameraCurrent.transform.rotation.eulerAngles.x, 
				cameraCurrent.transform.rotation.eulerAngles.y, 0f);
		else
			transform.rotation = Quaternion.Euler(0f,
				cameraCurrent.transform.rotation.eulerAngles.y, 0f);
	}

	private void OnWillRenderObject()
	{
		// Getting current camera for further use
		var cameraCurrent = Camera.current;

		// Counting time
		time += Time.deltaTime;
		
		// Waiting some time based on distance
		if (time < waitTime && !doNotOptimize) return;
		time = 0;
		
		try
		{
			// Calcualting time to wait
			if(!doNotOptimize)
				waitTime = Vector3.Distance(transform.position, cameraCurrent.transform.position) / UpdateFrequency;

			// Billboarding
			if(billboardX)
				transform.rotation = Quaternion.Euler(Camera.current.transform.rotation.eulerAngles.x, 
					cameraCurrent.transform.rotation.eulerAngles.y, 0f);
			else
				transform.rotation = Quaternion.Euler(0f,
					cameraCurrent.transform.rotation.eulerAngles.y, 0f);
		}
		catch
		{
			// Ignore all errors
		}
	}
}
