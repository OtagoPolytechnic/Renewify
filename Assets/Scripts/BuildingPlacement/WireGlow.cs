using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireGlow : MonoBehaviour
{
    public Renderer cylinderRenderer;
    public float minIntensity = 1f;
    public float maxIntensity = 5f;
    public Color[] glowColors;
    public float colorChangeInterval = 2f;

    private Material material;
    private float intensity;
    private int currentColorIndex = 0;

    void Start()
    {
        material = cylinderRenderer.material;
        intensity = minIntensity;
        SetGlowIntensity(intensity);
        InvokeRepeating("ChangeGlowColor", 0f, colorChangeInterval);
    }

    void Update()
    {
        // Example: Increase intensity over time
        intensity += Time.deltaTime;
        if (intensity > maxIntensity)
            intensity = minIntensity; // Reset to min if max is exceeded
        SetGlowIntensity(intensity);
    }

    void SetGlowIntensity(float value)
    {
        material.SetFloat("_EmissionIntensity", value);
    }

    void ChangeGlowColor()
    {
        currentColorIndex = (currentColorIndex + 1) % glowColors.Length;
        material.SetColor("_EmissionColor", glowColors[currentColorIndex]);
    }

    // Example method to externally change the glow color
    public void SetGlowColor(Color color)
    {
        material.SetColor("_EmissionColor", color);
    }
}
