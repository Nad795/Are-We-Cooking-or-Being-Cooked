using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Jump")]
    public float jumpForce = 10f;

    [Header("Horizontal")]
    public float moveSpeed = 5f;

    void Update()
    {
        MoveHorizontal();
    }

    void MoveHorizontal()
    {
        float moveInput =
            Input.GetAxisRaw("Horizontal");

        // MediaPipe override
        if(
            BodyLeanDetector.Instance != null
        )
        {
            if(
                Mathf.Abs(
                    BodyLeanDetector.Instance.moveDirection
                ) > 0
            )
            {
                moveInput =
                    BodyLeanDetector.Instance.moveDirection;
            }
        }

        rb.velocity =
            new Vector2(
                moveInput * moveSpeed,
                rb.velocity.y
            );
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Shell"))
        {
            Bounce();
        }
    }

    void Bounce()
    {
        rb.velocity =
            new Vector2(
                rb.velocity.x,
                jumpForce
            );
    }
}