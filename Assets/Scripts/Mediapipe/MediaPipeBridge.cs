using UnityEngine;

public class MediaPipeBridge : MonoBehaviour
{
    public void UpdateLandmarks(
        Vector3 leftHip,
        Vector3 rightHip,
        Vector3 leftShoulder,
        Vector3 rightShoulder,
        Vector3 leftWrist,
        Vector3 rightWrist)
    {
        PoseManager.Instance.UpdatePoseData(
            leftHip,
            rightHip,
            leftShoulder,
            rightShoulder,
            leftWrist,
            rightWrist
        );
    }
}