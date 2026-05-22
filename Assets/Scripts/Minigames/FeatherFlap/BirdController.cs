using UnityEngine;

public class BirdController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Flap")]
    public float flapForce = 8f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(FlapDetector.Instance == null)
            return;

        if(FlapDetector.Instance.flapTriggered)
        {
            Flap();
        }
    }

    void Flap()
    {
        rb.velocity =
            new Vector2(
                rb.velocity.x,
                flapForce
            );
    }
}