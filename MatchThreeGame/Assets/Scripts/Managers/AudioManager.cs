using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sound FX")]
    public AudioSource useBomb;
    public AudioSource createBomb;
    public AudioSource match;
    public AudioSource switchPiece;
    public AudioSource switchBack;
    public AudioSource boardShuffle;
    public AudioSource uiButton;
    //TODO: Add functionality for these audio sources once I get
    //the functionality in
    public AudioSource star;
    
    public AudioSource bonus;

    [Header("Music")]
    public AudioSource levelOne;
    public AudioSource levelTwo;
    public AudioSource levelThree;
    public AudioSource mainMenu;
    public AudioSource map;
    public AudioSource store;

    private void Start()
    {
        
    }

    public void PlayUseBomb()
    {
        useBomb.Play();
    }

    public void PlayCreateBomb()
    {
        createBomb.Play();
    }

    public void PlayMatch()
    {
        match.Play();
    }

    public void PlaySwitch()
    {
        switchPiece.Play();
    }

    public void PlaySwitchBack()
    {
        switchBack.Play();
    }

    public void PlayBoardShuffle()
    {
        boardShuffle.Play();
    }

    public void PlayButtonPress()
    {
        uiButton.Play();
    }
}
