using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionCheck : MonoBehaviour
{
    private bool isDangerous = true;

    private void Start()
    {
        isDangerous = gameObject.GetComponentInParent<Platform>().IsDangerous();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            if(isDangerous)
            {
                Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;

                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);

                //rb.gravityScale = 0;
            }
            else
            {
                Debug.Log("yipee!");
                //Recover web?
            }
        }
    }
}
