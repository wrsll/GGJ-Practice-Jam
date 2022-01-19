using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEffect : MonoBehaviour
{
    Spider spider;
    GameManager gm;

    [SerializeField] bool isBouncy = false;
    [SerializeField] bool recoversWeb = false;
    [SerializeField] bool limitWebRecovery = false;

    [SerializeField] bool isLevelGoal = false;

    [SerializeField] int webRecoveryAmount = 0;
    [SerializeField] int maxRecoveryAmount = 5;


    private void Start()
    {
        spider = FindObjectOfType<Spider>();
        gm = FindObjectOfType<GameManager>();
    }

    public void UseEffect()
    {
        if(isLevelGoal)
        {
            gm.NextLevel();
        }

        if (isBouncy && !spider.IsShotConnected())
        {
            Rigidbody2D rb = spider.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector3.up * 1000f);
            spider.SetInvincible();
        }

        if (recoversWeb)
        {
            if (limitWebRecovery)
            {
                int shots = spider.GetShotsLeft();

                if (shots < webRecoveryAmount)
                {
                    spider.SetShotsLeft(maxRecoveryAmount);
                }
            }
            else
            {
                spider.AddNewShot(webRecoveryAmount);
            }
        }
    }
}
