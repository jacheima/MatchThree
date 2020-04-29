using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Mission Pannel")]
    public GameObject missionPanel;
    
    [Header("Win Panel")]
    public GameObject winPanel;
    public Image[] starsPosition;
    public GameObject bigStarSprite;
    public GameObject starSprite;

    public GameObject losePanel;
    public GameObject pauseMenu;

    [Header("Managers")]
    public ScoreManager scoreManager;

    public Vector3 desiredScale;

    

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
