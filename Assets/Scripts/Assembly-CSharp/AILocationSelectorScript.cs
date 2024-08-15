using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class AILocationSelectorScript : MonoBehaviour
{
	// Token: 0x0600099F RID: 2463 RVA: 0x00024478 File Offset: 0x00022878
	public void GetNewTarget()
	{
		this.id = Mathf.RoundToInt(UnityEngine.Random.Range(0f, (float)this.newLocation.Length)); //Get a random number between 0 and how long it's length is
		base.transform.position = this.newLocation[this.id].position; //Set it's location to a position in a list of positions using the ID variable that just got set.
		this.ambience.PlayAudio(); //Play an ambience audio
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x000244C8 File Offset: 0x000228C8
	public void GetNewTargetHallway()
	{
		this.id = Mathf.RoundToInt(UnityEngine.Random.Range(0f, (float)this.hallLocation.Length)); //Get a random number between 0 and how long it's length is
		base.transform.position = this.hallLocation[this.id].position; //Set it's location to a position in a list of positions using the ID variable that just got set.
		this.ambience.PlayAudio(); //Play an ambience audio
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00024517 File Offset: 0x00022917
	public void QuarterExclusive()
	{
		this.id = Mathf.RoundToInt(UnityEngine.Random.Range(0f, (float)this.quarterLocation.Length)); //Get a random number between 0 and how long it's length is
		base.transform.position = this.quarterLocation[this.id].position; //Set it's location to a position in a list of positions using the ID variable that just got set.
	}

	// Token: 0x0400067C RID: 1660
	public Transform[] newLocation;

	// Token: 0x0400067D RID: 1661
	public Transform[] hallLocation;

	// Token: 0x0400067E RID: 1662
	public Transform[] quarterLocation;

	// Token: 0x0400067F RID: 1663
	public AmbienceScript ambience;

	// Token: 0x0400067G RID: 1664
	private int id;
}
