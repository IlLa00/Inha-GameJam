using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundCenter : MonoBehaviour
{
    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    [Header("씬별 배경음악 설정")]
    [SerializeField] private SceneBGM[] sceneBGMs;
    [SerializeField] private float volume = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        DontDestroyOnLoad(gameObject);  
        if (FindObjectsOfType<SoundCenter>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        PlaySceneBGM();

        SceneManager.sceneLoaded += OnSceneChanged;
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        PlaySceneBGM();
    }

    void PlaySceneBGM()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        AudioClip bgmToPlay = GetBGMForScene(currentSceneName);

        if (bgmToPlay != null)
        {
            audioSource.clip = bgmToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"씬 '{currentSceneName}'에 대한 배경음악이 설정되지 않았습니다!");
        }
    }

    AudioClip GetBGMForScene(string sceneName)
    {
        foreach (SceneBGM sceneBGM in sceneBGMs)
        {
            if (sceneBGM.sceneName == sceneName)
            {
                return sceneBGM.bgmClip;
            }
        }
        return null;
    }

    void Update()
    {

    }

    public void PlayBGM()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }
}