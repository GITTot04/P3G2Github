using Unity.VisualScripting;
using UnityEngine;
public class Test : MonoBehaviour
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
    ParticleSystem followingParticles;
    Vector3[] spherePositions = new Vector3[40];
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
                        colourMenu.SetActive(false);
                        StopCoroutine(nonDrawingHandTracker.GetComponent<ColourChoosing>().PickingColours());
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, leftHandPositionInvertedY * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, rightHandPositionInvertedY * 8, 0.1f);

                    drawManager.Draw(drawingHandTracker.transform.position);
                    /*spherePositions[stashedSpherePositions] = drawingHandTracker.transform.position;
                    stashedSpherePositions++;
                    */
                }
                else if (drawingHand && Mathf.Abs(leftHandPositionInvertedY.y - leftShoulderPositionInvertedY.y) > 0.2)
                {
                    if (colourMenu.activeSelf)
                    {
                        colourMenu.SetActive(false);
                        StopCoroutine(nonDrawingHandTracker.GetComponent<ColourChoosing>().PickingColours());
                    }
                    followingParticles.startColor = Color.green;
                    drawingHandTracker.transform.position = Vector3.Lerp(drawingHandTracker.transform.position, rightHandPositionInvertedY * 8, 0.1f);
                    nonDrawingHandTracker.transform.position = Vector3.Lerp(nonDrawingHandTracker.transform.position, leftHandPositionInvertedY * 8, 0.1f);

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

}
