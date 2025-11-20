using UnityEngine;
using System.Collections;

public class ColourChoosing : MonoBehaviour
{
    GameObject colourMenu;
    public Test handController;
    public Color drawingColour = Color.gray;

    private void Awake()
    {
        colourMenu = transform.GetChild(0).gameObject;
        if (!handController.drawingHand)
        {
            colourMenu.transform.localPosition = new Vector3(1, 0, 0);
        }
        else
        {
            colourMenu.transform.localPosition = new Vector3(-1, 0, 0);
        }
    }

    public IEnumerator PickingColours()
    {
        while (true)
        {
            RaycastHit hit;
            Ray ray = new Ray(handController.drawingHandTracker.transform.position, new Vector3(0, 0, -1));
            Physics.Raycast(ray, out hit);
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
                    default:
                        break;
                }
            }
            yield return null;
        }
    }
}
