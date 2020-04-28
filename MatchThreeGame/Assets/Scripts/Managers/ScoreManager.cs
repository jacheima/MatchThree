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

    public int[] scoreGoals;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        maxScore = scoreGoals[2];
    }
    private void Update()
    {
        scoreText.text = score.ToString();

    }
    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        if(board != null)
        {
            if(scoreImage != null)
            {
                scoreImage.fillAmount = score / maxScore;
            }
        }
    }
}
