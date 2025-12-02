using UnityEngine;
using System.Collections;
public class Test : MonoBehaviour
{
    public static Vector3 leftHandPosition;
    public static Vector3 leftShoulderPosition;
    public static Vector3 rightHandPosition;
    public static Vector3 rightShoulderPosition;
    public static Vector3 chestPosition;
    Vector3 leftHandPositionInvertedYZ;
    Vector3 leftShoulderPositionInvertedY;
    Vector3 rightHandPositionInvertedYZ;
    Vector3 rightShoulderPositionInvertedY;
    static Vector3 chestPositionInvertedYZ;
    public GameObject drawingHandTracker;
    public GameObject nonDrawingHandTracker;
    public GameObject curser;
    ParticleSystem followingParticles;
    // Vector3[] spherePositions = new Vector3[40];
    int stashedSpherePositions;
    public GameObject drawingSphere;
    public bool drawingHand; // 0 = left, 1 = right
    GameObject colourMenu;
    public static GameObject meshContainer;


    public bool mouseDraw = false;
    bool mouseDown = false;

    public DrawManager drawManager;
    private void Awake()
    {
        followingParticles = drawingHandTracker.transform.GetChild(0).GetComponent<ParticleSystem>();
        colourMenu = nonDrawingHandTracker.transform.GetChild(0).gameObject;
        meshContainer = GameObject.Find("DrawMeshes");
    }
    private void Start()
    {
        StartCoroutine(InitialCalibration());
    }
    void FixedUpdate()
    {
        if (!mouseDraw)
        {
            leftHandPositionInvertedYZ = new Vector3(leftHandPosition.x, leftHandPosition.y * -1, leftHandPosition.z * -1);
            leftShoulderPositionInvertedY = new Vector3(leftShoulderPosition.x, leftShoulderPosition.y * -1, leftShoulderPosition.z);
            rightHandPositionInvertedYZ = new Vector3(rightHandPosition.x, rightHandPosition.y * -1, rightHandPosition.z * -1);
            rightShoulderPositionInvertedY = new Vector3(rightShoulderPosition.x, rightShoulderPosition.y * -1, rightShoulderPosition.z);
            if (stashedSpherePositions < 40)
            {
                if (!drawingHand && Mathf.Abs(rightHandPositionInvertedYZ.y - rightShoulderPositionInvertedY.y) > 0.2) // 0 = left, 1 = right
                {
                    if (colourMenu.activeSelf)
                    {
                        StopAllCoroutines();
                        colourMenu.SetActive(false);
                        curser.SetActive(false);
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, leftHandPositionInvertedYZ * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, rightHandPositionInvertedYZ * 8, 0.1f);

                    drawManager.Draw(drawingHandTracker.transform.position);
                    /*spherePositions[stashedSpherePositions] = drawingHandTracker.transform.position;
                    stashedSpherePositions++;
                    */
                }
                else if (drawingHand && Mathf.Abs(leftHandPositionInvertedYZ.y - leftShoulderPositionInvertedY.y) > 0.2)
                {
                    if (colourMenu.activeSelf)
                    {
                        StopAllCoroutines();
                        colourMenu.SetActive(false);
                        curser.SetActive(false);
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, rightHandPositionInvertedYZ * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, leftHandPositionInvertedYZ * 8, 0.1f);

                    drawManager.Draw(drawingHandTracker.transform.position);
                    /*spherePositions[stashedSpherePositions] = drawingHandTracker.transform.position;
                    stashedSpherePositions++;
                    */
                }
                else
                {
                    followingParticles.startColor = Color.red;
                    if (!colourMenu.activeSelf)
                    {
                        colourMenu.SetActive(true);
                        StartCoroutine(nonDrawingHandTracker.GetComponent<ColourChoosing>().PickingColours());
                        curser.SetActive(true);
                    }
                    if (!drawingHand)
                    {
                        drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, leftHandPositionInvertedYZ * 8, 0.1f);
                        nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, rightHandPositionInvertedYZ * 8, 0.1f);
                    }
                    else
                    {
                        drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, rightHandPositionInvertedYZ * 8, 0.1f);
                        nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, leftHandPositionInvertedYZ * 8, 0.1f);
                    }
                    if (stashedSpherePositions != 0)
                    {
                        //drawSpheres();
                    }
                }
            }
            else
            {
                //drawSpheres();
            }
        }
        else if (mouseDraw)
        {

            if (mouseDown)
            {
                drawManager.Draw(MouseTracker.worldPos);
            }
          
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }
    }
    /*
    void drawSpheres()
    {
        for (int i = 0; i < stashedSpherePositions; i++)
        {
            Instantiate(drawingSphere, spherePositions[i], new Quaternion(0, 0, 0, 0));
        }
        stashedSpherePositions = 0;
    }
    */

    public static void Calibrate()
    {
        chestPositionInvertedYZ = new Vector3(chestPosition.x, chestPosition.y * -1, chestPosition.z * -1);
        Debug.Log(meshContainer.name);
        meshContainer.transform.position = new Vector3(chestPositionInvertedYZ.x, chestPositionInvertedYZ.y, chestPositionInvertedYZ.z - 4f);
    }

    IEnumerator InitialCalibration()
    {
        while (chestPosition == new Vector3(0,0,0))
        {
            yield return null;
        }
        Calibrate();
        yield return null;
    }
}
