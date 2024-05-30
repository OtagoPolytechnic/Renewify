using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricArc : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int positionCount = 10;
    public float arcHeight = 0.5f;
    public float jitterAmount = 0.2f;
    public AnimationCurve widthCurve;  // Curve to vary width along the line
    public float length = 5.0f;  // Length of the arc

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.positionCount = positionCount;  // Set the number of points
        lineRenderer.widthCurve = widthCurve;  // Set the width curve

        UpdateArc();  // Initialize the arc
        InvokeRepeating("UpdateArc", 0, 0.1f);  // Update the arc every 0.1 seconds
    
    }

    void UpdateArc()
    {
        for (int i = 0; i < positionCount; i++)
        {
            float t = i / (float)(positionCount - 1);
            float x = t * length;
            float y = Mathf.Sin(t * Mathf.PI) * arcHeight;
            float xJitter = Random.Range(-jitterAmount, jitterAmount);
            float yJitter = Random.Range(-jitterAmount, jitterAmount);
            lineRenderer.SetPosition(i, new Vector3(x + xJitter, y + yJitter, 0));
        }
    }
}
