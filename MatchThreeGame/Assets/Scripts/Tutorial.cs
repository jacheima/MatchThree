using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Steps
{
    public string instruction;
    public HighlightPieces[] piecesToHighlight;
    public HighlightGoals[] goalsToHighlight;
    public bool isComplete;
}

[System.Serializable]
public class DotConfiguration
{
    public GameObject[] dotOrder;


}

[System.Serializable]
public class HighlightGoals
{
    public GameObject indicator;
    public Vector3 highlightScale;
}

[System.Serializable]
public class HighlightPieces
{
    public int column;
    public int row;
    public GameObject indicator;
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
    public List<Steps> completedSteps;

    public float tutorialDelay;
    public float goalDelay;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        goalsIndicatorTimerDelay = goalDelay;

        currentStep = 0;

        SetupTutorial();

    }
    private void Update()
    {
        goalsIndicatorTimeDelaySeconds -= Time.deltaTime;

        if (indicatorObject != null)
        {
            if (goalsIndicatorTimeDelaySeconds <= 0)
            {
                DestroyHighlights();
                goalsIndicatorTimeDelaySeconds = goalsIndicatorTimerDelay;
            }
        }

        if (completedSteps.Count == steps.Length)
        {
            board.isTutorialLevel = false;
        }
    }

    private void SetupTutorial()
    {

        steps = board.world.levels[board.level].steps;
        dotConfig = board.world.levels[board.level].dotConfiguration;

    }

    public void StartTutorial()
    {
        StartCoroutine(StartTutorialCo());
        

    }

    public IEnumerator StartTutorialCo()
    {
        yield return new WaitForSeconds(tutorialDelay);
        board.currentState = GameState.move;
        //turn on the tutorial panel
        //TODO: Change this to animation
        board.uiManager.tutorialPanel.SetActive(true);

        if (!steps[currentStep].isComplete)
        {
            board.uiManager.tutorialTextText.text = steps[currentStep].instruction;

            if (board.world.levels[board.level].steps[currentStep].piecesToHighlight != null)
            {
                HighlightPieces();
            }

            if (board.world.levels[board.level].steps[currentStep].goalsToHighlight != null)
            {
                HighlightGoals();
            }
        }
        else
        {
            if(currentStep >= board.world.levels[board.level].steps.Length)
            {
                board.isTutorialLevel = false;
            }
        }

    }

    public void CloseTutorialPanel()
    {
        board.uiManager.tutorialPanel.SetActive(false);
        board.world.levels[board.level].isTutorial = false;
        board.endGame.StartGame();
    }
    void HighlightGoals()
    {
        if (board != null)
        {
            if (board.world != null)
            {
                if (board.world.levels != null)
                {
                    //has this step already been completed?
                    if (!board.world.levels[board.level].steps[currentStep].isComplete)
                    {
                        //do we need to highlight any board pieces?
                        if (board.world.levels[board.level].steps[currentStep].piecesToHighlight != null)
                        {
                            highlightsOnBoard.Clear();

                            // do we need to highlight any goals?
                            if (board.world.levels[board.level].steps[currentStep].goalsToHighlight != null)
                            {
                                highlightsOnBoard.Clear();

                                for (int i = 0; i < board.world.levels[board.level].steps[currentStep].goalsToHighlight.Length; i++)
                                {
                                    //get highlight prefab
                                    Image screenOverlay = board.world.levels[board.level].steps[currentStep].goalsToHighlight[i].indicator.GetComponent<Image>();

                                    //get canvas you want to put it on
                                    GameObject holder = GameObject.Find("Highlight_Holder");

                                    //put the highlight on the screen
                                    GameObject overlay = Instantiate(screenOverlay.gameObject, holder.transform.position, Quaternion.identity);
                                    
                                    overlay.transform.SetParent(holder.transform);

                                    overlay.transform.localScale = board.world.levels[board.level].steps[currentStep].goalsToHighlight[i].highlightScale;


                                    screenOverlay.rectTransform.position = new Vector3(0f, 0f, 0f);


                                    highlightsOnBoard.Add(screenOverlay.gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void HighlightPieces()
    {
        if (board != null)
        {
            if (board.world != null)
            {
                if (board.world.levels != null)
                {
                    //has this step already been completed?
                    if (!board.world.levels[board.level].steps[currentStep].isComplete)
                    {
                        //do we need to highlight any board pieces?
                        if (board.world.levels[board.level].steps[currentStep].piecesToHighlight != null)
                        {
                            highlightsOnBoard.Clear();

                            //for each piece we need to highlight
                            for (int i = 0; i < board.world.levels[board.level].steps[currentStep].piecesToHighlight.Length; i++)
                            {
                                //get the location of the baord that needs to be highlighted
                                int column = board.world.levels[board.level].steps[currentStep].piecesToHighlight[i].column;
                                int row = board.world.levels[board.level].steps[currentStep].piecesToHighlight[i].row;

                                //get the highlight prefab
                                GameObject highlightPrefab = board.world.levels[board.level].steps[currentStep].piecesToHighlight[i].indicator;

                                //Put the highlight on the baord
                                GameObject highlight = Instantiate(highlightPrefab, board.backgroundTiles[column, row].transform.position, Quaternion.identity);
                                highlight.transform.parent = board.backgroundTiles[column, row].transform;
                                highlight.transform.localScale = board.world.levels[board.level].steps[currentStep].piecesToHighlight[i].highlightScale;

                                highlightsOnBoard.Add(highlight);
                            }
                        }
                    }
                }
            }
        }
    }

    public void DestroyHighlights()
    {
        for (int i = 0; i < highlightsOnBoard.Count; i++)
        {
            Destroy(highlightsOnBoard[i]);
            highlightsOnBoard[i] = null;
        }
    }

    public void NextStep()
    {
        CloseTutorialPanel();

        if (currentStep < board.world.levels[board.level].steps.Length - 1)
        {
            board.isTutorialLevel = true;
            board.world.levels[board.level].steps[currentStep].isComplete = true;
            completedSteps.Add(board.world.levels[board.level].steps[currentStep]);
            currentStep++;

            StartTutorial();
        }
        else
        {
            board.isTutorialLevel = false;
        }

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
