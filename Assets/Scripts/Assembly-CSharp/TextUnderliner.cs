using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
// Token: 0x020000D9 RID: 217
public class TextUnderliner : MonoBehaviour
{

    // Token: 0x060009F8 RID: 2552 RVA: 0x00026A87 File Offset: 0x00024E87
    public void Underline(bool enableUnderline)
    {
        if (enableUnderline)
        {
            if ((text.fontStyle & FontStyles.Underline) == 0)
                text.fontStyle |= FontStyles.Underline;
        }
        else
        {
            if ((text.fontStyle & FontStyles.Underline) != 0)
                text.fontStyle ^= FontStyles.Underline;
        }
    }

    // Token: 0x0400072A RID: 1834
    [SerializeField] private TMP_Text text;
    
}