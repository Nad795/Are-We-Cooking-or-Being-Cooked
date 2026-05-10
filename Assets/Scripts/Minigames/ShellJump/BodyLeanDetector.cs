using UnityEngine;

public class BodyLeanDetector : MonoBehaviour
{
    public static BodyLeanDetector Instance;

    [Header("Debug")]
    public float hipCenterX;

    [Header("Settings")]
    public float threshold = 0.05f;

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

        // titik tengah pinggul
        hipCenterX =
            (leftHip.x + rightHip.x) / 2f;

        // reset
        moveDirection = 0;

        // kanan
        if(hipCenterX > threshold)
        {
            moveDirection = 1;
        }

        // kiri
        else if(hipCenterX < -threshold)
        {
            moveDirection = -1;
        }
    }
}