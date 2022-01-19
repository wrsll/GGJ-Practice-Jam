using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Spider spider;
    GameManager gm;

    private Collider2D collider;

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] bool isCollectable = false;
    [SerializeField] int shotValue = 1;

    [SerializeField] bool shouldMoveStraight = true;
    [SerializeField] float sinMagnitude = 1f;
    [SerializeField] float waveFrequency = 5f;

    [SerializeField] bool canCutWeb = false;
    [SerializeField] bool walksOnPlatform = false;
    [SerializeField] bool ignoresPlatforms = false;

    Vector3 enemyPos;

    private void Start()
    {
        spider = FindObjectOfType<Spider>();
        gm = FindObjectOfType<GameManager>();
        collider = gameObject.GetComponent<Collider2D>();

        moveSpeed *= -1;

        enemyPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldMoveStraight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            enemyPos += transform.right * Time.deltaTime * moveSpeed;
            transform.position = enemyPos + transform.up * Mathf.Sin(Time.time * waveFrequency) * sinMagnitude;
        }
    }

    public void FlipDirection()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollectable)
        {
            if (other.tag.Equals("Web"))
            {
                if(canCutWeb)
                {
                    spider.CutWeb();
                }
                else
                {
                    Physics2D.IgnoreCollision(other, collider);
                }
            }

            if (other.tag.Equals("Player"))
            {
                gm.GameOver();
            }
        }
        else
        {
            if (other.tag.Equals("Player"))
            {
                spider.AddNewShot(shotValue);
                Destroy(gameObject);
            }

            if (other.tag.Equals("Web"))
            {
                Physics2D.IgnoreCollision(other, collider);
            }
        }
    }

    public bool CanCutWeb()
    {
        return canCutWeb;
    }

    public bool WalksOnPlatform()
    {
        return walksOnPlatform;
    }

    public bool IgnoresPlatforms()
    {
        return ignoresPlatforms;
    }
}
