using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000D5 RID: 213
public class YouWonScript : MonoBehaviour
{

	// Token: 0x060009E9 RID: 2537 RVA: 0x00026797 File Offset: 0x00024B97
	private void Update()
    {
        if (!updateExecuted)
        {
            delay -= Time.deltaTime;

            if (delay <= 0f)
            {
                SceneManager.LoadScene("MainMenu");
                updateExecuted = true;
            }
        }
    }

	// Token: 0x0400071A RID: 1818
	private float delay = 10f;

	// Token: 0x0400071B RID: 1819
	private bool updateExecuted = false;
}
