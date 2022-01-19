using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] static AudioClip shoot;
    [SerializeField] static AudioClip cut;
    [SerializeField] static AudioClip collect;
    [SerializeField] static AudioClip bounce;
    [SerializeField] static AudioClip mushroom;
    [SerializeField] static AudioClip gameOverSFX;

    [SerializeField] static AudioClip menuMusic;
    [SerializeField] static AudioClip gameOverMusic;
    [SerializeField] static AudioClip gameplayMusic;

    static AudioSource audioSource;

    private static SoundManager sm;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if(sm == null)
        {
            sm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverSFX = Resources.Load<AudioClip>("GameOverSFX");
        gameOverMusic = Resources.Load<AudioClip>("GameOverMusic");

        gameplayMusic = Resources.Load<AudioClip>("Prominade");
        menuMusic = Resources.Load<AudioClip>("MenuMusic");

        shoot = Resources.Load<AudioClip>("shoot");
        cut = Resources.Load<AudioClip>("cut");
        collect = Resources.Load<AudioClip>("collect");
        bounce = Resources.Load<AudioClip>("bounce");
        mushroom = Resources.Load<AudioClip>("mushroom");

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shoot);
    }

    public void PlayCutSound()
    {
        audioSource.PlayOneShot(cut);
    }

    public void PlayCollectSound()
    {
        audioSource.PlayOneShot(collect);
    }

    public void PlayBounceSound()
    {
        audioSource.PlayOneShot(bounce);
    }

    public void PlayMushroomSound()
    {
        audioSource.PlayOneShot(mushroom);
    }

    public void PlayGameOverSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSFX);
    }

    public void PlayGameOverMusic()
    {
        audioSource.PlayOneShot(gameOverMusic);
    }

    public void PlayGamePlayMusic()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(gameplayMusic);
        audioSource.loop = true;
    }
}
