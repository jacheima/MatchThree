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
    public UIManager uiManager;
    public ScoreManager scoreManager;



    private void Start()
    {
        uiManager = GetComponent<UIManager>();
        scoreManager = GetComponent<ScoreManager>();

        SetupMissionPanel();
        SetupLevelGoal();


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
                info.goalSprite = missionGoals[i].goalSprite;
                info.goalString = missionGoals[i].goalText;

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
                info.goalSprite = levelGoals[i].goalSprite;
                info.goalString = levelGoals[i].numberCollected.ToString();

                //add to a list of active level goals
                activeLevelGoals.Add(info);
            }

            Debug.Log(levelGoals.Length);

            
        }
    }

    void UpdateGoals()
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            if ((levelGoals[i].numberNeeded - levelGoals[i].numberCollected) > 0)
            {
                activeLevelGoals[i].goalText.text = (levelGoals[i].numberNeeded - levelGoals[i].numberCollected).ToString();
            }
            else
            {
                activeLevelGoals[i].goalText.text = 0.ToString();
            }
        }
    }

    public void CompareGoal(string tagText)
    {
        if (levelGoals != null)
        {
            for (int i = 0; i < levelGoals.Length; i++)
            {
                if(!levelGoals[i].isCompleted)
                {
                    if (tagText == levelGoals[i].matchValue)
                    {
                        if (levelGoals[i].numberCollected < levelGoals[i].numberNeeded)
                        {
                            levelGoals[i].numberCollected++;
                        }
                        else
                        {
                            levelGoals[i].isCompleted = true;
                            goalsCompleted++;
                        }
                    }
                }
                else
                {
                    if (goalsCompleted >= levelGoals.Length)
                    {
                        YouWin();
                    }
                }
            }
        }
    }

    public void YouWin()
    {
        Debug.Log("You Win");

        uiManager.AddStars(uiManager.winPanel, starsCollected);
        uiManager.AddScore(uiManager.winPanel, scoreManager.score, scoreManager.bestScore);

        uiManager.winPanel.GetComponent<Win_Popup>().WinIsOpening();
    }

}
