using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    Dictionary<Renderer, Material[]> _glowMaterialDictionary = new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> _originalMaterialDictionary = new Dictionary<Renderer, Material[]>();
    Dictionary<Color, Material> _cachedGlowMaterials = new Dictionary<Color, Material>();

    [SerializeField] private Material _glowMaterial;

    private bool _isGlowing = false;

    private void Awake()
    {
        PrepareMaterialDictionaries();
    }

    private void PrepareMaterialDictionaries()
    {
        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            Material[] originalMaterials = renderer.materials;
            _originalMaterialDictionary.Add(renderer, originalMaterials);
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                Material mat = null;
                if (_cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false)
                {
                    mat = new Material(_glowMaterial);
                    mat.color = originalMaterials[i].color;
                    _cachedGlowMaterials[mat.color] = mat;
                }
                newMaterials[i] = mat;
            }
            _glowMaterialDictionary.Add(renderer, newMaterials);
        }
    }

    private void ToggleGlow()
    {
        if (!_isGlowing)
        {
            foreach(Renderer renderer in _originalMaterialDictionary.Keys)
            {
                renderer.materials = _glowMaterialDictionary[renderer];
            }
        }
        else
        {
            foreach(Renderer renderer in _originalMaterialDictionary.Keys)
            {
                renderer.materials = _originalMaterialDictionary[renderer];
            }
        }
        _isGlowing = !_isGlowing;
    }

    public void ToggleGlow(bool state)
    {
        if (_isGlowing == state)
        {
            return;
        }
        _isGlowing = !state;
        ToggleGlow();
    }
}
