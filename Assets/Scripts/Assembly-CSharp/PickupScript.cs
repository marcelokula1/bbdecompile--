using UnityEngine;
using System.Collections.Generic;

// Token: 0x02000043 RID: 67
public class PickupScript : MonoBehaviour
{
	// Token: 0x060009CA RID: 2506 RVA: 0x00025604 File Offset: 0x00023A04
    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 10f) && raycastHit.transform.gameObject == transform.gameObject)
        {
            if (gc.item[0] == 0 || gc.item[1] == 0 || gc.item[2] == 0)
            {
                raycastHit.transform.gameObject.SetActive(false);
                gc.CollectItem(ID);
                return;
            }

            int orgID = ID;
            ID = gc.item[gc.itemSelected];

            if (!cachedSprites.ContainsKey(ID))
            {
                Texture itemTexture = gc.BigItemTextures[ID];
                Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100);
                cachedSprites.Add(ID, itemSprite);
            }

            GetComponentInChildren<SpriteRenderer>().sprite = cachedSprites[ID];
            gameObject.name = itemNames[ID];
            
            if (gc.item[0] != 0 && gc.item[1] != 0 && gc.item[2] != 0)
            {
                raycastHit.transform.gameObject.SetActive(true);
            }

            gc.CollectItem(orgID);
        }
    }

    // Token: 0x040006DB RID: 1755
    [SerializeField] private GameControllerScript gc;

    // Token: 0x040006DC RID: 1756
    [SerializeField] private int ID;

    // Token: 0x040006DD RID: 1757
    public List<string> itemNames;

    // Token: 0x040006DE RID: 1758
    private static Dictionary<int, Sprite> cachedSprites = new Dictionary<int, Sprite>();
    
}
