using UnityEngine;

public class PoseManager : MonoBehaviour
{
    public static PoseManager Instance;

    [Header("Landmarks")]
    public Vector3 nose;

    public Vector3 leftShoulder;
    public Vector3 rightShoulder;

    public Vector3 leftHip;
    public Vector3 rightHip;

    public Vector3 leftWrist;
    public Vector3 rightWrist;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Vector3 ConvertToWorld(Vector3 raw)
    {
        return new Vector3(
            (raw.x - 0.5f) * 10f,
            (0.5f - raw.y) * 10f,
            0
        );
    }

    public void UpdatePoseData(
        Vector3 nosePos,
        Vector3 leftShoulderPos,
        Vector3 rightShoulderPos,
        Vector3 leftHipPos,
        Vector3 rightHipPos,
        Vector3 leftWristPos,
        Vector3 rightWristPos
    )
    {
        nose = ConvertToWorld(nosePos);

        leftShoulder = ConvertToWorld(leftShoulderPos);
        rightShoulder = ConvertToWorld(rightShoulderPos);

        leftHip = ConvertToWorld(leftHipPos);
        rightHip = ConvertToWorld(rightHipPos);

        leftWrist = ConvertToWorld(leftWristPos);
        rightWrist = ConvertToWorld(rightWristPos);
    }
}