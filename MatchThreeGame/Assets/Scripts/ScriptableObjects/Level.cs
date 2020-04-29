using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Board Dimentions")]
    public int columns; //width of board
    public int rows; //height of board

    [Header("Starting Tiles")]
    public TileType[] boardlayout;

    [Header("Available Dots")]
    public GameObject[] dots;

    [Header("ScoreGoals")]
    public int[] scoreGoals;

    [Header("End Game Requirements")]
    public EndGameRequirements endGameRequirements;
    public BlankGoal[] levelGoals;
    public BlankGoal[] missionGoals;
    

}
