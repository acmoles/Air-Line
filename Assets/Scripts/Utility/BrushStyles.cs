using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BrushUpDownState
{
    Up,
    Down
}

public enum BrushSize
{
    Small,
    Medium,
    Large
}

public enum BrushColor
{
    Orange,
    Purple,
    Blue,
    Green,
    Custom
}

[CreateAssetMenu(fileName = "BrushStyles", menuName = "Utils/BrushStyles")]
public class BrushStyles : ScriptableObject
{
    public bool networkBrushStyles = true;

    [SerializeField]
    private StringEvent stylesChangedEvent = null;

    [Header("Global Brush Style")]

    [SerializeField]
    private BrushColor color = BrushColor.Blue;
    private Color customColor = Color.white;
    private Color secondaryColor = Color.white;

    [SerializeField]
    private BrushSize brushSize = BrushSize.Medium;

    [SerializeField]
    private Material material;

    private BrushUpDownState brushToggle = BrushUpDownState.Down;

    public BrushColor BrushColor
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            customColor = GetPrimary(value);
            secondaryColor = GetSecondary(value);
            OnStylesChanged();
        }
    }

    public Color CustomColor
    {
        get
        {
            return customColor;
        }
        set
        {
            color = BrushColor.Custom;
            customColor = value;
            secondaryColor = value;
            OnStylesChanged();
        }
    }
    public Color SecondaryColor
    {
        get
        {
            return secondaryColor;
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
            OnStylesChanged();
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
            OnStylesChanged();
        }
    }

    public BrushUpDownState BrushToggle
    {
        get
        {
            return brushToggle;
        }
        set
        {
            brushToggle = value;
            OnStylesChanged();
        }
    }

    private Dictionary<BrushColor, Color> colorTranslatorPrimary;
    private Dictionary<BrushColor, Color> colorTranslatorSecondary;

    private void OnEnable()
    {
        colorTranslatorPrimary = new Dictionary<BrushColor, Color>()
        {
            {BrushColor.Purple, purple1},
            {BrushColor.Blue, blue1},
            {BrushColor.Orange, orange1},
            {BrushColor.Green, green1}
        };
        colorTranslatorSecondary = new Dictionary<BrushColor, Color>()
        {
            {BrushColor.Purple, purple2},
            {BrushColor.Blue, blue2},
            {BrushColor.Orange, orange2},
            {BrushColor.Green, green2}
        };
        BrushColor = color;
        OnStylesChanged();
    }

    private void OnStylesChanged()
    {
        stylesChangedEvent.Trigger("Update");
        if(networkBrushStyles) SerializeBrushStyles();
    }

    // Getting actual colors for BrushColor
    public Color GetPrimary(BrushColor color)
    {
        if (colorTranslatorPrimary == null) return Color.black;
        return colorTranslatorPrimary[color];
    }
    public Color GetSecondary(BrushColor color)
    {
        if (colorTranslatorSecondary == null) return Color.black;
        return colorTranslatorSecondary[color];
    }


    [Space(10)]
    [Header("Line Settings")]
    public float small = 0.01f;
    public float medium = 0.02f;
    public float large = 0.04f;

    [SerializeField]
    private float wobbleModifierValue = 0.16f;

    public float wobbleModifier
    {
        get
        {
            return wobbleModifierValue / ((int)brushSize + 1);
        }
    }

    public int drawResolution = 8;
    public float minSegmentLength = 0.1f;
    public int maxPoints = 40;

    [Space(10)]

    public int startTaper = 3;
    public int endTaper = 3;
    public int amountToAverage = 3;
    public int amountToAverageColor = 3;

    [Space(10)]
    [Header("Color Settings")]
    public Color purple1 = Color.blue;
    public Color purple2 = Color.blue;
    [Space(10)]
    public Color blue1 = Color.blue;
    public Color blue2 = Color.blue;
    [Space(10)]
    public Color orange1 = Color.blue;
    public Color orange2 = Color.blue;
    [Space(10)]
    public Color green1 = Color.blue;
    public Color green2 = Color.blue;

    void OnValidate()
    {
        small = Mathf.Max(0, small);
        medium = Mathf.Max(0, medium);
        large = Mathf.Max(0, large);

        drawResolution = Mathf.Clamp(drawResolution, 3, 24);
        minSegmentLength = Mathf.Max(0, minSegmentLength);

        BrushColor = color;
    }

    // String access to float value from brushsize enum
    // Value array
    private float[] val = new float[3];

    // Indexer array 
    private string[] indices = {
        "Small", "Medium", "Large"
    };

    public float this[string index]
    {
        get
        {
            val[0] = small; val[1] = medium; val[2] = large;
            return val[Array.IndexOf(indices, index)];
        }
    }

    // Networking

    public string SerializeBrushStyles()
    {
        // Pack Brushstyles for network transfer
        return "Brush styles serialized";
    }

    public void DeSerializeBrushStyles(string serializedBrushStyles)
    {
        // Unpack Brushstyles transfer
        // Apply Brushstyles values to self
        OnStylesChanged();
    }

}
