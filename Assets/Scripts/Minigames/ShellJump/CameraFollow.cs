using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 5f;

    float targetY;

    void Start()
    {
        targetY = transform.position.y;
    }

    void Update()
    {
        Vector3 targetPos =
            new Vector3(
                0,
                targetY,
                -10
            );

        transform.position =
            Vector3.Lerp(
                transform.position,
                targetPos,
                smoothSpeed * Time.deltaTime
            );
    }

    public void MoveToY(float y)
    {
        targetY = y;
    }
}