using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose_Popup : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void LoseIsOpening()
    {
        anim.SetBool("isClosed", false);
        anim.SetBool("isOpening", true);   
    }

    public void LoseIsOpen()
    {
        anim.SetBool("isOpening", false);
        anim.SetBool("isOpen", true);
    }

    public void LoseIsClosing()
    {
        
        anim.SetBool("isOpen", false);
    }

    public void LoseIsClosed()
    {
        anim.SetBool("isClosed", true);
    }
}
