using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public float score;
    public float maxScore;
    public TextMeshProUGUI scoreText;
    public Image scoreImage;

    public float bestScore;

    public int[] scoreGoals;

    public GoalsManager goals;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        maxScore = scoreGoals[2];

        if (PlayerPrefs.HasKey("Best Score"))
        {
            bestScore = PlayerPrefs.GetFloat("Best Score");
        }
        else
        {
            PlayerPrefs.SetFloat("Best Score", 0);
        }

        PlayerPrefs.Save();
    }
    private void Update()
    {
        scoreText.text = score.ToString();

        //if the current score is greater than the best score
        //set best score to current score
        if (score >= bestScore)
        {
            bestScore = score;
        }
    }
    public void IncreaseScore(int amountToIncrease)
    {
        //increase score
        score += amountToIncrease;

        //set player prefs
        PlayerPrefs.SetFloat("Best Score", score);
        PlayerPrefs.Save();

        //update the score image
        if (scoreImage != null)
        {
            scoreImage.fillAmount = score / maxScore;
        }

        for(int i = 0; i < scoreGoals.Length; i++)
        {
            if(score > scoreGoals[i])
            {
                goals.starsCollected = i + 1;
            }
        }
    }
}
