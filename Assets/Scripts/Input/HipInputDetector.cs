using UnityEngine;

public class HipInputDetector : MonoBehaviour
{
    public static HipInputDetector Instance;

    [Header("Threshold")]
    public float moveThreshold = 0.08f;

    [Header("Debug")]
    public float currentHipX;
    public float neutralHipX;

    public bool moveLeft;
    public bool moveRight;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Calibrate();
    }

    void Update()
    {
        DetectHipMovement();
        if(moveLeft)
        {
            Debug.Log("LEFT");
        }

        if(moveRight)
        {
            Debug.Log("RIGHT");
        }
    }

    public void Calibrate()
    {
        Vector3 hipCenter = GetHipCenter();
        neutralHipX = hipCenter.x;
    }

    Vector3 GetHipCenter()
    {
        return
            (PoseManager.Instance.leftHip +
             PoseManager.Instance.rightHip) / 2f;
    }

    void DetectHipMovement()
    {
        Vector3 hipCenter = GetHipCenter();

        currentHipX = hipCenter.x;

        float delta =
            currentHipX - neutralHipX;

        moveLeft = delta < -moveThreshold;
        moveRight = delta > moveThreshold;
    }
}