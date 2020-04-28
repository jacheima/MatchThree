using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBomb : MonoBehaviour
{
    private SpriteRenderer spriteRend;

    [SerializeField] private Sprite[] colors;

    private Dot dot;
    public int colorBomb = 0;

    private float changeStartTime;
    [SerializeField] private float colorChangeDelay;

    bool startColorChange;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        dot = GetComponent<Dot>();
        startColorChange = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartColorChange()
    {
        InvokeRepeating("ChangeColor", 0f, .5f);
    }

    void ChangeColor()
    {
        Debug.Log(colors.Length);
        if(colorBomb >= 0 && colorBomb < colors.Length)
        {
            colorBomb++;
        }
        else
        {
            colorBomb = 0;
        }

        spriteRend.sprite = colors[colorBomb];
    }

}
