using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Container : MonoBehaviour {

    [Header("References")]
    public GameObject metaBallPrefab;
    public ColourChoosing ColourChoosing;
    public GameObject drawZone;
    public Material material;

    [Header("RenderBox settings")]
    public float edgeSize; //Size of edge of renderzone. Prevents cut off meshes.
    public float boxOffsetCompentsater = 0.25f;
    List<GameObject> metaBalls = new List<GameObject>();

    [Header("Gab-fill smoothing")]
    public float gabSmoothDistance = 0.2f;
    public int gabSmoothIterations = 2;
    public float metaBallSmoothOutMinDistance = 0.02f;
    public float metaBallSmoothOutMaxDistance = 0.3f;

    [Header("Splashing")]
    public float timePerPosMin;
    public float timePerPosMax;
    float drawClock = 0;

    [Header("MetaBalls Values")]
    public float safeZone;
    public float resolution;
    public float threshold;
    public ComputeShader computeShader;
    public bool calculateNormals;

    private CubeGrid grid;
    public int gridSize;

    public void Start() {
        this.grid = new CubeGrid(this, this.computeShader);
        Render();
    }
    

    public void Update() {

        drawClock += Time.deltaTime;
    }
    public void InstantiateMetaBalls(Vector3[] globalPositions, int arrayLength, Vector3 lowestPosition) 
    {
           
        transform.position = new Vector3( 
        lowestPosition.x + transform.localScale.x / 2 - edgeSize, 
        lowestPosition.y + transform.localScale.y / 2 - edgeSize, 
        lowestPosition.z + transform.localScale.z / 2 - edgeSize);

        for (int i = 0; i < arrayLength; i++)
        {
            //Smoothing out between gaps;
            if (i > 0 && gabSmoothIterations > 0)
            {
                float gapBetweenBalls = (globalPositions[i] - globalPositions[i - 1]).magnitude;
                for (int j = gabSmoothIterations; j > 0; j--) 
                {

                    if (gapBetweenBalls > gabSmoothDistance*j) //Starts with biggest distance
                    {
                        int fillOutBallsCount = j;
                        for (int k = fillOutBallsCount; k > 0; k--)
                        {
                            float lerpValue = (float)(k / (fillOutBallsCount + 1));
                            Vector3 position = Vector3.Lerp(globalPositions[i] ,globalPositions[i - 1], lerpValue);
                            InstantiateBall(position);
                        }
                        break;
                    }
                }

            }
            InstantiateBall(globalPositions[i]);
        }
        StartCoroutine(Render());

    }
    void InstantiateBall (Vector3 InstPosition) 
    {
        GameObject newMetaBall = Instantiate(metaBallPrefab, this.transform);
        newMetaBall.transform.position = InstPosition;
        newMetaBall.transform.localScale = new Vector3(.06f, .06f, .06f);
        metaBalls.Add(newMetaBall);
    }
    void ClearMetaBalls()
    {
        foreach (GameObject metaBall in metaBalls)
        {
            Destroy(metaBall);
        }
        metaBalls.Clear();
    }
    
    public IEnumerator Render()
    {
        yield return null;
        material.color = ColourChoosing.drawingColour;
        float timePerPos = drawClock / metaBalls.Count;
        float timeToIntensityFactor = (timePerPosMax - timePerPosMin);
        float drawIntensity = 1 - (timePerPos - timePerPosMin) / timeToIntensityFactor;
        Debug.Log(timePerPos);
        if (drawIntensity < 0)
        {
            drawIntensity = 0;
        }
        else if (drawIntensity > 1)
        {
            drawIntensity = 1;
        }
        material.SetFloat("_DrawIntensity", drawIntensity);
       

        this.grid.evaluateAll(this.GetComponentsInChildren<MetaBall>());
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = this.grid.vertices.ToArray();
        mesh.triangles = this.grid.getTriangles();

        RecalculateSmoothNormals(mesh);
       

        GameObject drawZoneNewObj = new GameObject();
        drawZoneNewObj.transform.localScale = transform.localScale;

        //First set scale then parent!
        drawZoneNewObj.transform.SetParent(drawZone.transform);
        Mesh independentMesh = Instantiate(mesh); //Mesh needs to be independent
        drawZoneNewObj.AddComponent<MeshFilter>().mesh = independentMesh;
        drawZoneNewObj.AddComponent<MeshRenderer>().material = Instantiate(material);
        
        drawZoneNewObj.transform.position = transform.position;
        mesh.Clear();

        ClearMetaBalls();
        drawClock = 0f;

    }
  
    public static void RecalculateSmoothNormals(Mesh mesh, float mergeEpsilon = 0.000001f, float smoothingAngle = 180f)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        //Merge nearby vertices
        Dictionary<int, int> mergeIndices = new Dictionary<int, int>();
        List<Vector3> mergedVertices = new List<Vector3>();
        int[] remap = new int[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < mergedVertices.Count; j++)
            {
                if (Vector3.Distance(vertices[i], mergedVertices[j]) <= mergeEpsilon)
                {
                    remap[i] = j;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                mergedVertices.Add(vertices[i]);
                remap[i] = mergedVertices.Count - 1;
            }
        }

        //Remap triangles
        int[] newTriangles = new int[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
            newTriangles[i] = remap[triangles[i]];

        Mesh tempMesh = new Mesh();
        tempMesh.vertices = mergedVertices.ToArray();
        tempMesh.triangles = newTriangles;

        //Recalculate normals
        tempMesh.RecalculateNormals();

        Vector3[] smoothNormals = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            smoothNormals[i] = tempMesh.normals[remap[i]];

        
        //Apply normals to original mesh
        mesh.normals = smoothNormals;
    }
}