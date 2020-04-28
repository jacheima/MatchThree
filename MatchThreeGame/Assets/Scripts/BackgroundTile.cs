using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints;
    private SpriteRenderer sprite;

    private void Start()
    {
        //Get the sprite renderer of the background tile
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //if this tile has less than or equal to 0 hit points
        if(hitPoints <= 0)
        {
            //destroy the gameobject
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// Applies damage to the background tile
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        //subtract the damage from the hitpoints
        hitPoints -= damage;

        //adjust how the sprite looks
        MakeLighter();
    }

    /// <summary>
    /// This method changes the alpha value of the tile when the tile takes damage
    /// </summary>
    void MakeLighter()
    {
        //get the color of this sprite
        Color color = sprite.color;

        //calculate the new alpha
        float newAlpha = color.a * .5f;

        //set the sprite color to the new color
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
