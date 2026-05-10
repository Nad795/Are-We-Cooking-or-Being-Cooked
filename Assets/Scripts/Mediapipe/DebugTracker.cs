using UnityEngine;
public class DebugTracker : MonoBehaviour
{
    public enum TrackType
    {
        LeftWrist,
        RightWrist
    }

    public TrackType trackType;

    void Update()
    {
        if(trackType == TrackType.LeftWrist)
        {
            transform.position =
                PoseManager.Instance.leftWrist;
        }
        else if(trackType == TrackType.RightWrist)
        {
            transform.position =
                PoseManager.Instance.rightWrist;
        }
    }
}