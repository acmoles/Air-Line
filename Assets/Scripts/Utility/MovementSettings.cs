using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "MovementSettings", menuName = "Utils/MovementSettings")]
public class MovementSettings : ScriptableObject
{
    [Header("Global Brush Style")]

    [SerializeField]
    private Color color = Color.white;

    [SerializeField]
    private BrushSize brushSize = BrushSize.Medium;

    [SerializeField]
    private Material material;

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            // Trigger update event
        }
    }

    public BrushSize BrushSize
    {
        get
        {
            return brushSize;
        }
        set
        {
            brushSize = value;
            // Trigger update event
        }
    }

    public Material Material
    {
        get
        {
            return material;
        }
        set
        {
            material = value;
            // Trigger update event
        }
    }
    

    [Space(10)]
    [Header("Line Settings")]
    public float small = 0.01f;
    public float medium = 0.02f;
    public float large = 0.04f;
    public float wobbleModifier = 0.16f;
    public int drawResolution = 8;
    public float minSegmentLength = 0.1f;

    [Space(10)]

    public int startTaper = 3;
    public int endTaper = 3;
    public int amountToAverage = 3;

    void OnValidate()
    {
        small = Mathf.Max(0, small);
        medium = Mathf.Max(0, medium);
        large = Mathf.Max(0, large);

        drawResolution = Mathf.Clamp(drawResolution, 3, 24);
        minSegmentLength = Mathf.Max(0, minSegmentLength);
    }

    // String access to float value from brushsize enum
    // Value array
    private float[] val = new float[3];

    // Indexer array 
    private string[] indices = { "Small", "Medium", "Large" };

    public float this[string index]
    {
        get
        {
            val[0] = small; val[1] = medium; val[2] = large;
            return val[Array.IndexOf(indices, index)];
        }
    }
}
