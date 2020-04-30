using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;

    public Sprite goalSprite;

    public string matchValue;
    public string goalText;

    public bool isCompleted = false;

}

public class GoalsManager : MonoBehaviour
{
    [Header("Goal Properties")]
    public int goalsCompleted;

    public int starsCollected;
    public Vector3 prefabScale;

    [Header("All Goals")]
    public BlankGoal[] levelGoals;
    public BlankGoal[] missionGoals;

    [Header("Mission Panel")]

    public GameObject missionPrefab;
    public GameObject goalsContainer;
    public List<GoalInfo> activeMissionGoals;

    [Header("In Level Goals")]
    public GameObject levelGoalsContainer;
    public GameObject levelPrefab;
    public List<GoalInfo> activeLevelGoals;


    [Header("Managers")]
    public Board board;
    public UIManager uiManager;
    public ScoreManager scoreManager;



    private void Start()
    {
        uiManager = GetComponent<UIManager>();
        scoreManager = GetComponent<ScoreManager>();
        board = FindObjectOfType<Board>();
        GetGoals();
        SetupMissionPanel();
        SetupLevelGoal();


    }

    private void GetGoals()
    {
        if (board != null)
        {
            if (board.world != null)
            {
                if (board.world.levels != null)
                {
                    levelGoals = board.world.levels[board.level].levelGoals;
                    missionGoals = board.world.levels[board.level].missionGoals;
                }
            }
        }
    }

    private void LateUpdate()
    {
        UpdateGoals();
    }

    private void SetupMissionPanel()
    {
        if (missionGoals != null)
        {
            for (int i = 0; i < missionGoals.Length; i++)
            {
                //create a new goal prefab on the mission panel & parent it to the goals container
                GameObject levelGoal = Instantiate(missionPrefab, goalsContainer.transform.position, Quaternion.identity);
                levelGoal.transform.parent = goalsContainer.transform;

                //set the desired scale
                levelGoal.transform.localScale = prefabScale;

                //populate the goal with the correct sprite & text
                GoalInfo info = levelGoal.GetComponent<GoalInfo>();
                info.goalSprite = board.world.levels[board.level].missionGoals[i].goalSprite;
                info.goalString = board.world.levels[board.level].missionGoals[i].goalText;

                //add the mission goal info to a list of active mission goal 
                activeMissionGoals.Add(info);
            }
        }
    }

    private void SetupLevelGoal()
    {
        if (levelGoals != null)
        {
            for (int i = 0; i < levelGoals.Length; i++)
            {
                //create a new goal prefab on the level goal panel & parent it to the level goal container
                GameObject levelGoal = Instantiate(levelPrefab, levelGoalsContainer.transform.position, Quaternion.identity);
                levelGoal.transform.parent = levelGoalsContainer.transform;

                //set desired scale
                levelGoal.transform.localScale = prefabScale;

                //populate the goal with the correct sprite & text
                GoalInfo info = levelGoal.GetComponent<GoalInfo>();
                info.goalSprite = board.world.levels[board.level].levelGoals[i].goalSprite;
                info.goalString = board.world.levels[board.level].levelGoals[i].numberCollected.ToString();

                //add to a list of active level goals
                activeLevelGoals.Add(info);
            }
        }
    }

    void UpdateGoals()
    {
        for (int i = 0; i < board.world.levels[board.level].levelGoals.Length; i++)
        {
            if ((board.world.levels[board.level].levelGoals[i].numberNeeded - board.world.levels[board.level].levelGoals[i].numberCollected) > 0)
            {
                activeLevelGoals[i].goalText.text = (board.world.levels[board.level].levelGoals[i].numberNeeded - board.world.levels[board.level].levelGoals[i].numberCollected).ToString();
            }
            else
            {
                activeLevelGoals[i].goalText.text = 0.ToString();
            }

            if (goalsCompleted == board.world.levels[board.level].levelGoals.Length)
            {
                board.endGame.CheckWinOrLose();
            }
        }
    }

    public void CompareGoal(string tagText)
    {
        if (board.world.levels[board.level].levelGoals != null)
        {

            for (int i = 0; i < levelGoals.Length; i++)
            {
                if (!board.world.levels[board.level].levelGoals[i].isCompleted)
                {
                    Debug.Log("This goal is not yet complete!");
                    if (tagText == board.world.levels[board.level].levelGoals[i].matchValue)
                    {
                        Debug.Log("Collected Some Blues");

                        board.world.levels[board.level].levelGoals[i].numberCollected++;
                        if (IsGoalCompleted(i))
                        {
                            Debug.Log(IsGoalCompleted(i));
                            board.world.levels[board.level].levelGoals[i].isCompleted = true;
                            goalsCompleted++;
                        }
                    }
                }
            }
        }
    }

    private bool IsGoalCompleted(int levelgoal)
    {
        if (board.world.levels[board.level].levelGoals[levelgoal].numberCollected >= board.world.levels[board.level].levelGoals[levelgoal].numberNeeded)
        {
            return true;
        }

        return false;
    }



}
