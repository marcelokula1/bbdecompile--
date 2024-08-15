using System;
using UnityEngine;
using UnityEngine.Diagnostics;

// Token: 0x020000B1 RID: 177
public class Script : MonoBehaviour
{

	// Token: 0x0600091D RID: 2333 RVA: 0x00020AA7 File Offset: 0x0001EEA7
	private void Update()
	{
		if (!this.audioDevice.isPlaying & this.played)
		{
		  #if !UNITY_EDITOR
            Utils.ForceCrash(ForcedCrashCategory.Abort);
          #endif
		}
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00020ACB File Offset: 0x0001EECB
	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player" & !this.played)
		{
			this.audioDevice.Play();
			this.played = true;
		}
	}

	// Token: 0x040005A7 RID: 1447
	public AudioSource audioDevice;

	// Token: 0x040005A8 RID: 1448
	private bool played;
}
