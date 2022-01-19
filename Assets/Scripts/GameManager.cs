using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Spider spider;

    private void Start()
    {
        spider = FindObjectOfType<Spider>();
    }

    public void GameOver()
    {
        Rigidbody2D rb = spider.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        spider.GetComponent<Collider2D>().enabled = false;

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void NextLevel()
    {

    }
}
