using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer videoplayer;
    public GameObject videoScreen;
    public GameObject afterButtons;
    private void OnEnable()
    {
        videoplayer.loopPointReached += VideoStopped;
    }
    private void OnDisable()
    {
        videoplayer.loopPointReached -= VideoStopped;
    }

    void VideoStopped(VideoPlayer vp)
    {
        videoScreen.SetActive(false);
        afterButtons.SetActive(true);
        MenuController.menuControllerInstance.mainHand.SetActive(true);
    }
}
