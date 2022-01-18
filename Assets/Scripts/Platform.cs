using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Spider spider;
    [SerializeField] GameObject CollisionLine;
    [SerializeField] GameObject WebShotLine;

    [SerializeField] bool isDangerous = true;
    private bool isGrappled = false;

    private void Start()
    {
        spider = FindObjectOfType<Spider>();
    }

    private void Update()
    {
        if (!isDangerous)
        {
            if (spider.GetPlayerPos().y <= gameObject.transform.position.y)
            {
                CollisionLine.SetActive(true);
            }
        }
        else
        {
            if (!isGrappled && !spider.IsShooting())
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
