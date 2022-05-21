using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeDrawer : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;

    [SerializeField]
    private PositionReporter[] positionReporters;

    [SerializeField]
    private BrushStyles brushStyles;
    // TODO future enhancement: array of brushStyles objects for each position reporter

    private bool scheduledRestart = false;

    public BrushStyles Styles
    {
        get
        {
            return brushStyles;
        }
        set
        {
            brushStyles = value;
        }
    }

    private DrawState[] drawStates;


    void Awake()
    {
        if (positionReporters.Length == 0)
        {
            Debug.LogWarning("No detectors were specified! TubeDraw cannot draw any lines without detectors.");
        }
    }

    void Start()
    {
        drawStates = new DrawState[positionReporters.Length];
        for (int i = 0; i < positionReporters.Length; i++)
        {
            drawStates[i] = new DrawState(this);
        }
    }

    void Update()
    {
        for (int i = 0; i < positionReporters.Length; i++)
        {
            var reporter = positionReporters[i];
            var drawState = drawStates[i];

            //Restart line if point maximum is reached
            if (drawState.points.Count > Styles.maxPoints && !scheduledRestart)
            {
                drawState.FinishLine();
                scheduledRestart = true;
                break;
            }
            if (scheduledRestart)
            {
                drawState.BeginNewLine();
                scheduledRestart = false;
                break;
            }

            if (reporter.DidStart)
            {
                drawState.BeginNewLine();
            }

            if (reporter.DidStop)
            {
                drawState.FinishLine();
            }

            if (reporter.IsMoving)
            {
                drawState.UpdateLine(reporter.Position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawStates is null) return;

        foreach (DrawState state in drawStates)
        {
            foreach (Vector3 linePoint in state.smoothPoints)
            {
                // Draw circle gizmo
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(linePoint, 0.005f);
            }
        }

    }

    [Serializable]
    private class DrawState
    {
        private Vector3 prevPoint = Vector3.zero;
        public List<Vector3> points = new List<Vector3>();
        public List<Vector3> smoothPoints = new List<Vector3>();
        private List<Color> colors = new List<Color>();
        private List<Color> smoothColors = new List<Color>();
        private List<float> radii = new List<float>();
        private List<float> modifiedRadii = new List<float>();

        private TubeDrawer parent;

        private Tube tube;

        private Mesh mesh;

        public DrawState(TubeDrawer parent)
        {
            this.parent = parent;
        }

        public GameObject BeginNewLine()
        {
            prevPoint = Vector3.zero;
            points.Clear();
            smoothPoints.Clear();
            colors.Clear();
            smoothColors.Clear();
            radii.Clear();
            modifiedRadii.Clear();

            // Create empty tube
            tube = new Tube();

            mesh = new Mesh();
            mesh.name = "Line Mesh";
            mesh.MarkDynamic();

            GameObject lineObj = new GameObject("Line Object");
            lineObj.transform.parent = parent.contentParent;
            lineObj.transform.position = Vector3.zero;
            lineObj.transform.rotation = Quaternion.identity;
            lineObj.transform.localScale = Vector3.one;
            lineObj.AddComponent<MeshFilter>().mesh = mesh;
            lineObj.AddComponent<MeshRenderer>().sharedMaterial = parent.brushStyles.Material;

            return lineObj;
        }

        public void UpdateLine(Vector3 position)
        {

            bool shouldAdd = false;

            shouldAdd |= points.Count == 0;
            shouldAdd |= Vector3.Distance(prevPoint, position) >= parent.brushStyles.minSegmentLength;

            if (shouldAdd)
            {
                smoothPoints.Clear();
                smoothColors.Clear();
                modifiedRadii.Clear();

                points.Add(position);
                colors.Add(parent.Styles.CustomColor);
                radii.Add(parent.Styles[(string)parent.Styles.BrushSize.ToString()]);

                for (int i = 0; i < points.Count; i++)
                {
                    AveragePoints(i);
                }

                if (points.Count >= 2)
                {
                    ModifyRadii();
                    //ModifyColors();
                    tube.Create(
                        smoothPoints.ToArray(),  // Polyline points
                        smoothColors.ToArray(),  // Polyline colors
                        1f,                       // Decimation (not used currently)
                        1f,                       // Scale (not used currently)
                        modifiedRadii.ToArray(),          // Radius at point
                        parent.brushStyles.drawResolution // Circle resolution
                    );
                    UpdateMesh();
                }
                prevPoint = position;
            }
        }

        public void FinishLine()
        {
            mesh.UploadMeshData(true);
        }

        private void UpdateMesh()
        {
            // Post-process end vertices
            for (int i = 0; i < tube.resolution; i++)
            {
                // Start vertices
                tube.vertices[i] = Vector3.Lerp(smoothPoints[0], smoothPoints[1], 0.6f);
            }
            int lastFullRingVerts = tube.vertices.Length - 4 - tube.resolution * 5;
            for (int i = lastFullRingVerts; i < tube.vertices.Length; i++)
            {
                // End vertices
                tube.vertices[i] = Vector3.Lerp(smoothPoints[points.Count - 2], smoothPoints[points.Count - 1], 0.4f);
            }

            mesh.SetVertices(tube.vertices);
            mesh.SetColors(tube.colors);
            mesh.SetUVs(0, tube.uv);
            mesh.SetIndices(tube.tris, MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            // Modify normals
            Vector3[] normals = mesh.normals;

            int lastFullRing = (tube.vertices.Length - 2) - tube.resolution * 5;
            for (int i = tube.resolution - 1; i < lastFullRing; i += tube.resolution)
            {
                normals[i] = normals[i - tube.resolution + 1];
            }
            // End point normal
            for (int i = lastFullRing; i < normals.Length; i++)
            {
                normals[i] = normals[lastFullRing - 1];
            }
            // Start point normal
            Vector3 startNormal = Vector3.zero;
            for (int i = 0; i < tube.resolution; i++)
            {
                startNormal += normals[i];
                startNormal.Normalize();
            }
            for (int i = 0; i < tube.resolution; i++)
            {
                normals[i] = startNormal;
            }
            // assign the array of normals to the mesh
            mesh.SetNormals(normals);
        }

        float ModifyRadii()
        {
            // Iterate polyline length
            for (int i = 0; i < radii.Count; i++)
            {
                // Tapers
                float amount = Mathf.Min(Mathf.Min(1f, (float)i / parent.brushStyles.startTaper), Mathf.Min(1f, (float)(radii.Count - i - 1) / parent.brushStyles.endTaper));
                amount -= 1f;
                amount *= amount;
                amount = 1f - amount;
                //Debug.Log("taper amount: " + amount);

                // Wobble
                float progress = (float)i / radii.Count;
                amount += parent.brushStyles.wobbleModifier * (Mathf.Cos(progress) * Mathf.Cos(progress * 300f) * Mathf.Cos(progress * 800f));
                //Debug.Log("progress: " + progress);
                //_radii[i] = (1f - Mathf.Pow(Mathf.Abs(progress - 0.5f) * 2f, 2f)) * 0.2f;

                // Special case for end vertices
                if (i == 0 || i == radii.Count - 1)
                {
                    amount = 0f;
                }

                modifiedRadii[i] = amount * modifiedRadii[i];
                //Debug.Log(_modifiedRadii[i]);
            }

            return 1f;
        }

        float ModifyColors()
        {
            // TODO rainbow-ish look
            // Iterate polyline length
            for (int i = 0; i < smoothColors.Count; i++)
            {
                // Wobble colors
                float progress = (float)i / smoothColors.Count;
                float amount = 1 + parent.brushStyles.wobbleModifier * (Mathf.Cos(progress) * Mathf.Cos(progress * 300f) * Mathf.Cos(progress * 800f));

                smoothColors[i] = amount * smoothColors[i];
            }

            return 1f;
        }

        void AveragePoints(int index)
        {
            if (index <= parent.brushStyles.startTaper + parent.brushStyles.amountToAverage
                || index >= points.Count - parent.brushStyles.endTaper)
            {
                smoothPoints.Add(points[index]);
                smoothColors.Add(colors[index]);
                modifiedRadii.Add(radii[index]);
                return;
            }

            Vector3 acumulator = Vector3.zero;
            Vector3 averageVector;

            float radiusAcumulator = 0f;
            float averageRadius;

            Color colorAcumulator = Color.black;
            Color averageColor;


            for (int i = -1; i < parent.brushStyles.amountToAverage - 1; i++)
            {
                acumulator += points[index + i];
                radiusAcumulator += radii[index + i];
            }

            for (int i = -1; i < parent.brushStyles.amountToAverageColor - 1; i++)
            {
                colorAcumulator += colors[index + i];
            }

            averageVector = acumulator / (float)parent.brushStyles.amountToAverage;
            smoothPoints.Add(averageVector);

            averageRadius = radiusAcumulator / (float)parent.brushStyles.amountToAverage;
            modifiedRadii.Add(averageRadius);

            averageColor = colorAcumulator / (float)parent.brushStyles.amountToAverageColor;
            smoothColors.Add(averageColor);
        }
    }
}

