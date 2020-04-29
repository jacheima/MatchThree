using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalInfo : MonoBehaviour
{
    public Image goalImage;
    public Sprite goalSprite;
    public TextMeshProUGUI goalText;
    public string goalString;

    void Start()
    {
        Setup();
    }
    void Setup()
    {
        goalImage.sprite = goalSprite;
        goalText.text = goalString;
    }
}
