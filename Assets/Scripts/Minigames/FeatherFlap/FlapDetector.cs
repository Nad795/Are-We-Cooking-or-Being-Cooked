using UnityEngine;

public class FlapDetector : MonoBehaviour
{
    public static FlapDetector Instance;

    [Header("Threshold")]
    public float flapThreshold = 0.15f;

    [Header("Cooldown")]
    public float flapCooldown = 0.4f;

    [Header("Debug")]
    public float leftVelocity;
    public float rightVelocity;

    public bool flapTriggered;

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
        Vector3 currentLeft =
            PoseManager.Instance.leftWrist;

        Vector3 currentRight =
            PoseManager.Instance.rightWrist;

        // hitung velocity
        leftVelocity =
            currentLeft.y - previousLeftWrist.y;

        rightVelocity =
            currentRight.y - previousRightWrist.y;

        // harus dua tangan bergerak cukup besar
        bool leftFlap =
            Mathf.Abs(leftVelocity)
            > flapThreshold;

        bool rightFlap =
            Mathf.Abs(rightVelocity)
            > flapThreshold;

        // trigger flap
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