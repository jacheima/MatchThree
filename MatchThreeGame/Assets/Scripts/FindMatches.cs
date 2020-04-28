using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    private AudioManager audioManager;
    public List<GameObject> currentMatches = new List<GameObject>(); //change to private after debug

    private void Start()
    {
        //find and set the board reference
        board = FindObjectOfType<Board>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    /// <summary>
    /// This method starts the find all matches coroutine
    /// </summary>
    public void FindAllMatches()
    {
        //start coroutine
        StartCoroutine(FindAllMatchesCo());
    }

    /// <summary>
    /// This coroutine finds all the matches on the board
    /// </summary>
    /// <returns></returns>
    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < board.columns; i++)
        {
            for (int j = 0; j < board.rows; j++)
            {
                //set the current dot to the do we are on
                GameObject currentDot = board.allDots[i, j];

                //if the current dot is not empty
                if (currentDot != null)
                {
                    //get the current dots dot component
                    Dot currentDotDot = currentDot.GetComponent<Dot>();

                    //if the column is greater than 0 and less than the width of the board
                    if (i > 0 && i < board.columns - 1)
                    {
                        //get the dot to the left and right of our current dot
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];

                        //if the left and right dot are not empty
                        if (leftDot != null && rightDot != null)
                        {
                            //Get the do components from the left and right dot
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();

                            //if the tags of the left and right dot both equal the tag of our current dot
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                //check for row bomb
                                currentMatches.Union(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));

                                //check for column bomb
                                currentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));

                                //check for adjacent bomb
                                currentMatches.Union(IsAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));


                                GetNearbyPieces(leftDot, currentDot, rightDot);
                            }
                        }
                    }

                    //if the current row is greater than 0 and less than or equal to the number of rows on the board
                    if (j > 0 && j < board.rows - 1)
                    {
                        //get the dot above and below our current dot
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];

                        //if the dot above and below are not empty
                        if (upDot != null && downDot != null)
                        {
                            //get the dot component of the dots above and below our current dot
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();

                            //if the tags of the dot above and below our current dot are the same as the current dot
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                //check for column bomb
                                currentMatches.Union(IsColumnBomb(upDotDot, currentDotDot, downDotDot));

                                //check for row bomb
                                currentMatches.Union(IsRowBomb(upDotDot, currentDotDot, downDotDot));

                                //check for adjacent bom
                                currentMatches.Union(IsAdjacentBomb(upDotDot, currentDotDot, downDotDot));

                                //find the nearby pieces
                                GetNearbyPieces(upDot, currentDot, downDot);

                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks the pieces adjacent to the current dot and marks them matched
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public List<GameObject> GetAdjacentPieces(int col, int row)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = col - 1; i <= col + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                //check if the peice is inside the baord
                if ((i >= 0 && i < board.columns) && (j >= 0 && j < board.rows))
                {
                    if(board.allDots[i, j] != null)
                    {
                        //add the matched dot to the dot list
                        dots.Add(board.allDots[i, j]);

                        //set the dots to be matched
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                        audioManager.PlayUseBomb();
                    }
                }
            }
        }

        return dots;
    }

    /// <summary>
    /// Returns a list of adjacent bombs that are adjacent to the current piece
    /// </summary>
    /// <param name="dot1"></param>
    /// <param name="dot2"></param>
    /// <param name="dot3"></param>
    /// <returns></returns>
    public List<GameObject> IsAdjacentBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }
        if (dot2.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }
        if (dot3.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot3.column, dot3.row));
        }

        return currentDots;

    }
    /// <summary>
    /// Returns a list of matched bombd in the row of the current piece that are row bombs 
    /// </summary>
    /// <param name="dot1"></param>
    /// <param name="dot2"></param>
    /// <param name="dot3"></param>
    /// <returns></returns>

    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.row));
        }

        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.row));
        }

        if (dot3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.row));
        }

        return currentDots;
    }
    /// <summary>
    /// Returns a list of matched bombd in the column of the current piece that are column bombs
    /// </summary>
    /// <param name="dot1"></param>
    /// <param name="dot2"></param>
    /// <param name="dot3"></param>
    /// <returns></returns>

    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot1.column));
        }

        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot2.column));
        }

        if (dot3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot3.column));
        }

        return currentDots;
    }

    /// <summary>
    /// Adds the dot to the list of matched dots 
    /// </summary>
    /// <param name="dot"></param>
    private void AddToListAndMatch(GameObject dot)
    {
        //if current matches doesn't already contain the dot
        if (!currentMatches.Contains(dot))
        {
            //add the dot to the current matches
            currentMatches.Add(dot);
        }

        //set the dot is matched to be true
        dot.GetComponent<Dot>().isMatched = true;
    }

    /// <summary>
    /// Helper method that uses "AddToListAndMatch" to check the dots passed into it
    /// </summary>
    /// <param name="dot1"></param>
    /// <param name="dot2"></param>
    /// <param name="dot3"></param>
    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        //add the dots to the current matches list
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    /// <summary>
    /// Checks to see if the dot is the same color as the dot it's trying
    /// to be matched with
    /// </summary>
    /// <param name="color"></param>
    public void MatchPiecesOfColor(string color)
    {
        for (int i = 0; i < board.columns; i++)
        {
            for (int j = 0; j < board.rows; j++)
            {
                //check to see if the piece exists
                if (board.allDots[i, j] != null)
                {
                    //check the tag for color
                    if (board.allDots[i, j].CompareTag(color))
                    {
                        //set dot to be matched
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                        audioManager.PlayUseBomb();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets all the peices in a given column when a column bomb is used
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    private List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.rows; i++)
        {
            if (board.allDots[column, i] != null)
            {
                Dot dot = board.allDots[column, i].GetComponent<Dot>();

                if(dot.isRowBomb)
                {
                    dots.Union(GetRowPieces(i).ToList());
                }

                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<Dot>().isMatched = true;
                audioManager.PlayUseBomb();
            }
        }

        return dots;
    }

    /// <summary>
    /// Gets all the pieces in a given row when a row bomb is used
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.columns; i++)
        {
            if (board.allDots[i, row] != null)
            {
                Dot dot = board.allDots[i, row].GetComponent<Dot>();

                if (dot.isColumnBomb)
                {
                    dots.Union(GetColumnPieces(i).ToList());
                }

                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
                audioManager.PlayUseBomb();
            }
        }

        return dots;
    }

    /// <summary>
    /// Determines which type of bombs should be created
    /// </summary>
    public void CheckBombs()
    {
        //did the player move anything?
        if (board.currentDot != null)
        {
            //is the piece we moved currently matched?
            if (board.currentDot.isMatched)
            {
                //make it unmatched
                board.currentDot.isMatched = false;

                //decide which type of bomb we want to make 

                if (currentMatches != null)
                {
                    for (int i = 0; i < currentMatches.Count; i++)
                    {
                        if (currentMatches[i].GetComponent<Dot>().row < board.currentDot.row ||
                            currentMatches[i].GetComponent<Dot>().row > board.currentDot.row)
                        {
                            board.currentDot.MakeColumnBomb();
                        }
                        else if (currentMatches[i].GetComponent<Dot>().column < board.currentDot.column ||
                            currentMatches[i].GetComponent<Dot>().column > board.currentDot.column)
                        {
                            board.currentDot.MakeRowBomb();
                        }
                    }
                }
            }
            //is the piece we swaped with matched?
            else if (board.currentDot.otherDot != null)
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();

                if (otherDot.isMatched)
                {
                    //make it unmatched
                    otherDot.isMatched = false;

                    //decide which type of bomb we want to make
                    for (int i = 0; i < currentMatches.Count; i++)
                    {
                        if (currentMatches[i].GetComponent<Dot>().row < otherDot.row ||
                            currentMatches[i].GetComponent<Dot>().row > otherDot.row)
                        {
                            otherDot.MakeColumnBomb();
                        }
                        else if (currentMatches[i].GetComponent<Dot>().column < otherDot.column ||
                            currentMatches[i].GetComponent<Dot>().column > otherDot.column)
                        {
                            otherDot.MakeRowBomb();
                        }
                    }
                }
            }
        }
    }
}
