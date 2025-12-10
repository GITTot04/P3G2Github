using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColourChoosing : MonoBehaviour
{
    GameObject colourMenu;
    public GameObject resetCircle;
    public GameObject resetCircle1;
    float resetFillAmount;
    float timeToReset = 5f;
    float resetHeldTime;
    public HandController handController;
    public Color drawingColour = Color.gray;
    public Transform mainCameraTransform;
    public LayerMask layerMaskUI;
    public Material[] pureColours = new Material[7];
    public Renderer cursorColourRenderer;
    public GameObject allDrawnBallsContainer;

    private void Awake()
    {
        colourMenu = transform.GetChild(0).gameObject;
        if (!handController.drawingHand)
        {
            colourMenu.transform.localPosition = new Vector3(2, 0, 0);
        }
        else
        {
            colourMenu.transform.localPosition = new Vector3(-2, 0, 0);
        }
        layerMaskUI = LayerMask.GetMask("UI");
    }

    public IEnumerator PickingColours()
    {
        while (true)
        {
            RaycastHit hit;
            Vector3 rayDirection = handController.drawingHandTracker.transform.position - mainCameraTransform.position;
            Ray ray = new Ray(mainCameraTransform.position, rayDirection);
            Physics.Raycast(ray, out hit, (colourMenu.transform.position - mainCameraTransform.position).magnitude + 1f, layerMaskUI);
            if (hit.collider != null)
            {
                switch (hit.collider.gameObject.name)
                {
                    case "Gray":
                        drawingColour = Color.gray;
                        cursorColourRenderer.material = pureColours[0];
                        break;
                    case "Blue":
                        drawingColour = Color.blue;
                        cursorColourRenderer.material = pureColours[1];
                        break;
                    case "Red":
                        drawingColour = Color.red;
                        cursorColourRenderer.material = pureColours[2];
                        break;
                    case "Green":
                        drawingColour = Color.green;
                        cursorColourRenderer.material = pureColours[3];
                        break;
                    case "Yellow":
                        drawingColour = Color.yellow;
                        cursorColourRenderer.material = pureColours[4];
                        break;
                    case "Black":
                        drawingColour = Color.black;
                        cursorColourRenderer.material = pureColours[5];
                        break;
                    case "Magenta":
                        drawingColour = Color.magenta;
                        cursorColourRenderer.material = pureColours[6];
                        break;
                    case "Reset":
                        resetHeldTime += Time.deltaTime;
                        resetFillAmount = resetHeldTime / timeToReset;
                        resetCircle.GetComponent<Image>().fillAmount = resetFillAmount;
                        resetCircle1.GetComponent<Image>().fillAmount = resetFillAmount;
                        if (resetFillAmount >= 1)
                        {
                            ResetDrawing();
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                resetHeldTime = 0;
                resetCircle.GetComponent<Image>().fillAmount = 0;
                resetCircle1.GetComponent<Image>().fillAmount = 0;
            }
            yield return null;
        }
    }
    public void ResetDrawing()
    {
        resetHeldTime = 0;
        resetCircle.GetComponent<Image>().fillAmount = 0;
        resetCircle1.GetComponent<Image>().fillAmount = 0;
        for (int i = 0; i < allDrawnBallsContainer.transform.childCount; i++)
        {
            Destroy(allDrawnBallsContainer.transform.GetChild(i).gameObject);
        }
    }
}
