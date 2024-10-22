using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private TMP_Text scoreText;

    private int score = 0;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        ScoreEventManager.ScoreIncrement += ScoreEventManager_ScoreIncrement;
    }
    private void OnDisable()
    {
        ScoreEventManager.ScoreIncrement -= ScoreEventManager_ScoreIncrement;
    }

    private void ScoreEventManager_ScoreIncrement()
    {
        score+=10;
        scoreText.text = "Score: " + score;
    }
}
