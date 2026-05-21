using UnityEngine;

public class BirdController : MonoBehaviour
{
    Rigidbody2D rb;

    public float flapForce = 5f;

    bool previousFlapState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        bool currentFlap =
            FlapDetector.Instance.flapTriggered;

        if(currentFlap && !previousFlapState)
        {
            Flap();
        }

        previousFlapState =
            currentFlap;
    }

    void Flap()
    {
        Debug.Log("BURUNG FLAP");

        rb.WakeUp();

        rb.velocity =
            new Vector2(
                rb.velocity.x,
                0
            );

        rb.AddForce(
            Vector2.up * flapForce,
            ForceMode2D.Impulse
        );
    }
}