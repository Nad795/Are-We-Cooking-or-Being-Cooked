using UnityEngine;

public class BodyLeanDetector : MonoBehaviour
{
    public static BodyLeanDetector Instance;

    [Header("Debug")]
    public float hipCenterX;

    [Header("Settings")]
    public float threshold = 0.15f;

    [Header("Output")]
    public float moveDirection;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        DetectLean();
    }

    void DetectLean()
    {
        Vector3 leftHip =
            PoseManager.Instance.leftHip;

        Vector3 rightHip =
            PoseManager.Instance.rightHip;

        hipCenterX =
            (leftHip.x + rightHip.x) / 2f;

        // mirror webcam correction
        hipCenterX *= -1f;

        moveDirection = 0;

        // deadzone
        if(Mathf.Abs(hipCenterX) < threshold)
            return;

        moveDirection =
            Mathf.Sign(hipCenterX);
    }
}