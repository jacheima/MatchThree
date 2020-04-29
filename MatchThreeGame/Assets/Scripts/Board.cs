using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Board : MonoBehaviour
{
    

    [Header("Background Tile Attributes")]
    public int columns; //width of board
    public int rows; //height of board
    private bool[,] blankSpaces;
    [SerializeField] private float refillOffset;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private GameObject tilePrefab;
    public float tileWidth;
    public float tileHeight;



    [Header("Dot Tile Attrubutes")]
    [SerializeField] private GameObject[] dots;
    public GameObject[,] allDots;

    public int maxIterations;

    public GameObject destroyEffect;

    public Dot currentDot;
    public GameState currentState = GameState.move;


    public TileType[] boardLayout;
    private BackgroundTile[,] breakableTiles;
    public GameObject breakableTilePrefab;


    
    private int basePieceValue = 20;
    private int streakValue = 1;

    private float refillDelay = 0.5f;

    

    [Header("Managers")]
    private FindMatches findMatches;
    private ScoreManager scoreManager;
    private AudioManager audioManager;
    private GoalsManager goalsManager;


    public enum GameState
    {
        wait, move
    }

    public enum TypeOfTile
    {
        Breakable, Blank, Normal
    }


    [System.Serializable]
    public class TileType
    {
        public int x;
        public int y;
        public TypeOfTile typeOfTile;
    }


    private void Start()
    {
        blankSpaces = new bool[columns, rows];
        allDots = new GameObject[columns, rows];
        findMatches = FindObjectOfType<FindMatches>();
        currentState = GameState.move;
        breakableTiles = new BackgroundTile[columns, rows];
        scoreManager = FindObjectOfType<ScoreManager>();
        audioManager = FindObjectOfType<AudioManager>();
        goalsManager = FindObjectOfType<GoalsManager>();
        SetUp();
    }

    public void GenerateBlankSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].typeOfTile == TypeOfTile.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    public void GenerateBreakableTiles()
    {
        //look at all tiles in the board layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if the tile is a breakable tile
            if (boardLayout[i].typeOfTile == TypeOfTile.Breakable)
            {
                //create a breakable tile in that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x * tileWidth, boardLayout[i].y * tileHeight);
                GameObject breakableTile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = breakableTile.GetComponent<BackgroundTile>();
            }
        }
    }
    private void SetUp()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (!blankSpaces[i, j])
                {
                    //-----------------------Laying Out Background Tiles-------------------------
                    Vector2 tempPostion = new Vector2((i + xOffset) * tileWidth, (j + yOffset)* tileHeight);
                    //instatiate a new tile
                    GameObject tile = Instantiate(tilePrefab, tempPostion, Quaternion.identity);

                    //Parent all the tiles to the board game object in the hierarchy
                    tile.transform.parent = this.transform;

                    //name the tile based off its column, row
                    tile.name = "(" + i + ", " + j + ")";


                    //------------------------Laying out Dots on Background Tiles-------------------------
                    //choose a random dot to use
                    int dotToUse = Random.Range(0, dots.Length);

                    int maxIterations = 0;

                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                    }

                    maxIterations = 0;

                    //instante the chosen dot on the background tile
                    GameObject dot = Instantiate(dots[dotToUse], tempPostion, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;
                    dot.transform.parent = GameObject.Find("Dots").transform;

                    //name the dot
                    dot.name = "Dot" + tile.name;

                    allDots[i, j] = dot;
                }
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if(piece != null)
        {
            if (column > 1 && row > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }

                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            else if (column <= 1 || row <= 1)
            {
                if (row > 1)
                {

                    if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                    {
                        if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                        {
                            return true;
                        }
                    }
                }

                if (column > 1)
                {
                    if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                    {
                        if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    private bool IsColumnOrRow()
    {
        int numberHorizontal = 0;
        int numberVertical = 0;

        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();

        if (firstPiece != null)
        {
            foreach (GameObject currentPiece in findMatches.currentMatches)
            {
                Dot dot = currentPiece.GetComponent<Dot>();

                if (dot.row == firstPiece.row)
                {
                    numberHorizontal++;
                }

                if (dot.column == firstPiece.column)
                {
                    numberVertical++;
                }
            }
        }

        return (numberHorizontal == 5 || numberVertical == 5);
    }

    private void CheckToMakeBombs()
    {
        if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
        {
            findMatches.CheckBombs();
        }

        if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (IsColumnOrRow())
            {
                //make a color bomb
                //is the current dot matched
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();

                            if (otherDot.isMatched)
                            {
                                if (!otherDot.isColorBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeColorBomb();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //make a adjacent bomb
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isAdjacentBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacntBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();

                            if (otherDot.isMatched)
                            {
                                if (!otherDot.isAdjacentBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeAdjacntBomb();
                                }
                            }
                        }
                    }
                }
            }

        }
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            //how many elements are in the matched pieces list from findmatches?
            if (findMatches.currentMatches.Count >= 4)
            {
                CheckToMakeBombs();
            }

            //does a tile need to break?
            if (breakableTiles[column, row] != null)
            {
                //if it does, give it 1 damage
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoints <= 0)
                {
                    breakableTiles[column, row] = null;
                }
            }

            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .3f);

            if (goalsManager != null)
            {
                goalsManager.CompareGoal(allDots[column, row].tag);
            }
            Destroy(allDots[column, row]);
            audioManager.PlayMatch();
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allDots[column, row] = null;
        }
    }

    public void DestoryMatches()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }
    private IEnumerator DecreaseRowCo2()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //if the current space isn't blank and is empty...
                if (!blankSpaces[i, j] && allDots[i, j] == null)
                {
                    //loop from the space above to top of the column
                    for (int k = j + 1; k < rows; k++)
                    {
                        //if a dot is found
                        if (allDots[i, k] != null)
                        {
                            //move the dot to this empty space
                            allDots[i, k].GetComponent<Dot>().row = j;

                            //set that spot to be null
                            allDots[i, k] = null;

                            //break out of the loop
                            break;
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoard());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (allDots[i, j] == null && !blankSpaces[i, j])
                {
                    Vector2 tempPos = new Vector2(i, j + refillOffset);
                    int dotToUse = Random.Range(0, dots.Length);

                    GameObject piece = Instantiate(dots[dotToUse], tempPos, Quaternion.identity);
                    allDots[i, j] = piece;

                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(refillDelay);

        int maxIterations = 0;

        while (MatchesOnBoard() && maxIterations < 100)
        {
            streakValue++;
            yield return new WaitForSeconds(2 * refillDelay);
            DestoryMatches();
            
        }

        maxIterations = 0;

        findMatches.currentMatches.Clear();
        currentDot = null;

        yield return new WaitForSeconds(refillDelay);

        if (IsDeadLocked())
        {
            StartCoroutine(ShuffleBoard());
        }

        yield return new WaitForSeconds(refillDelay);

        currentState = GameState.move;
        streakValue = 1;
    }

    private void SwitchPieces(int col, int row, Vector2 direction)
    {
        //take the first piece and save it to holder
        GameObject holder = allDots[col + (int)direction.x, row + (int)direction.y] as GameObject;

        //switching the first dot to the second position
        allDots[col + (int)direction.x, row + (int)direction.y] = allDots[col, row];

        //set the first dot to be the second dot
        allDots[col, row] = holder;
    }

    private bool CheckMatches()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (allDots[i, j] != null)
                {
                    //make sure that one and two to the right are to the right are in the board

                    if (i < columns - 2)
                    {
                        //check if the dot to the right and 2 to the right exist
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            //check if the dots are matches with the current dot
                            if (allDots[i + 1, j].tag == allDots[i, j].tag && allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }

                    if (j < rows - 2)
                    {
                        //check if the dots above and 2 above exist
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            //check if the dots are matches with the current dot
                            if (allDots[i, j + 1].tag == allDots[i, j].tag && allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }

                }
            }
        }

        return false;
    }

    public bool SwitchAndCheck(int col, int row, Vector2 direction)
    {
        SwitchPieces(col, row, direction);
        if (CheckMatches())
        {
            SwitchPieces(col, row, direction);
            return true;
        }

        SwitchPieces(col, row, direction);
        return false;
    }

    private bool IsDeadLocked()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < columns - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }

                    if (j < rows - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private IEnumerator ShuffleBoard()
    {
        List<GameObject> boardPieces = new List<GameObject>();

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (allDots[i, j] != null)
                {
                    boardPieces.Add(allDots[i, j]);
                }
            }
        }

        yield return new WaitForSeconds(refillDelay);
        //for every spot on the board
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //if this spot shouldn't be blank
                if (!blankSpaces[i, j])
                {
                    //pick a random number
                    int pieceToUse = Random.Range(0, boardPieces.Count);

                    if(boardPieces[pieceToUse] != null)
                    {
                        int maxIterations = 0;

                        while (MatchesAt(i, j, boardPieces[pieceToUse]) && maxIterations < 100)
                        {
                            pieceToUse = Random.Range(0, boardPieces.Count);
                            maxIterations++;
                        }

                        maxIterations = 0;

                        //make a container for the piece
                        Dot piece = boardPieces[pieceToUse].GetComponent<Dot>();

                        //set the piece to this location
                        piece.column = i;
                        piece.row = j;
                    }

                    //set the piece on the board
                    allDots[i, j] = boardPieces[pieceToUse];

                    //remove it from the list
                    boardPieces.Remove(boardPieces[pieceToUse]);
                    audioManager.PlayBoardShuffle();
                }

            }
        }
 

        //check if it's still deadlocked
        if(IsDeadLocked())
        {
            StartCoroutine(ShuffleBoard());
            
        }
    }
}
