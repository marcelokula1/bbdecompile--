using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftersAttackerScript : MonoBehaviour
{
    public void Attack()
    {
        base.StartCoroutine(this.AttackPlayer());
    }
	public IEnumerator AttackPlayer()
	{
		float speed = 350f;
		float acceleration = 25f;
		float spinDistance = 10f;
		Vector3 currentAngle = this.playerTransform.forward;
		float time = 0f;
        float echoDistance = 0f;
        Transform[] array = this.echos;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(true);
		}
		while (time < 15f)
		{
			currentAngle = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up) * currentAngle;
			base.transform.position = new Vector3(this.playerTransform.position.x, base.transform.position.y, this.playerTransform.position.z) + currentAngle * spinDistance;
            for (int j = 0; j < this.echos.Length; j++)
			{
				Vector3 echoAngle = Quaternion.AngleAxis(echoDistance * (float)j * -1f * Time.deltaTime, Vector3.up) * currentAngle;
				this.echos[j].transform.position = new Vector3(this.playerTransform.position.x, base.transform.position.y, this.playerTransform.position.z) + echoAngle * (spinDistance + (float)j + 1f);
			}
			speed += acceleration * Time.deltaTime;
            echoDistance += 300f * Time.deltaTime;
			time += Time.deltaTime;
			yield return null;
		}
		this.crafters.SetActive(true);
        this.craftersScript.GiveConsequence();
		Object.Destroy(base.gameObject);
		yield break;
	}
    public Transform playerTransform;
    public GameObject crafters;
    public CraftersScript craftersScript;
    public Transform[] echos;
}