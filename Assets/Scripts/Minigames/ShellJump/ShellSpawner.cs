using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    public GameObject shellPrefab;

    public Transform shellParent;

    [Header("Spawn")]
    public float verticalSpacing = 2.5f;

    public float leftX = -2.5f;
    public float rightX = 2.5f;

    public Shell currentShell;

    bool lastSpawnLeft;

    float nextY;

    void Start()
    {
        SpawnFirstShell();
    }

    void SpawnFirstShell()
    {
        bool left =
            Random.value > 0.5f;

        lastSpawnLeft = left;

        currentShell =
            SpawnShell(left, 0);

        nextY += verticalSpacing;
    }

    public Shell SpawnNextShell()
    {
        bool left =
            GenerateSide();

        currentShell =
            SpawnShell(left, nextY);

        nextY += verticalSpacing;

        return currentShell;
    }

    Shell SpawnShell(bool isLeft, float y)
    {
        float x =
            isLeft ? leftX : rightX;

        GameObject obj =
            Instantiate(
                shellPrefab,
                new Vector3(x, y, 0),
                Quaternion.identity,
                shellParent
            );

        Shell shell =
            obj.GetComponent<Shell>();

        shell.isLeft = isLeft;

        return shell;
    }

    bool GenerateSide()
    {
        lastSpawnLeft = !lastSpawnLeft;

        return lastSpawnLeft;
    }
}