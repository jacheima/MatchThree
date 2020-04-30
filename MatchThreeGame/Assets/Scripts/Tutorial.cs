using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Steps
{
    public string instruction;
    public GameObject indicator;
}

[System.Serializable]
public class DotConfiguration
{
    public GameObject[] dotOrder;
    public HighlightPieces[] piecesToHighlight;
    public HighlightGoals[] goalsToHighlight;

}

[System.Serializable]
public class HighlightGoals
{
    public Vector3 highlightScale;
}

[System.Serializable]
public class HighlightPieces
{
    public int column;
    public int row;
    public Vector3 highlightScale;
}


public class Tutorial : MonoBehaviour
{
    public Board board;
    public Steps[] steps;
    public int currentStep;
    public DotConfiguration dotConfig;

    public GameObject indicatorObject;

    public float goalsIndicatorTimerDelay;
    public float goalsIndicatorTimeDelaySeconds;

    public List<GameObject> highlightsOnBoard;

    public float tutorialDelay;
    public float goalDelay;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        currentStep = 0;
        goalsIndicatorTimerDelay = goalDelay;

        SetupTutorial();

    }
    private void Update()
    {
        goalsIndicatorTimeDelaySeconds -= Time.deltaTime;

        if(indicatorObject != null)
        {
           if(goalsIndicatorTimeDelaySeconds <= 0)
            {
                DestroyHighlights();
                goalsIndicatorTimeDelaySeconds = goalsIndicatorTimerDelay;
            }
        }

        if (currentStep == steps.Length)
        {
            board.isTutorialLevel = false;
        }
    }

    private void SetupTutorial()
    {

        steps = board.world.levels[board.level].steps;
        dotConfig = board.world.levels[board.level].dotConfiguration;
        indicatorObject = steps[currentStep].indicator;

    }

    public void StartTutorial()
    {
        StartCoroutine(StartTutorialCo());
        
    }

    public IEnumerator StartTutorialCo()
    {
        yield return new WaitForSeconds(tutorialDelay);

        //turn on the tutorial panel
        //TODO: Change this to animation
        board.uiManager.tutorialPanel.SetActive(true);

        Debug.Log(currentStep);

        //set the instructions
        board.uiManager.tutorialTextText.text = steps[currentStep].instruction;
        indicatorObject = steps[currentStep].indicator;

        if(currentStep + 1 < steps.Length)
        {
            currentStep++;
        }

        HighlightPieces();

    }

    public void CloseTutorialPanel()
    {
        board.uiManager.tutorialPanel.SetActive(false);
        board.world.levels[board.level].isTutorial = false;
        board.endGame.StartGame();
    }

    void HighlightPieces()
    {
        //do we need to highlight board pieces?

        //if dotConfig is not empty
        if(dotConfig != null)
        {
            //do we need to highlight any pieces on the baord
            if(dotConfig.piecesToHighlight != null)
            {
                //foreach piece we need to highligh
                for (int i = 0; i <dotConfig.piecesToHighlight.Length; i++)
                {
                    //instantiate the highlight indicator on the 
                    GameObject highLight = Instantiate(indicatorObject, board.backgroundTiles[dotConfig.piecesToHighlight[i].column, dotConfig.piecesToHighlight[i].row].transform.position, Quaternion.identity);
                    highLight.transform.parent = board.backgroundTiles[dotConfig.piecesToHighlight[i].column, dotConfig.piecesToHighlight[i].row].transform;

                    highLight.transform.localScale = dotConfig.piecesToHighlight[i].highlightScale;

                    highlightsOnBoard.Add(highLight);
                }
            }
            
            //do we need to highlight any goals?
            if(dotConfig.goalsToHighlight != null)
            {
                for(int i = 0; i < dotConfig.goalsToHighlight.Length; i++)
                {
                    //instantiate the highlight indicator on the 
                    GameObject levelGoals = GameObject.Find("LevelGoals");
                    GameObject highLight = Instantiate(indicatorObject, levelGoals.transform.position, Quaternion.identity);
                    highLight.transform.parent = levelGoals.transform;

                    highLight.transform.localScale = dotConfig.goalsToHighlight[i].highlightScale;

                    highlightsOnBoard.Add(highLight);

                    goalsIndicatorTimerDelay = goalDelay;
                }
            }
        } 
        
    }

    public void DestroyHighlights()
    {
        for(int i = 0; i < highlightsOnBoard.Count; i++)
        {
            Destroy(highlightsOnBoard[i]);
            highlightsOnBoard[i] = null;
        }
    }

    public void NextStep()
    {
        StartTutorial();

    }

    public bool IsTutoral()
    {
        if (board != null)
        {
            if (board.world != null)
            {
                if (board.world.levels != null)
                {
                    if (board.world.levels[board.level].isTutorial)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return false;
    }
}
