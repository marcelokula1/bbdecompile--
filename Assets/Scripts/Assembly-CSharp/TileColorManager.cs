using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TileColorManager : MonoBehaviour
{
    private struct RendererMaterials
    {
        public Renderer renderer;
        public bool isExcluded;
        public Material[] originalMaterials;
        public Color[] originalColors;
        public MaterialPropertyBlock[] blocks;
    }

    private List<RendererMaterials> rendererMaterialsList = new List<RendererMaterials>();

    private void Start()
    {
        this.InitializeRenderers();

        if (AllLighting.Instance.Lights.Count > 0)
        {
            this.BlendLightingColors();
        }
    }

    private void OnDestroy()
    {
        this.CleanupRenderers();
    }

    private void InitializeRenderers()
    {
        Renderer[] renderers = base.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            RendererMaterials rendererMaterials;
            rendererMaterials.renderer = renderer;
            rendererMaterials.isExcluded = false;
            foreach (Lighting currentLighting in AllLighting.Instance.Lights)
            {
                if (currentLighting.isChildExcluded(renderer.gameObject))
                {
                    rendererMaterials.isExcluded = true;
                    break;
                }
            }
            rendererMaterials.originalMaterials = renderer.sharedMaterials;
            rendererMaterials.originalColors = new Color[rendererMaterials.originalMaterials.Length];
            rendererMaterials.blocks = new MaterialPropertyBlock[rendererMaterials.originalMaterials.Length];
            for (int i = 0; i < rendererMaterials.originalMaterials.Length; i++)
            {
                if (rendererMaterials.originalMaterials.Length > i && rendererMaterials.originalMaterials[i].HasProperty("_Color"))
                {
                    rendererMaterials.originalColors[i] = rendererMaterials.originalMaterials[i].color;
                }
                rendererMaterials.blocks[i] = new MaterialPropertyBlock();
                rendererMaterials.renderer.GetPropertyBlock(rendererMaterials.blocks[i], i);
            }
            this.rendererMaterialsList.Add(rendererMaterials);
        }
    }

    private void CleanupRenderers()
    {
        foreach (RendererMaterials rendererMaterials in this.rendererMaterialsList)
        {
            for (int i = 0; i < rendererMaterials.blocks.Length; i++)
            {
                rendererMaterials.renderer.SetPropertyBlock(null, i);
            }
        }
        this.rendererMaterialsList.Clear();
    }

    public Color GetTileColor()
    {
        Color col = this.CalculateFinalColor();
        return col;
    }

    private void BlendLightingColors()
    {
        Color finalColor = this.CalculateFinalColor();
        this.ApplyColor(finalColor);
    }

    private Color CalculateFinalColor()
    {
        Color finalColor = Color.clear;
        float totalInfluence = 0f;

        foreach (Lighting currentLighting in AllLighting.Instance.Lights)
        {
            if (!currentLighting.IsExcluded(base.gameObject))
            {
                float influence = currentLighting.CalculateInfluence(base.transform.position);
                if (influence > 0)
                {
                    Color lightColor = currentLighting.GetColorAtPosition(base.transform.position);
                    finalColor += lightColor * influence;
                    totalInfluence += influence;
                }
            }
        }

        return totalInfluence > 0 ? finalColor / totalInfluence : Color.white;
    }

    private void ApplyColor(Color color)
    {
        foreach (RendererMaterials rendererMaterials in this.rendererMaterialsList)
        {
            ApplyColorToMaterials(color, rendererMaterials, rendererMaterials.isExcluded);
        }
    }

    private static void ApplyColorToMaterials(Color color, RendererMaterials rendererMaterials, bool isExcluded)
    {
        for (int i = 0; i < rendererMaterials.blocks.Length; i++)
        {
            if (isExcluded)
            {
                rendererMaterials.blocks[i].SetColor("_Color", rendererMaterials.originalColors[i]);
            }
            else if (rendererMaterials.renderer.sharedMaterials.Length > i && rendererMaterials.renderer.sharedMaterials[i].HasProperty("_Color"))
            {
                rendererMaterials.blocks[i].SetColor("_Color", rendererMaterials.originalColors[i] * color);
            }
            rendererMaterials.renderer.SetPropertyBlock(rendererMaterials.blocks[i], i);
        }
    }
}
