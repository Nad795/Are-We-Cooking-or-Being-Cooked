using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ShellSpawner spawner;

    public CameraFollow cameraFollow;

    [Header("Current")]
    public Shell currentShell;

    Shell nextShell;

    [Header("Settings")]
    public Vector3 shellOffset =
        new Vector3(0, 1f, 0);

    bool canInput = true;

    bool inputLocked;

    void Start()
    {
        currentShell =
            spawner.currentShell;

        AttachToShell(currentShell);

        nextShell =
            spawner.SpawnNextShell();
    }

    void Update()
    {
        if(!canInput)
            return;

        if(!ShellJumpManager.Instance.gameRunning)
            return;

        ReadInput();
    }

    void ReadInput()
    {
        float direction = 0;

        if(
            BodyLeanDetector.Instance != null
        )
        {
            direction =
                BodyLeanDetector.Instance.moveDirection;
        }

        // reset lock kalau badan balik netral
        if(Mathf.Abs(direction) < 0.1f)
        {
            inputLocked = false;
            return;
        }

        // cegah spam input
        if(inputLocked)
            return;

        inputLocked = true;

        // kanan
        if(direction > 0)
        {
            AttemptMove(true);
        }

        // kiri
        else if(direction < 0)
        {
            AttemptMove(false);
        }
    }

    void AttemptMove(bool left)
    {
        canInput = false;

        if(nextShell.isLeft == left)
        {
            MoveToNextShell();
        }

        else
        {
            Debug.Log("Wrong Direction");

            Invoke(
                nameof(EnableInput),
                0.3f
            );
        }
    }

    void MoveToNextShell()
    {
        Shell oldShell =
            currentShell;

        currentShell =
            nextShell;

        // pindah parent dulu
        AttachToShell(currentShell);

        // camera naik
        cameraFollow.MoveToY(
            currentShell.transform.position.y
        );

        ShellJumpManager.Instance.AddScore(1);

        // spawn berikutnya
        nextShell =
            spawner.SpawnNextShell();

        // destroy shell lama
        Destroy(
            oldShell.gameObject,
            0.1f
        );

        EnableInput();
    }

    void EnableInput()
    {
        canInput = true;
    }

    void AttachToShell(Shell shell)
    {
        transform.SetParent(
            shell.transform
        );

        transform.localPosition =
            new Vector3(0, 1.5f, 0);

        transform.localScale =
            Vector3.one;

        Vector3 pos =
            transform.position;

        pos.z = -1;

        transform.position = pos;
    }
}