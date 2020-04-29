using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_Popup : MonoBehaviour
{
    private Animator anim;
    public Board board;

    void Start()
    {
        anim = GetComponent<Animator>();
        board = FindObjectOfType<Board>();
    }
    public void OpenMissionPopup()
    {
        anim.SetBool("openMissions", true);
    }

    public void MissionIsOpen()
    {
        anim.SetBool("openMissions", false);
        anim.SetBool("isOpen", true);
    }

    public void MissionIsClosed()
    {
        anim.SetBool("isOpen", false);
    }

}
