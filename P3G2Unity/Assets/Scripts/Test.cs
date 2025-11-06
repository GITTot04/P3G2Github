using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject leftFingertip;
    public GameObject leftHand;
    public GameObject leftParticleSystem;
    void Start()
    {
        
    }

    void Update()
    {
        if (leftFingertip.transform.position.y - leftHand.transform.position.y > 0.05f && !leftParticleSystem.activeInHierarchy)
        {
            leftParticleSystem.SetActive(true);
        }
        else if(leftFingertip.transform.position.y - leftHand.transform.position.y <= 0.05f && leftParticleSystem.activeInHierarchy)
        {
            leftParticleSystem.SetActive(false);
        }
    }
}
