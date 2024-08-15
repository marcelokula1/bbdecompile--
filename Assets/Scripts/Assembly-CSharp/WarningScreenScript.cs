using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200009E RID: 158
public class WarningScreenScript : MonoBehaviour
{
	// Token: 0x060002E9 RID: 745 RVA: 0x00011476 File Offset: 0x0000F676
	private void Start()
	{
		this.images[0].SetActive(true);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00011490 File Offset: 0x0000F690
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			this.Advance();
		}
	}

	// Token: 0x060002EB RID: 747 RVA: 0x000114A0 File Offset: 0x0000F6A0
	private void Advance()
	{
		this.current++;
            if (this.current < this.images.Length)
            {
                this.images[this.current - 1].SetActive(false);
                this.images[this.current].SetActive(true);
				return;
            }
			SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x04000443 RID: 1091
	public GameObject[] images;

	// Token: 0x04000444 RID: 1092
	public string scene;

	// Token: 0x04000445 RID: 1093
	private int current;

}