using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutoral_Popup : MonoBehaviour
{
    public Board board;

    void Start()
    {
        board = FindObjectOfType<Board>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Destroy(this.gameObject);
            board.tutorial.NextStep();
        }
    }

}
