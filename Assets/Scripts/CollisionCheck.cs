using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    GameManager gm;

    private bool isDangerous = true;

    private MushroomEffect effect = null;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        isDangerous = gameObject.GetComponentInParent<Platform>().IsDangerous();

        if(!isDangerous)
        {
            effect = gameObject.GetComponentInParent<MushroomEffect>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            if(isDangerous)
            {
                gm.GameOver();
            }
            else
            {
                effect.UseEffect();
            }
        }
    }
}
