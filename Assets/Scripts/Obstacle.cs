using UnityEngine;

public class Obstacle : InteractiveObject
{
    [SerializeField] private AudioClip HideOnSound;
    [SerializeField] private AudioClip HideOffSound;
    private AudioSource audioSource;

    private bool isHide = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        base.Update();
    }

    public override void OnInteractive()
    {
        Debug.Log("Starting Obstacle OnInteractive");

        OnOffHide();
    }

    void OnOffHide()
    {
        if(!isHide)
        {
            Debug.Log("캐비닛에 숨었다!");
            isHide = true;

            PlayHideOnGeneratorSound();
            Player.OnHide();
        }
        else
        {
            Debug.Log("캐비닛에 나왔다!");
            isHide = false;

            PlayHideffGeneratorSound();
            Player.OffHide();
        }
    }

    public void PlayHideOnGeneratorSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = HideOnSound;
            audioSource.Play();
        }

    }

    public void PlayHideffGeneratorSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = HideOffSound;
            audioSource.Play();
        }

    }
}
