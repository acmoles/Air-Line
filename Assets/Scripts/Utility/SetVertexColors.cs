using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetVertexColors : MonoBehaviour
{
    [SerializeField]
    private Color color1 = Color.blue;

    [SerializeField]
    private Color color2 = Color.green;

    [SerializeField]
    private MeshFilter meshFilter = null;

    void Start()
    {
        SetMeshColors();
    }

    void OnValidate()
    {
        SetMeshColors();
    }

    void SetMeshColors()
    {
        if (meshFilter)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
                colors[i] = Color.Lerp(color1, color2, vertices[i].y);

            // assign the array of colors to the Mesh.
            mesh.colors = colors;
        }
    }
}
