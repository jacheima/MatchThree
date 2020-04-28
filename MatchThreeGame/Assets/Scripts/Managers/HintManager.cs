using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    private Board board;
    public float hintDelay;
    public float hintDelaySeconds;
    public GameObject hintParticle;
    public GameObject currentHint;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;
    }

    // Update is called once per frame
    void Update()
    {
        hintDelaySeconds -= Time.deltaTime;

        if(hintDelaySeconds <= 0 && currentHint == null)
        {
            MarkHint();
            hintDelaySeconds = hintDelay;
        }
    }

    //First, I want to find all possible matches on board
    private List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();

        for (int i = 0; i < board.columns; i++)
        {
            for (int j = 0; j < board.rows; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (i < board.columns - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.right))
                        {
                            possibleMoves.Add(board.allDots[i, j]);
                        }
                    }

                    if (j < board.rows - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.up))
                        {
                            possibleMoves.Add(board.allDots[i, j]);
                        }
                    }
                }
            }
        }

        return possibleMoves;
    }

    //Pick one of those matches randomly
    GameObject PickARandomMove()
    {
        List<GameObject> possibles = new List<GameObject>();
        possibles = FindAllMatches();

        if(possibles.Count > 0)
        {
            int pieceToUse = Random.Range(0, possibles.Count);
            return possibles[pieceToUse];
        }

        return null;
    }

    //Create the on the chosen match
    private void MarkHint()
    {
        GameObject move = PickARandomMove();

        if(move != null)
        {
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
        }
    }

    //Destroy hint particle
    public void DestroyHint()
    {
        if (currentHint != null)
        {
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;
        }
    }
}
