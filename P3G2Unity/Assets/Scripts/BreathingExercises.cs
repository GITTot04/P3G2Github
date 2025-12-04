using UnityEngine;
using TMPro;
using System.Collections;

public class BreathingExercises : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public GameObject nextButton;

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
                textMesh.text = "Inhale for " + (4 - count) + " seconds";
                count++;
                yield return new WaitForSeconds(1);
            }
            count = 0;
            while (count < 8)
            {
                textMesh.text = "Hold for " + (7 - count) + " seconds";
                count++;
                yield return new WaitForSeconds(1);
            }
            count = 0;
            while (count < 9)
            {
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
