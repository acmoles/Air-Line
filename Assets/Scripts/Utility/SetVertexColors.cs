using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVertexColors : MonoBehaviour
{
    [SerializeField]
    private Color color1 = Color.blue;

    [SerializeField]
    private Color color2 = Color.green;

    [SerializeField]
    private MeshFilter meshFilter = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    void Start()
    {
        SetMeshColors();
    }

    void OnValidate()
    {
        ValidationUtility.SafeOnValidate(() =>
        {
            SetMeshColors();
        });
    }

    void SetMeshColors()
    {
        if (meshFilter != null)
        {
            if (meshFilter.sharedMesh == null || meshRenderer == null)
            {
                return;
            }
            Mesh meshCopy = new Mesh();
            meshCopy.vertices = meshFilter.sharedMesh.vertices;
            Vector3[] vertices = meshCopy.vertices;

            // create new colors array where the colors will be created.
            Color[] colors = new Color[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
                colors[i] = Color.Lerp(color1, color2, vertices[i].y);

            meshCopy.colors = colors;

            meshRenderer.additionalVertexStreams = meshCopy;
            meshCopy.UploadMeshData(true);
        }
    }

    public void SetColors(Color col1, Color col2) {
        color1 = col1;
        color2 = col2;
        SetMeshColors();
    }
}
