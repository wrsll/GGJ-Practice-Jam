using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundryCollider : MonoBehaviour
{
    private Enemy enemy;
    private bool doesEnemyPatrol = false;

    private void Start()
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
        doesEnemyPatrol = enemy.WalksOnPlatform();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!doesEnemyPatrol)
        {
            if(enemy.IgnoresPlatforms())
            {
                if (other.gameObject.GetComponent<Boundry>() || other.gameObject.GetComponent<Enemy>())
                {
                    enemy.FlipDirection();
                }
            }
            else
            {
                if (other.gameObject.GetComponent<Boundry>() || other.gameObject.GetComponent<Platform>() || other.gameObject.GetComponent<Enemy>())
                {
                    enemy.FlipDirection();
                }
            }
        }
        else
        {
            if (other.gameObject.GetComponent<Boundry>() || other.gameObject.GetComponent<Enemy>())
            {
                enemy.FlipDirection();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Platform>())
        {
            enemy.FlipDirection();
        }
    }
}
