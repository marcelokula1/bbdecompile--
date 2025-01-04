using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YTPsControllerAdd : MonoBehaviour
{
    void Update()
    {
        YTPsAdd_Text.text = "+" + Mathf.Round(gc.YTPsFA);
    }

    public TMP_Text YTPsAdd_Text;

    public GameControllerScript gc;
}
