using System;
using UnityEngine;
using System.Collections;

// Token: 0x020000B2 RID: 178
public class AudioQueueScript : MonoBehaviour
{
	// Token: 0x06000920 RID: 2336 RVA: 0x00020B13 File Offset: 0x0001EF13
	private void Start()
	{
		this.audioDevice = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x00020B21 File Offset: 0x0001EF21
	private void Update()
	{
		if (this.audioInQueue > 0 & !this.audioDevice.isPlaying)
		{
			this.PlayQueue();
		}
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x00020B45 File Offset: 0x0001EF45
	public void QueueAudio(AudioClip sound)
	{
		this.audioQueue[this.audioInQueue] = sound;
		this.audioInQueue++;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00020B63 File Offset: 0x0001EF63
	private void PlayQueue()
	{
		this.audioDevice.PlayOneShot(this.audioQueue[0]);
		this.UnqueueAudio();
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00020B80 File Offset: 0x0001EF80
	private void UnqueueAudio()
	{
		for (int i = 1; i < this.audioInQueue; i++)
		{
			this.audioQueue[i - 1] = this.audioQueue[i];
		}
		this.audioInQueue--;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00020BC4 File Offset: 0x0001EFC4
	public void ClearAudioQueue()
	{
		this.audioInQueue = 0;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000028BB File Offset: 0x00000ABB
	public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
	{
		float startVolume = audioSource.volume;
		while (audioSource.volume > 0f)
		{
			audioSource.volume -= startVolume * Time.unscaledDeltaTime / FadeTime;
			yield return null;
		}
		audioSource.Stop();
		audioSource.volume = startVolume;
		yield break;
	}

	// Token: 0x040005A9 RID: 1449
	private AudioSource audioDevice;

	// Token: 0x040005AA RID: 1450
	private int audioInQueue;

	// Token: 0x040005AB RID: 1451
	private AudioClip[] audioQueue = new AudioClip[100];
}
