using UnityEngine;

public class main : MonoBehaviour
{
    // Handler for SkeletalTracking thread.
    private SkeletalTrackingProvider m_skeletalTrackingProvider;
    void Start()
    {
        //tracker ids needed for when there are two trackers
        const int TRACKER_ID = 0;
        m_skeletalTrackingProvider = new SkeletalTrackingProvider(TRACKER_ID);
    }
    void OnApplicationQuit()
    {
        if (m_skeletalTrackingProvider != null)
        {
            m_skeletalTrackingProvider.Dispose();
        }
    }
}
