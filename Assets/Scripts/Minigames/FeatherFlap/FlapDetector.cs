using UnityEngine;

public class FlapDetector : MonoBehaviour
{
    public static FlapDetector Instance;

    [Header("Threshold")]
    public float flapThreshold = 0.25f;

    [Header("Cooldown")]
    public float flapCooldown = 0.4f;

    [Header("Debug")]
    public float leftVelocity;
    public float rightVelocity;

    public bool flapTriggered;
    bool initialized;

    Vector3 previousLeftWrist;
    Vector3 previousRightWrist;

    bool canFlap = true;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        DetectFlap();
    }

    void DetectFlap()
    {
        if(PoseManager.Instance == null)
            return;

        Vector3 currentLeft =
            PoseManager.Instance.leftWrist;

        Vector3 currentRight =
            PoseManager.Instance.rightWrist;

        // init pertama
        if(!initialized)
        {
            previousLeftWrist = currentLeft;
            previousRightWrist = currentRight;

            initialized = true;

            return;
        }

        leftVelocity =
            currentLeft.y - previousLeftWrist.y;

        rightVelocity =
            currentRight.y - previousRightWrist.y;

        bool leftFlap =
            leftVelocity < -flapThreshold;

        bool rightFlap =
            rightVelocity < -flapThreshold;

        if(canFlap &&
        leftFlap &&
        rightFlap)
        {
            TriggerFlap();
        }

        previousLeftWrist = currentLeft;
        previousRightWrist = currentRight;
    }

    void TriggerFlap()
    {
        flapTriggered = true;

        Debug.Log("FLAP");

        StartCoroutine(FlapRoutine());
    }

    System.Collections.IEnumerator FlapRoutine()
    {
        canFlap = false;

        yield return new WaitForSeconds(0.1f);

        flapTriggered = false;

        yield return new WaitForSeconds(
            flapCooldown
        );

        canFlap = true;
    }
}