using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhirlPool : MonoBehaviour
{
	public FloodEventMgr mgr;
	public float RotateSpeed = 10;
	public float veclocity = 10;
	public float MaxDistance = 10;
	public float TeleportHeight = 4;
	public float TeleportSpeed = 1;
	public List<GameObject> Output = new List<GameObject>();
	public static List<GameObject> Teleporting = new List<GameObject>();
	private void Update()
	{
		base.transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
		for (int i = 0; i < GameObject.FindGameObjectsWithTag("NPC").Length; i++)
		{
			Debug.DrawRay(base.transform.position, GameObject.FindGameObjectsWithTag("NPC")[i].transform.position - base.transform.position, Color.red); RaycastHit hit;
			if (Physics.Raycast(base.transform.position, GameObject.FindGameObjectsWithTag("NPC")[i].transform.position - base.transform.position, out hit, MaxDistance))
			{
				if (hit.transform.CompareTag("NPC"))
				{
					hit.transform.GetComponent<NavMeshAgent>().Move((base.transform.position - hit.transform.position).normalized * veclocity * Time.deltaTime);
				}
			}
		}
		for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
		{
			Debug.DrawRay(base.transform.position, GameObject.FindGameObjectsWithTag("Player")[i].transform.position - base.transform.position, Color.red); RaycastHit hit;
			if (Physics.Raycast(base.transform.position, GameObject.FindGameObjectsWithTag("Player")[i].transform.position - base.transform.position, out hit, MaxDistance))
			{
				if (hit.transform.CompareTag("Player"))
				{
					hit.transform.GetComponent<CharacterController>().Move((base.transform.position - hit.transform.position).normalized * veclocity * Time.deltaTime);
				}
			}
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!Output.Contains(other.transform.gameObject) && !Teleporting.Contains(other.gameObject))
		{
			if (other.transform.tag.Contains("NPC"))
			{
				base.StartCoroutine(Teleport(other.transform.GetComponent<NavMeshAgent>()));
			}
			if (other.transform.tag.Contains("Player"))
			{
				base.StartCoroutine(Teleport(other.transform.GetComponent<PlayerScript>()));
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		Output.Remove(other.gameObject);
		Teleporting.Remove(other.gameObject);
	}
	public IEnumerator Teleport(NavMeshAgent subject)
	{
		subject.enabled = false;
		Teleporting.Add(subject.gameObject);
		WhirlPool whirlPool = mgr.GetRandomWhirlPool(this);
		if (!whirlPool.Output.Contains(subject.gameObject))
		{
			whirlPool.Output.Add(subject.gameObject);
		}
		subject.transform.position = base.transform.position + Vector3.up * TeleportHeight;
		while (subject.transform.position.y > 0)
		{
			subject.transform.position -= Vector3.up * Time.deltaTime * TeleportSpeed;
			yield return null;
		}
		subject.transform.position = whirlPool.transform.position
		; subject.enabled = true;
		while (!subject.isOnNavMesh)
		{
			subject.Warp(subject.transform.position + Vector3.up * Time.deltaTime * TeleportSpeed);
			yield return null;
		}
	}
	public IEnumerator Teleport(PlayerScript subject)
	{
		subject.cc.enabled = false;
		Teleporting.Add(subject.gameObject);
		WhirlPool whirlPool = mgr.GetRandomWhirlPool(this);
		if (!whirlPool.Output.Contains(subject.gameObject))
		{
			whirlPool.Output.Add(subject.gameObject);
		}
		subject.transform.position = base.transform.position + Vector3.up * TeleportHeight;
		while (subject.transform.position.y > 0)
		{
			subject.height -= Time.deltaTime * TeleportSpeed;
			yield return null;
		}
		subject.transform.position = whirlPool.transform.position;
		while (subject.transform.position.y < TeleportHeight)
		{
			subject.height += Time.deltaTime * TeleportSpeed;
			yield return null;
		}
		subject.cc.enabled = true;
	}
}
