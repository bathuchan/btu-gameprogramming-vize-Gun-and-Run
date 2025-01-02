using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuEffect : MonoBehaviour
{
    public float yTilingSpeed = 0.1f; // Speed at which the Y tiling changes
    private Material _material;
    private float _currentYTile;

    private void Start()
    {
        
        // Get the material from the renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _material = renderer.material;
            _currentYTile = _material.mainTextureScale.y;
        }
        else
        {
            Debug.LogError("No Renderer found on the GameObject.");
        }
    }

    private void Update()
    {
        if (_material == null) return;

        // Change the Y tiling value at a constant rate
        _currentYTile += yTilingSpeed;

        // Apply the updated tiling value
        Vector2 newScale = _material.mainTextureScale;
        newScale.y = _currentYTile;
        _material.mainTextureOffset = newScale;
    }
}