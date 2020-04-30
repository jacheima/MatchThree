using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameType
{
    Moves, Time
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue; //in seconds
}

public class EndGame : MonoBehaviour
{
    public Board board;

    public TextMeshProUGUI timerText;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private float timer = 1;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        SetUpRequirements();
        SetupGame();
    }

    private void Update()
    {
        if(requirements.gameType == GameType.Time)
        {
            if(board.currentState != GameState.pause)
            {
                timer -= Time.deltaTime;
            }

            if(timer <= 0)
            {
                CountDown();
                timer = 1;
            }
        }
    }
    void SetUpRequirements()
    {
        if (board.world != null)
        {
            if (board.world.levels[board.level] != null)
            {
                requirements = board.world.levels[board.level].endGameRequirements;
                currentCounterValue = requirements.counterValue;
            }
        }
    }

    void SetupGame()
    {
        if(requirements.gameType == GameType.Moves)
        {
            timerText.text = requirements.counterValue.ToString();
        }
        else
        {
            float minutes = Mathf.FloorToInt(currentCounterValue / 60);
            float seconds = Mathf.FloorToInt(currentCounterValue % 60);

            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            
        }
    }

    public void StartGame()
    {
        if(board.world.levels[board.level].isTutorial)
        {
            board.tutorial.StartTutorial();
        }
        else
        {
            StartCoroutine(StartGameCo());
        }
        
    }

    private IEnumerator StartGameCo()
    {
        yield return new WaitForSeconds(board.startDelay);
        board.currentState = GameState.move;
        timer = 1f;
    }

    public void CountDown()
    {
        
        if (currentCounterValue >= 1)
        {
            currentCounterValue--;

            if (requirements.gameType == GameType.Moves)
            {
                timerText.text = currentCounterValue.ToString();
            }
            else
            {
                float minutes = Mathf.FloorToInt(currentCounterValue / 60);
                float seconds = Mathf.FloorToInt(currentCounterValue % 60);

                timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
            } 
        }
        else
        {
            CheckWinOrLose();
        }
    }

    public void CheckWinOrLose()
    {
        if(currentCounterValue >= 1)
        {
            YouWin();
        }
        else
        {
            YouLose();
        }
    }

    private void YouWin()
    {
        board.currentState = GameState.win;
        board.uiManager.AddStars(board.uiManager.winPanel, board.goalsManager.starsCollected);
        board.uiManager.AddScore(board.uiManager.winPanel, board.scoreManager.score, board.scoreManager.bestScore);

        board.uiManager.winPanel.GetComponent<Win_Popup>().WinIsOpening();
    }

    private void YouLose()
    {
        board.currentState = GameState.lose;
        board.uiManager.AddScore(board.uiManager.losePanel, board.scoreManager.score, board.scoreManager.bestScore);
        board.uiManager.losePanel.GetComponent<Lose_Popup>().LoseIsOpening();
    }
}
