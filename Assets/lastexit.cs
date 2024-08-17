using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lastexit : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
      if (!done & this.gc.exitsReached == 3 & this.gc.mode == "NULL")
          {
		  this.gc.entrance_2.Raise();
            done = true;
          }
    }

   public GameControllerScript gc;
   public bool done;
}
