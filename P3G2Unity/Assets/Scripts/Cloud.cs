using UnityEngine;
using TMPro;

public class Cloud : MonoBehaviour
{
    private void Start()
    {
        UpdateText(MenuController.menuControllerInstance.drawingHand);
    }
    public void UpdateText(bool b)
    {
        if (b)
        {
            GetComponent<TextMeshProUGUI>().text = "Control the cursor with your left hand";
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "Control the cursor with your right hand";
        }
    }
}
