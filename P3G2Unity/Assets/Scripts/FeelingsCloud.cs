using UnityEngine;

public class FeelingsCloud : InteractableButton
{
    public GameObject nextButton;
    public override void FullyPressed()
    {
        foreach (GameObject cloud in GameObject.FindGameObjectsWithTag("Cloud"))
        {
            if (cloud.name != name)
            {
                cloud.SetActive(false);
            }
        }
        nextButton.SetActive(true);
    }
}
