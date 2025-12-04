using UnityEngine;
using UnityEngine.Video;

public class VideoButton : InteractableButton
{
    public VideoPlayer videoPlayer;
    public GameObject videoScreen;
    public GameObject beforeButtons;
    public GameObject afterButtons;
    public override void FullyPressed()
    {
        beforeButtons.SetActive(false);
        afterButtons.SetActive(false);
        MenuController.menuControllerInstance.mainHand.SetActive(false);
        videoPlayer.GetComponent<VideoPlayer>().frame = 0;
        videoScreen.SetActive(true);
        videoPlayer.Play();
    }
}
