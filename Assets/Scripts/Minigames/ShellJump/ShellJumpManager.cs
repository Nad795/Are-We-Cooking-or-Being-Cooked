using UnityEngine;
using TMPro;

public class ShellJumpManager : MonoBehaviour
{
    public Transform player;

    public TMP_Text scoreText;

    int score;

    void Update()
    {
        score =
            Mathf.FloorToInt(
                player.position.y
            );

        scoreText.text =
            "Score : " + score;
    }
}