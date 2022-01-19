using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameManager gm;
    Spider spider;
    SoundManager sm;

    private bool isGameOver = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spider = FindObjectOfType<Spider>();
        sm = FindObjectOfType<SoundManager>();
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            isGameOver = false;

            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        Rigidbody2D rb = spider.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        spider.GetComponent<Collider2D>().enabled = false;

        sm.PlayGameOverSound();
        sm.PlayGameOverMusic();

        Camera.main.transform.SetParent(null, true);
        isGameOver = true;
        spider.SetGameOver();
    }

    public void NextLevel()
    {

    }
}
