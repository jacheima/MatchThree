using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    private int lives;
    private int score;

    [Header("Score Properties")]
    [SerializeField] private Image scoreImage;

    [Header("Lives UI")]
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Level Stats")]
    [SerializeField] private TextMeshProUGUI levelText;
    private int level;

    int amountNeeded;


    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        score = 0;
        amountNeeded = 100;
    }

    // Update is called once per frame
    void Update()
    {
        scoreImage.fillAmount = (float)score / amountNeeded;
    }
}
