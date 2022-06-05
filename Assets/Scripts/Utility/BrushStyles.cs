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

    [SerializeField]
    private bool logging = true;

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

    [SerializeField]
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
            OnStylesChanged(value.ToString());
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
            OnStylesChanged("CustomColor");
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
            OnStylesChanged(value.ToString());
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
            OnStylesChanged("Material");
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
            OnStylesChanged(value.ToString());
        }
    }

    private Dictionary<BrushColor, Color> colorTranslatorPrimary;
    private Dictionary<BrushColor, Color> colorTranslatorSecondary;

    private void OnEnable()
    {
        // Ensure color and secondaryColor are set correctly and trigger an update event
        BrushColor = color;
    }

    private void OnStylesChanged(string state)
    {
        if (logging) Debug.Log("Onstyles changed event triggered: " + state);
        stylesChangedEvent.Trigger(state);
    }

    // Getting actual colors for BrushColor
    public Color GetPrimary(BrushColor color)
    {
        if (colorTranslatorPrimary == null)
        {
            colorTranslatorPrimary = new Dictionary<BrushColor, Color>()
            {
                {BrushColor.Purple, purple1},
                {BrushColor.Blue, blue1},
                {BrushColor.Orange, orange1},
                {BrushColor.Green, green1},
                {BrushColor.Custom, customColor},
            };
        }
        return colorTranslatorPrimary[color];
    }
    public Color GetSecondary(BrushColor color)
    {
        if (colorTranslatorSecondary == null)
        {
            colorTranslatorSecondary = new Dictionary<BrushColor, Color>()
            {
                {BrushColor.Purple, purple2},
                {BrushColor.Blue, blue2},
                {BrushColor.Orange, orange2},
                {BrushColor.Green, green2},
                {BrushColor.Custom, customColor},
            };
        }
        return colorTranslatorSecondary[color];
    }

    [Space(10)]
    [Header("Placement Settings")]
    public float followMeScreenOffset = 0.1f;
    public float waypointScreenOffset = 0.1f;


    [Space(10)]
    [Header("Line Settings")]
    public float small = 0.005f;
    public float medium = 0.01f;
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

        // Ensure color and secondaryColor are set correctly and trigger an update event
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
    private BrushStylesMessage cachedOutgoingMessage = new BrushStylesMessage();
    public string SerializeBrushStyles()
    {
        // Pack Brushstyles for network transfer
        cachedOutgoingMessage.brushColor = this.BrushColor;
        cachedOutgoingMessage.customColor = this.CustomColor;
        cachedOutgoingMessage.brushSize = this.BrushSize;
        cachedOutgoingMessage.brushToggle = this.BrushToggle;

        string body = JsonUtility.ToJson(cachedOutgoingMessage);

        if (logging) Debug.Log("BrushSyles serialized: " + body);
        return body;
    }

    private BrushStylesMessage cachedIncomingMessage = new BrushStylesMessage();
    public void DeSerializeBrushStyles(string body)
    {
        if (logging) Debug.Log("BrushSyles deserialized: " + body);

        // Unpack Brushstyles
        JsonUtility.FromJsonOverwrite(body, cachedIncomingMessage);

        // Apply Brushstyles values to self
        // Assume only one parameter changes on each message

        if (cachedIncomingMessage.brushSize != this.BrushSize)
        {
            this.BrushSize = cachedIncomingMessage.brushSize;
        }
        else if (cachedIncomingMessage.brushToggle != this.BrushToggle)
        {
            this.BrushToggle = cachedIncomingMessage.brushToggle;
        }
        else if (cachedIncomingMessage.brushColor != this.BrushColor || cachedIncomingMessage.customColor != this.CustomColor)
        {
            if (cachedIncomingMessage.brushColor == BrushColor.Custom)
            {
                this.CustomColor = cachedIncomingMessage.customColor;
            }
            else
            {
                this.BrushColor = cachedIncomingMessage.brushColor;
            }
        }
    }

    [Serializable]
    public class BrushStylesMessage
    {
        public BrushColor brushColor;
        public Color customColor;
        public BrushSize brushSize;
        public BrushUpDownState brushToggle;
    }

}
