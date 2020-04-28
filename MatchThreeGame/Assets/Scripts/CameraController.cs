using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Board board;
    private float cameraOffset = -10f;
    public float aspectRatio = 0.625f;
    public float padding = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //set the board 
        board = FindObjectOfType<Board>();

        //if the board reference is not empty
        if(board != null)
        {
            //reposition the camera
           RepositionCamera((board.columns -1) * board.tileWidth, (board.rows -1) * board.tileHeight);
        }
    }

    /// <summary>
    /// This method repositions the camera so that the board will always fit in the camera
    /// no matter how big or small the board is
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void RepositionCamera(float x, float y)
    {
        //set the position of the camera based on the hight and width of the baord
        Vector3 tempPosition = new Vector3(x / 2, (y / 2) + 1f, cameraOffset);
        transform.position = tempPosition;
        
        //set the width of the camera
        if(board.columns >= board.rows)
        {
            Camera.main.orthographicSize = ((board.columns / 2) + padding) / aspectRatio;
        }
        //set the height of the camera
        else
        {
            Camera.main.orthographicSize = ((board.rows / 2) + padding);
        }
    }
}
