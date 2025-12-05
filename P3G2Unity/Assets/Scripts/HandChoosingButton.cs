using UnityEngine;
using UnityEngine.SceneManagement;

public class HandChoosingButton : InteractableButton
{
    public bool drawinghand;
    public override void FullyPressed()
    {
        MenuController.menuControllerInstance.drawingHand = drawinghand;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
