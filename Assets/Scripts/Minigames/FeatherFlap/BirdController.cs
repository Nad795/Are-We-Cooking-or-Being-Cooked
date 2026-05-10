using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Rigidbody2D rb;

    public float flapForce = 5f;

    bool previousFlapState;

    void Update()
    {
        bool currentFlap =
            FlapDetector.Instance.flapTriggered;

        if(currentFlap && !previousFlapState)
        {
            Flap();
        }

        previousFlapState = currentFlap;
    }

    void Flap()
    {
        rb.velocity = Vector2.zero;

        rb.AddForce(
            Vector2.up * flapForce,
            ForceMode2D.Impulse
        );
    }
}