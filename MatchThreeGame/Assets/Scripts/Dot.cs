using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    private int targetX;
    private int targetY;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    [Header("Input Attributes")]
    public float swipeAngle = 0;
    private float swipeResist = 1;

    private Vector2 tempPos;

    public FindMatches findMatches;
    public GameObject otherDot;

    private Board board;

    public bool isMatched = false;

    [Header("Power Up Attributes")]
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;
    public ColorBomb colorBombScript;
    public GameObject colorBomb;
    public GameObject adjacentBomb;
    public GameObject rowArrow;
    public GameObject columnArrow;

    [Header("Dot Attributes")]
    Animator anim;
    public SpriteRenderer sprite;

    [Header("Managers")]
    private HintManager hint;
    private AudioManager audio;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        board = FindObjectOfType<Board>();
        hint = FindObjectOfType<HintManager>();
        findMatches = FindObjectOfType<FindMatches>();
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;
        colorBombScript = GetComponent<ColorBomb>();
        anim = GetComponent<Animator>();
        audio = FindObjectOfType<AudioManager>();
    }

    /// <summary>
    /// This is for testing and debug only
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MakeColorBomb();
        }
    }

    private void Update()
    {
        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move towards the target
            tempPos = new Vector2(targetX * board.tileHeight, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);

            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();
        }
        else
        {
            //set the position
            tempPos = new Vector2(targetX * board.tileWidth, transform.position.y);
            transform.position = tempPos;
        }

        if (Mathf.Abs(targetY * board.tileHeight - transform.position.y) > .1)
        {
            //move towards the target
            tempPos = new Vector2(transform.position.x, targetY * board.tileHeight);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);

            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();

        }
        else
        {
            //set the position
            tempPos = new Vector2(transform.position.x, targetY * board.tileHeight);
            transform.position = tempPos;

        }

    }

    /// <summary>
    /// Checks to see if we are matching with a bomb or if the dot should move normally
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckMoveCo()
    {
        if (isColorBomb)
        {
            //this piece is a color bomb and the other piece is the color to destroy
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;
        }
        else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            //the other piece is the color bomb and this piece is the color to destroy
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }
        yield return new WaitForSeconds(.5f);

        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;

                row = previousRow;
                column = previousColumn;

                audio.PlaySwitchBack();

                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = Board.GameState.move;
            }
            else
            {
                board.DestoryMatches();

            }
        }
    }

    private void OnMouseDown()
    {
        if (board.currentState == Board.GameState.move)
        {
            //get the position when the player starts the swipe
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(hint != null)
        {
            hint.DestroyHint();
            hint.hintDelaySeconds = hint.hintDelay;
        }
       

    }

    private void OnMouseUp()
    {
        if (board.currentState == Board.GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    private void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = Board.GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePiece();
            board.currentDot = this;
        }
        else
        {
            board.currentState = Board.GameState.move;
        }
    }

    void MovePieces(Vector2 direction)
    {

        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousColumn = column;
        previousRow = row;

        if (otherDot != null)
        {
            otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
            otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            audio.PlaySwitch();
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentState = Board.GameState.move;
        }
    }

    private void MovePiece()
    {
        if (swipeAngle > -45f && swipeAngle <= 45f && column < board.columns - 1)
        {
            MovePieces(Vector2.right);
            
        }
        else if (swipeAngle > 45f && swipeAngle <= 135f && row < board.rows - 1)
        {
            MovePieces(Vector2.up);
            
        }
        else if ((swipeAngle > 135f || swipeAngle <= -135f) && column > 0)
        {
            MovePieces(Vector2.left);
        }
        else if (swipeAngle < -45f && swipeAngle >= -135f && row > 0)
        {
            MovePieces(Vector2.down);
        }
        board.currentState = Board.GameState.move;


    }

    public void MakeRowBomb()
    {
        isRowBomb = true;
        sprite.sprite = rowArrow.GetComponent<SpriteRenderer>().sprite;
        audio.PlayCreateBomb();
    }

    public void MakeColumnBomb()
    {
        isColumnBomb = true;
        sprite.sprite = columnArrow.GetComponent<SpriteRenderer>().sprite;
        audio.PlayCreateBomb();
    }

    public void MakeAdjacntBomb()
    {
        isAdjacentBomb = true;
        sprite.sprite = adjacentBomb.GetComponent<SpriteRenderer>().sprite;
        audio.PlayCreateBomb();
    }

    public void MakeColorBomb()
    {
        isColorBomb = true;
        colorBombScript.StartColorChange();
        this.gameObject.tag = "ColorBomb";
        audio.PlayCreateBomb();
    }
}
