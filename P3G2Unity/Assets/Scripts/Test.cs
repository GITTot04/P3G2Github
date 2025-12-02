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
    public GameObject drawingSphere;
    public bool drawingHand; // 0 = left, 1 = right
    GameObject colourMenu;
    GameObject doneMenu;
    public bool doneDrawing;
    public static GameObject meshContainer;



    public bool mouseDraw = false;
    bool mouseDown = false;

    public DrawManager drawManager;
    private void Awake()
    {
        followingParticles = drawingHandTracker.transform.GetChild(0).GetComponent<ParticleSystem>();
        colourMenu = nonDrawingHandTracker.transform.GetChild(0).gameObject;
        doneMenu = nonDrawingHandTracker.transform.GetChild(1).gameObject;
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
            //Shader stuff:
            Shader.SetGlobalVector("_HandPosition", drawingHandTracker.transform.position);


            //The rest:

            leftHandPositionInvertedYZ = new Vector3(leftHandPosition.x, leftHandPosition.y * -1, leftHandPosition.z * -1);
            leftShoulderPositionInvertedY = new Vector3(leftShoulderPosition.x, leftShoulderPosition.y * -1, leftShoulderPosition.z);
            rightHandPositionInvertedYZ = new Vector3(rightHandPosition.x, rightHandPosition.y * -1, rightHandPosition.z * -1);
            rightShoulderPositionInvertedY = new Vector3(rightShoulderPosition.x, rightShoulderPosition.y * -1, rightShoulderPosition.z);
            if (!doneDrawing)
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
                }
            }
            else
            {
                if (colourMenu.activeSelf)
                {
                    colourMenu.SetActive(false);
                    StopAllCoroutines();
                    curser.SetActive(false);
                }
                if (!doneMenu.activeSelf)
                {
                    followingParticles.startColor = Color.red;
                    doneMenu.SetActive(true);
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

    public static void Calibrate()
    {
        chestPositionInvertedYZ = new Vector3(chestPosition.x, chestPosition.y * -1, chestPosition.z * -1);

        meshContainer.transform.position = new Vector3(chestPositionInvertedYZ.x, chestPositionInvertedYZ.y, chestPositionInvertedYZ.z - 10f);
    }

    IEnumerator InitialCalibration()
    {
        while (chestPosition == new Vector3(0, 0, 0))
        {
            yield return null;
        }
        Calibrate();
        yield return null;
    }
}
