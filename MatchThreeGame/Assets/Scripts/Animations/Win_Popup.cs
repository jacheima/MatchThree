using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_Popup : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    public void WinIsOpening()
    {
        anim.SetBool("isOpening", true);
        anim.SetBool("isClosed", false);
    }
    public void WinIsOpen()
    {
        anim.SetBool("isOpening", false);
        anim.SetBool("isOpen", true);
    }

    public void WinIsClosed()
    {
        anim.SetBool("isOpen", false);
    }

    public void SetToClosed()
    {
        anim.SetBool("isClosed", true);
    }
}
