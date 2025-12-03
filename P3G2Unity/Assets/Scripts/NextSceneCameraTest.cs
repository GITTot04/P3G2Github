using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextSceneCameraTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.aKey.isPressed)
        {
            SceneManager.LoadScene(1);
        }
    }
}
