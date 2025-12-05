using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class BreathingExercises : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public GameObject nextButton;
    public Image backGroundImage;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (true)
        {
            int count = 0;
            while (count < 5)
            {
                backGroundImage.color = new Color(146f/255f, 171f/255f, 211f/255f);
                textMesh.text = "Inhale for " + (4 - count) + " seconds";
                count++;
                yield return new WaitForSeconds(1);
            }
            count = 0;
            while (count < 8)
            {
                backGroundImage.color = new Color(228f/255f, 156f/255f, 149f/255f);
                textMesh.text = "Hold for " + (7 - count) + " seconds";
                count++;
                yield return new WaitForSeconds(1);
            }
            count = 0;
            while (count < 9)
            {
                backGroundImage.color = new Color(250f/255f, 192f/255f, 152f/255f);
                textMesh.text = "Exhale for " + (8 - count) + " seconds";
                count++;
                yield return new WaitForSeconds(1);
            }
            if (!nextButton.activeSelf)
            {
                nextButton.SetActive(true);
            }
        }
    }
}
