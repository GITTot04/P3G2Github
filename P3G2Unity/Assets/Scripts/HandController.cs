using UnityEngine;
public class HandController : MonoBehaviour
{
    public static Vector3 leftHandPosition;
    public static Vector3 leftShoulderPosition;
    public static Vector3 rightHandPosition;
    public static Vector3 rightShoulderPosition;
    Vector3 leftHandPositionInvertedY;
    Vector3 leftShoulderPositionInvertedY;
    Vector3 rightHandPositionInvertedY;
    Vector3 rightShoulderPositionInvertedY;
    public GameObject drawingHandTracker;
    public GameObject nonDrawingHandTracker;
    public GameObject curser;
    ParticleSystem followingParticles;
    int stashedSpherePositions;
    public GameObject drawingSphere;
    public bool drawingHand; // 0 = left, 1 = right
    GameObject colourMenu;

    public bool mouseDraw = false;
    bool mouseDown = false;

    public DrawManager drawManager;
    private void Awake()
    {
        followingParticles = drawingHandTracker.transform.GetChild(0).GetComponent<ParticleSystem>();
        colourMenu = nonDrawingHandTracker.transform.GetChild(0).gameObject;
    }
    void FixedUpdate()
    {
        if (!mouseDraw)
        {
            leftHandPositionInvertedY = new Vector3(leftHandPosition.x, leftHandPosition.y * -1, leftHandPosition.z);
            leftShoulderPositionInvertedY = new Vector3(leftShoulderPosition.x, leftShoulderPosition.y * -1, leftShoulderPosition.z);
            rightHandPositionInvertedY = new Vector3(rightHandPosition.x, rightHandPosition.y * -1, rightHandPosition.z);
            rightShoulderPositionInvertedY = new Vector3(rightShoulderPosition.x, rightShoulderPosition.y * -1, rightShoulderPosition.z);
            if (stashedSpherePositions < 40)
            {
                if (!drawingHand && Mathf.Abs(rightHandPositionInvertedY.y - rightShoulderPositionInvertedY.y) > 0.2) // 0 = left, 1 = right
                {
                    if (colourMenu.activeSelf)
                    {
                        StopAllCoroutines();
                        colourMenu.SetActive(false);
                        curser.SetActive(false);
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, leftHandPositionInvertedY * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, rightHandPositionInvertedY * 8, 0.1f);

                    drawManager.Draw(drawingHandTracker.transform.position);
                }
                else if (drawingHand && Mathf.Abs(leftHandPositionInvertedY.y - leftShoulderPositionInvertedY.y) > 0.2)
                {
                    if (colourMenu.activeSelf)
                    {
                        StopAllCoroutines();
                        colourMenu.SetActive(false);
                        curser.SetActive(false);
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, rightHandPositionInvertedY * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, leftHandPositionInvertedY * 8, 0.1f);

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
                        drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, leftHandPositionInvertedY * 8, 0.1f);
                        nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, rightHandPositionInvertedY * 8, 0.1f);
                    }
                    else
                    {
                        drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, rightHandPositionInvertedY * 8, 0.1f);
                        nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, leftHandPositionInvertedY * 8, 0.1f);
                    }
                }
            }
        }
        else
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
}
