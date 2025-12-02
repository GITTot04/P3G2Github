using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColourChoosing : MonoBehaviour
{
    GameObject colourMenu;
    GameObject doneMenu;
    public GameObject resetCircle;
    public GameObject resetCircle1;
    float resetFillAmount;
    public GameObject calibrateCircle;
    float calibrateFillAmount;
    public GameObject doneCircle;
    public GameObject doneCircle1;
    float doneFillAmount;
    float timeToReset = 5f;
    float resetHeldTime;
    float timeToCalibrate = 5f;
    float calibrateHeldTime;
    float timeToDone = 5f;
    float doneHeldTime;
    public Test handController;
    public Color drawingColour = Color.gray;
    public Transform mainCameraTransform;
    public LayerMask layerMaskUI;
    public Material[] pureColours = new Material[7];
    public Renderer cursorColourRenderer;
    public GameObject allDrawnBallsContainer;
    float rotationSpeed = 0.2f;

    private void Awake()
    {
        colourMenu = transform.GetChild(0).gameObject;
        doneMenu = transform.GetChild(1).gameObject;
        if (!handController.drawingHand)
        {
            colourMenu.transform.localPosition = new Vector3(2, 0, 0);
            doneMenu.transform.localPosition = new Vector3(2, 0, 0);
        }
        else
        {
            colourMenu.transform.localPosition = new Vector3(-2, 0, 0);
            doneMenu.transform.localPosition = new Vector3(-2, 0, 0);
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
                        ResetFillAmounts();
                        break;
                    case "Blue":
                        drawingColour = Color.blue;
                        cursorColourRenderer.material = pureColours[1];
                        ResetFillAmounts();
                        break;
                    case "Red":
                        drawingColour = Color.red;
                        cursorColourRenderer.material = pureColours[2];
                        ResetFillAmounts();
                        break;
                    case "Green":
                        drawingColour = Color.green;
                        cursorColourRenderer.material = pureColours[3];
                        ResetFillAmounts();
                        break;
                    case "Yellow":
                        drawingColour = Color.yellow;
                        cursorColourRenderer.material = pureColours[4];
                        ResetFillAmounts();
                        break;
                    case "Black":
                        drawingColour = Color.black;
                        cursorColourRenderer.material = pureColours[5];
                        ResetFillAmounts();
                        break;
                    case "Magenta":
                        drawingColour = Color.magenta;
                        cursorColourRenderer.material = pureColours[6];
                        ResetFillAmounts();
                        break;
                    case "Reset":
                        resetHeldTime += Time.deltaTime;
                        resetFillAmount = resetHeldTime / timeToReset;
                        if (!handController.doneDrawing)
                        {
                            resetCircle.GetComponent<Image>().fillAmount = resetFillAmount;
                        }
                        else
                        {
                            resetCircle1.GetComponent<Image>().fillAmount = resetFillAmount;
                        }
                        if (resetFillAmount >= 1)
                        {
                            ResetDrawing();
                        }
                        ResetFillAmounts("reset");
                        break;
                    case "Calibrate":
                        calibrateHeldTime += Time.deltaTime;
                        calibrateFillAmount = calibrateHeldTime / timeToCalibrate;
                        calibrateCircle.GetComponent<Image>().fillAmount = calibrateFillAmount;
                        if (calibrateFillAmount >= 1)
                        {
                            calibrateHeldTime = 0f;
                            calibrateFillAmount = 0f;
                            Test.Calibrate();
                        }
                        ResetFillAmounts("calibrate");
                        break;
                    case "Turn Left":
                        allDrawnBallsContainer.transform.RotateAround(allDrawnBallsContainer.transform.position, new Vector3(0, 1, 0), rotationSpeed);
                        break;
                    case "Turn Right":
                        allDrawnBallsContainer.transform.RotateAround(allDrawnBallsContainer.transform.position, new Vector3(0, 1, 0), rotationSpeed);
                        break;
                    case "Done":
                        doneHeldTime += Time.deltaTime;
                        doneFillAmount = doneHeldTime / timeToDone;
                        if (!handController.doneDrawing)
                        {
                            doneCircle.GetComponent<Image>().fillAmount = doneFillAmount;
                        }
                        else
                        {
                            doneCircle1.GetComponent<Image>().fillAmount = doneFillAmount;
                        }
                        if (doneFillAmount >= 1 && !handController.doneDrawing)
                        {
                            doneHeldTime = 0f;
                            doneFillAmount = 0f;
                            handController.doneDrawing = true;
                        }
                        else if (doneFillAmount >= 1)
                        {
                            // Set scene to main menu
                        }
                        ResetFillAmounts("done");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ResetFillAmounts();
            }
            yield return null;
        }
    }
    public void ResetDrawing()
    {
        resetHeldTime = 0;
        resetCircle.GetComponent<Image>().fillAmount = 0;
        for (int i = 0; i < allDrawnBallsContainer.transform.childCount; i++)
        {
            Destroy(allDrawnBallsContainer.transform.GetChild(i).gameObject);
        }
    }

    void ResetFillAmounts()
    {
        resetHeldTime = 0;
        if (!handController.doneDrawing)
        {
            resetCircle.GetComponent<Image>().fillAmount = resetFillAmount;
        }
        else
        {
            resetCircle1.GetComponent<Image>().fillAmount = resetFillAmount;
        }
        calibrateHeldTime = 0;
        calibrateCircle.GetComponent<Image>().fillAmount = 0;
        doneHeldTime = 0;
        if (!handController.doneDrawing)
        {
            doneCircle.GetComponent<Image>().fillAmount = doneFillAmount;
        }
        else
        {
            doneCircle1.GetComponent<Image>().fillAmount = doneFillAmount;
        }
    }
    void ResetFillAmounts(string exception)
    {
        switch (exception)
        {
            case "reset":
                calibrateHeldTime = 0;
                calibrateCircle.GetComponent<Image>().fillAmount = 0;
                doneHeldTime = 0;
                if (!handController.doneDrawing)
                {
                    doneCircle.GetComponent<Image>().fillAmount = doneFillAmount;
                }
                else
                {
                    doneCircle1.GetComponent<Image>().fillAmount = doneFillAmount;
                }
                break;
            case "calibrate":
                resetHeldTime = 0;
                if (!handController.doneDrawing)
                {
                    resetCircle.GetComponent<Image>().fillAmount = resetFillAmount;
                }
                else
                {
                    resetCircle1.GetComponent<Image>().fillAmount = resetFillAmount;
                }
                doneHeldTime = 0;
                if (!handController.doneDrawing)
                {
                    doneCircle.GetComponent<Image>().fillAmount = doneFillAmount;
                }
                else
                {
                    doneCircle1.GetComponent<Image>().fillAmount = doneFillAmount;
                }
                break;
            case "done":
                resetHeldTime = 0;
                if (!handController.doneDrawing)
                {
                    resetCircle.GetComponent<Image>().fillAmount = resetFillAmount;
                }
                else
                {
                    resetCircle1.GetComponent<Image>().fillAmount = resetFillAmount;
                }
                calibrateHeldTime = 0;
                calibrateCircle.GetComponent<Image>().fillAmount = 0;
                break;
            default:
                break;
        }
    }

}
