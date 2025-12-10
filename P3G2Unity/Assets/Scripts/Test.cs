using UnityEngine;
using System.Collections;
public class Test : MonoBehaviour
{
    static Vector3 chestPositionInvertedYZ;
    public GameObject drawingHandTracker;
    public GameObject nonDrawingHandTracker;
    public GameObject cursor;
    ParticleSystem followingParticles;
    public GameObject drawingSphere;
    GameObject colourMenu;
    GameObject doneMenu;
    public bool doneDrawing;
    public static GameObject meshContainer;

    public static float calibrateZCompensator = -15f;
    static float calibrationSensitivity = 1.8f;



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
            if (!doneDrawing)
            {
                if (doneMenu.activeSelf)
                {
                    StopAllCoroutines();
                    doneMenu.SetActive(false);
                    cursor.SetActive(false);
                }
                if (!MenuController.menuControllerInstance.drawingHand && Mathf.Abs(MenuController.menuControllerInstance.rightHandPositionInvertedYZ.y - MenuController.menuControllerInstance.rightShoulderPositionInvertedY.y) > 0.2)
                {
                    if (colourMenu.activeSelf)
                    {
                        StopAllCoroutines();
                        colourMenu.SetActive(false);
                        cursor.SetActive(false);
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, MenuController.menuControllerInstance.leftHandPositionInvertedYZ * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, MenuController.menuControllerInstance.rightHandPositionInvertedYZ * 8, 0.1f);

                    drawManager.Draw(drawingHandTracker.transform.position);
                }
                else if (MenuController.menuControllerInstance.drawingHand && Mathf.Abs(MenuController.menuControllerInstance.leftHandPositionInvertedYZ.y - MenuController.menuControllerInstance.leftShoulderPositionInvertedY.y) > 0.2)
                {
                    if (colourMenu.activeSelf)
                    {
                        StopAllCoroutines();
                        colourMenu.SetActive(false);
                        cursor.SetActive(false);
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, MenuController.menuControllerInstance.rightHandPositionInvertedYZ * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, MenuController.menuControllerInstance.leftHandPositionInvertedYZ * 8, 0.1f);

                    drawManager.Draw(drawingHandTracker.transform.position);
                }
                else
                {
                    followingParticles.startColor = Color.red;
                    if (!colourMenu.activeSelf)
                    {
                        colourMenu.SetActive(true);
                        StartCoroutine(nonDrawingHandTracker.GetComponent<ColourChoosing>().PickingColours());
                        cursor.SetActive(true);
                    }
                    if (!MenuController.menuControllerInstance.drawingHand)
                    {
                        drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, MenuController.menuControllerInstance.leftHandPositionInvertedYZ * 8, 0.1f);
                        nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, MenuController.menuControllerInstance.rightHandPositionInvertedYZ * 8, 0.1f);
                    }
                    else
                    {
                        drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, MenuController.menuControllerInstance.rightHandPositionInvertedYZ * 8, 0.1f);
                        nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, MenuController.menuControllerInstance.leftHandPositionInvertedYZ * 8, 0.1f);
                    }
                }
            }
            else
            {
                if (colourMenu.activeSelf)
                {
                    colourMenu.SetActive(false);
                    StopAllCoroutines();
                    cursor.SetActive(false);
                }
                if (!doneMenu.activeSelf)
                {
                    followingParticles.startColor = Color.red;
                    doneMenu.SetActive(true);
                    StartCoroutine(nonDrawingHandTracker.GetComponent<ColourChoosing>().PickingColours());
                    cursor.SetActive(true);
                }
                if (!MenuController.menuControllerInstance.drawingHand)
                {
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, MenuController.menuControllerInstance.leftHandPositionInvertedYZ * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, MenuController.menuControllerInstance.rightHandPositionInvertedYZ * 8, 0.1f);
                }
                else
                {
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, MenuController.menuControllerInstance.rightHandPositionInvertedYZ * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, MenuController.menuControllerInstance.leftHandPositionInvertedYZ * 8, 0.1f);
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
        chestPositionInvertedYZ = new Vector3(MenuController.chestPosition.x, MenuController.chestPosition.y * -1, MenuController.chestPosition.z * -1);

        meshContainer.transform.position = new Vector3(chestPositionInvertedYZ.x, chestPositionInvertedYZ.y, (chestPositionInvertedYZ.z * calibrationSensitivity + calibrateZCompensator));
    }

    IEnumerator InitialCalibration()
    {
        while (MenuController.chestPosition == new Vector3(0, 0, 0))
        {
            yield return null;
        }
        Calibrate();
        yield return null;
    }
}
