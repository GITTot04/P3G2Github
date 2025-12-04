using UnityEngine;
using UnityEngine.UI;

public class UICameraForUICanvas : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().worldCamera = MenuController.menuControllerInstance.UICamera;
    }
}
