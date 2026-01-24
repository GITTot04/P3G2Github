using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    public bool drawingHand; // 0 = left, 1 = right
    public static MenuController menuControllerInstance; // singleton
    public static Vector3 leftHandPosition;
    public static Vector3 leftShoulderPosition;
    public static Vector3 rightHandPosition;
    public static Vector3 rightShoulderPosition;
    public static Vector3 chestPosition;
    public Vector3 leftHandPositionInvertedYZ;
    public Vector3 leftShoulderPositionInvertedY;
    public Vector3 rightHandPositionInvertedYZ;
    public Vector3 rightShoulderPositionInvertedY;
    public GameObject mainHand;
    public float sensitivity = 5;
    public float menuSensitivity = 6;
    public GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    Vector2 screenpos;
    public Camera UICamera;
    GameObject lastHitGameObject;
    InteractableButton button = null;

    private void Awake()
    {
        if (menuControllerInstance == null)
        {
            menuControllerInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        eventSystem = GetComponent<EventSystem>();
        UICamera = transform.GetChild(0).gameObject.GetComponent<Camera>();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6) // Run this on the drawing scene
        {
            leftHandPositionInvertedYZ = new Vector3(leftHandPosition.x * sensitivity, leftHandPosition.y * -1 * sensitivity, leftHandPosition.z * -1 + sensitivity);
            leftShoulderPositionInvertedY = new Vector3(leftShoulderPosition.x * sensitivity, leftShoulderPosition.y * -1 * sensitivity, leftShoulderPosition.z * sensitivity);
            rightHandPositionInvertedYZ = new Vector3(rightHandPosition.x * sensitivity, rightHandPosition.y * -1 * sensitivity, rightHandPosition.z * -1 * sensitivity);
            rightShoulderPositionInvertedY = new Vector3(rightShoulderPosition.x * sensitivity, rightShoulderPosition.y * -1 * sensitivity, rightShoulderPosition.z * sensitivity);
        }
        else // Run this on menu scenes
        {
            leftHandPositionInvertedYZ = new Vector3(leftHandPosition.x * menuSensitivity, leftHandPosition.y * -1 * menuSensitivity, leftHandPosition.z * -1 + menuSensitivity);
            leftShoulderPositionInvertedY = new Vector3(leftShoulderPosition.x * menuSensitivity, leftShoulderPosition.y * -1 * menuSensitivity, leftShoulderPosition.z * menuSensitivity);
            rightHandPositionInvertedYZ = new Vector3(rightHandPosition.x * menuSensitivity, rightHandPosition.y * -1 * menuSensitivity, rightHandPosition.z * -1 * menuSensitivity);
            rightShoulderPositionInvertedY = new Vector3(rightShoulderPosition.x * menuSensitivity, rightShoulderPosition.y * -1 * menuSensitivity, rightShoulderPosition.z * menuSensitivity);
        }
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex != 6) // Do not run this on the drawing scene
        {
            if (!drawingHand) // 0 = left, 1 = right
            {
                mainHand.transform.position = Vector3.Lerp(mainHand.transform.position, leftHandPositionInvertedYZ * 8, 0.05f);
            }
            else
            {
                mainHand.transform.position = Vector3.Lerp(mainHand.transform.position, rightHandPositionInvertedYZ * 8, 0.05f);
            }
        }

        if (SceneManager.GetActiveScene().buildIndex != 6) // Do not run this on the drawing scene
        {
            // UI specific raycast
            screenpos = UICamera.WorldToScreenPoint(mainHand.transform.position);
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = screenpos;
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);
            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    if (lastHitGameObject == null)
                    {
                        lastHitGameObject = result.gameObject;
                        button = lastHitGameObject.GetComponent<InteractableButton>();
                    }
                    else if (lastHitGameObject == result.gameObject && button != null)
                    {
                        button.UpdateFillAmount();
                    }
                    else
                    {
                        button.ResetValues();
                        lastHitGameObject = result.gameObject;
                        button = lastHitGameObject.GetComponent<InteractableButton>();
                    }
                }
            }
            else
            {
                if (lastHitGameObject != null)
                {
                    button.ResetValues();
                }
            }
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 6) // Do not run this on the drawing scene  
        {
            mainHand = GameObject.FindGameObjectWithTag("Main Hand");
            graphicRaycaster = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<GraphicRaycaster>();
            button = null;
            lastHitGameObject = null;
        }
    }
}
