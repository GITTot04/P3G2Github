using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RainbowBackground : MonoBehaviour
{
    Image backgroundImage;
    float r = 200f;
    float g = 255f;
    float b = 255f;
    float speed = 0.25f;
    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (r <= 200.1f && g >= 254.9f && b > 200f)
        {
            b -= speed;
            backgroundImage.color = new Color(r / 255f, g / 255f, b / 255f);
        }
        if (r < 255f && g >= 254.9f && b <= 200.1f)
        {
            r += speed;
            backgroundImage.color = new Color(r / 255f, g / 255f, b / 255f);
        }
        if (r >= 254.9f && g > 200f && b <= 200.1f)
        {
            g -= speed;
            backgroundImage.color = new Color(r / 255f, g / 255f, b / 255f);
        }
        if (r >= 254.9f && g <= 200.1f && b < 255f)
        {
            b += speed;
            backgroundImage.color = new Color(r / 255f, g / 255f, b / 255f);
        }
        if (r > 200f && g <= 200.1f && b >= 254.9f)
        {
            r -= speed;
            backgroundImage.color = new Color(r / 255f, g / 255f, b / 255f);
        }
        if (r <= 200.1f && g < 255f && b >= 254.9f)
        {
            g += speed;
            backgroundImage.color = new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
