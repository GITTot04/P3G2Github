using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ColourChoosing : MonoBehaviour
{
    GameObject colourMenu;
    public GameObject resetCircle;
    public GameObject resetCircle1;
    float resetFillAmount;
    float timeToReset = 5f;
    float resetHeldTime;
    public Test handController;
    public Color drawingColour = Color.gray;
    public Transform mainCameraTransform;
    public LayerMask layerMaskUI;

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
            Physics.Raycast(ray, out hit, layerMaskUI);
            if (hit.collider != null)
            {
                switch (hit.collider.gameObject.name)
                {
                    case "Gray":
                        drawingColour = Color.gray;
                        break;
                    case "Blue":
                        drawingColour = Color.blue;
                        break;
                    case "Red":
                        drawingColour = Color.red;
                        break;
                    case "Green":
                        drawingColour = Color.green;
                        break;
                    case "Yellow":
                        drawingColour = Color.yellow;
                        break;
                    case "Black":
                        drawingColour = Color.black;
                        break;
                    case "Magenta":
                        drawingColour = Color.magenta;
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
    }
}
