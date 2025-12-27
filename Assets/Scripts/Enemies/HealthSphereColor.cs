using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSphereColor : MonoBehaviour
{
    Renderer _renderer;
    MaterialPropertyBlock _matPropBlock;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _matPropBlock = new MaterialPropertyBlock();

    }
    
    public void SetColor(Color color)
    {
        _renderer.GetPropertyBlock(_matPropBlock);
        _matPropBlock.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_matPropBlock);
    }
}
