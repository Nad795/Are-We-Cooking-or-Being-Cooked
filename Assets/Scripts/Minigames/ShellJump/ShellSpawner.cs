using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    public GameObject shellPrefab;

    public Transform player;

    public int initialShellCount = 20;

    public float verticalSpacing = 2.5f;

    public float minX = -3f;
    public float maxX = 3f;

    float highestY;

    void Start()
    {
        SpawnInitialShells();
    }

    void Update()
    {
        SpawnMoreShells();
    }

    void SpawnInitialShells()
    {
        float currentY = 0;

        for(int i = 0; i < initialShellCount; i++)
        {
            SpawnShell(currentY);

            currentY += verticalSpacing;
        }

        highestY = currentY;
    }

    void SpawnMoreShells()
    {
        if(player.position.y + 20f > highestY)
        {
            SpawnShell(highestY);

            highestY += verticalSpacing;
        }
    }

    void SpawnShell(float yPos)
    {
        float randomX =
            Random.Range(minX, maxX);

        Vector3 spawnPos =
            new Vector3(
                randomX,
                yPos,
                0
            );

        Instantiate(
            shellPrefab,
            spawnPos,
            Quaternion.identity
        );
    }
}