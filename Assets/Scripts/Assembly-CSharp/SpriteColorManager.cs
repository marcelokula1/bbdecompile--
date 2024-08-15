using UnityEngine;

public class SpriteColorManager : MonoBehaviour
{
    public bool useOnRuntime = false;
    private SpriteRenderer spriteRenderer;
    private Material spriteMaterial;
    private Color originalSpriteColor;
    private LayerMask layerMask;

    private void Start()
    {
        this.layerMask = LayerMask.GetMask("Floor");
        this.InitializeSpriteRenderer();
        if (!this.useOnRuntime)
        {
            this.SetColor();
        }
    }

    private void InitializeSpriteRenderer()
    {
        this.spriteRenderer = base.GetComponent<SpriteRenderer>();
        if (this.spriteRenderer != null)
        {
            this.spriteMaterial = this.spriteRenderer.material;
            this.originalSpriteColor = this.spriteMaterial.GetColor("_Color");
        }
    }

    private void OnDestroy()
    {
        this.CleanupSpriteRenderer();
    }

    private void CleanupSpriteRenderer()
    {
        if (this.spriteMaterial != null)
        {
            this.spriteMaterial.SetColor("_Color", this.originalSpriteColor);
        }
    }

    private void LateUpdate()
    {
        if (this.spriteRenderer != null && Time.timeScale != 0f && this.spriteRenderer.isVisible && this.useOnRuntime)
        {
            this.SetColor();
        }
    }

    private void SetColor()
    {
        Color mixedColor = this.originalSpriteColor * this.GetTileColor();
        this.spriteMaterial.SetColor("_Color", mixedColor);
    }

    private Color GetTileColor()
    {
        RaycastHit hit;

        if (Physics.Raycast(base.transform.position, Vector3.down, out hit, float.PositiveInfinity, this.layerMask, QueryTriggerInteraction.Ignore))
        {
            TileColorManager curTile = hit.transform.parent.GetComponent<TileColorManager>();
            if (curTile != null)
            {
                return curTile.GetTileColor();
            }
        }
        return Color.white;
    }
}
