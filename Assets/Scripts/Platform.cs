using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Spider spider;
    GameManager gm;
    [SerializeField] GameObject CollisionLine;
    [SerializeField] GameObject WebShotLine;

    [SerializeField] bool isDangerous = true;
    private bool isGrappled = false;

    private void Start()
    {
        spider = FindObjectOfType<Spider>();
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!isDangerous)
        {
            if (spider.GetPlayerPosY() >= gameObject.transform.position.y - 2f)
            {
                CollisionLine.SetActive(true);
            }
            else
            {
                CollisionLine.SetActive(false);
            }
        }
        else
        {
            if (!isGrappled && !spider.IsShooting() && !spider.IsInvincible())
            {
                if (spider.GetPlayerPos().y <= gameObject.transform.position.y)
                {
                    EnableWebShotLine();
                }
                else
                {
                    EnableCollisions();
                }
            }
            else if (CollisionLine.activeInHierarchy)
            {
                CollisionLine.SetActive(false);
            }
        }

    }

    private void EnableCollisions()
    {
        CollisionLine.SetActive(true);
        //WebShotLine.SetActive(false);
    }

    private void EnableWebShotLine()
    {
        CollisionLine.SetActive(false);
        WebShotLine.SetActive(true);
    }

    public void SetIsGrappled(bool _isGrappled)
    {
        isGrappled = _isGrappled;
    }

    public bool IsDangerous()
    {
        return isDangerous;
    }
}
