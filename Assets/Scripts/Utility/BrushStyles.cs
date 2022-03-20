using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[CreateAssetMenu(fileName = "BrushStyles", menuName = "Utils/BrushStyles")]
public class BrushStyles : ScriptableObject
{
    public float Small = 0.01f;
    public float Medium = 0.02f;
    public float Large = 0.04f;
    public float WobbleModifier = 0.16f;


    // value array
    private float[] val = new float[3]; 

    // indexer array 
    private string[] indices = {"Small","Medium","Large"};
  
    public float this[string index] 
    { 
        get
        { 
            val[0] = Small; val[1] = Medium; val[2] = Large; 
            return val[Array.IndexOf(indices,index)]; 
        } 
    } 
}
