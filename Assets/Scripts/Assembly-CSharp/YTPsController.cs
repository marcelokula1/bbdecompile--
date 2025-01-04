using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YTPsController : MonoBehaviour
{
    void Update()
    {
        if (this.gc.YTPsAnimAdd.GetCurrentAnimatorStateInfo(0).IsName("YTPsAddEnd"))
        {
            YTPs_Text.text = Mathf.Round(gc.YTPs) + "";
        }
    }

    public TMP_Text YTPs_Text;

    public GameControllerScript gc;
}
