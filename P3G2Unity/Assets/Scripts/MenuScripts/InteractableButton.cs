using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableButton : MonoBehaviour
{
    public float timeToComplete = 3f;
    public float heldTime;
    public float fillAmount;
    public Image progressCircle;

    public void UpdateFillAmount()
    {
        heldTime += Time.fixedDeltaTime;
        fillAmount = heldTime / timeToComplete;
        progressCircle.fillAmount = fillAmount;
        if (fillAmount >= 1)
        {
            ResetValues();
            FullyPressed();
        }
    }
    public void ResetValues()
    {
        heldTime = 0f;
        fillAmount = 0f;
        progressCircle.fillAmount = 0f;
    }
    public abstract void FullyPressed();
}
