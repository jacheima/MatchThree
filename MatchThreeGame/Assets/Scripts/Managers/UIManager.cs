using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Board board;

    [Header("Mission Pannel")]
    public GameObject missionPanel;
    
    [Header("Win Panel")]
    public GameObject winPanel;
    public Image[] starsPosition;
    
    [Header("Lose Panel")]
    public GameObject losePanel;

    [Header("Pause Panel")]
    public GameObject pauseMenu;

    [Header("Tutorial Panels")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialTextText;
    public Animator tutorialAnimator;

    [Header("Stars")]
    public GameObject bigStarSprite;
    public GameObject starSprite;

    [Header("Managers")]
    public ScoreManager scoreManager;

    [Header("Transform Attributes")]
    public Vector3 desiredScale;

    private void Start()
    {
        board = FindObjectOfType<Board>();

        //get the animator component of the tutorial panel;
        tutorialAnimator = tutorialPanel.GetComponent<Animator>();
    }




    public void AddStars(GameObject panel, int numberofStars)
    {
        Star[] stars = panel.GetComponentsInChildren<Star>();

        for(int i = 0; i < numberofStars; i++)
        {
            if (i == 0)
            {
                GameObject bigStar = Instantiate(bigStarSprite, stars[i].transform.position, Quaternion.identity);
                bigStar.transform.parent = starsPosition[i].transform;

                bigStar.transform.localScale = desiredScale;
            }
            else
            {
                GameObject star = Instantiate(starSprite, stars[i].transform.position, Quaternion.identity);
                star.transform.parent = starsPosition[i].transform;

                star.transform.localScale = desiredScale;
            }
        }
    }

    public void AddScore(GameObject panel, float score, float bestScore)
    {
        if (panel != null)
        {
            ScoreText scoreText = panel.GetComponentInChildren<ScoreText>();
            TextMeshProUGUI text = scoreText.gameObject.GetComponent<TextMeshProUGUI>();

            text.text = score.ToString();

            if (panel.GetComponent<BestScoreText>())
            {
                BestScoreText bestScoreText = panel.GetComponentInChildren<BestScoreText>();
                TextMeshProUGUI bestText = bestScoreText.gameObject.GetComponent<TextMeshProUGUI>();

                bestText.text = bestScore.ToString();
            } 
        }
    }
}
