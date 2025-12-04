using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleButton : InteractableButton
{
    public int nextScene;
    public override void FullyPressed()
    {
        SceneManager.LoadScene(nextScene);
    }
}
