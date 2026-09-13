using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] AudioClip menuClip;
    [SerializeField] AudioClip level1Clip;
    [SerializeField] AudioClip level2Clip;
    [SerializeField] AudioClip level3Clip;
    [SerializeField] AudioClip level4Clip;
    [SerializeField] AudioClip BossFightClip;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip clip = sceneName switch
        {
            "Menu" => menuClip,
            "Level 1" => level1Clip,
            "Level 2" => level2Clip,
            "Level 3" => level3Clip,
            "Level 4" => level4Clip,
            _ => null
        };

        if (clip == null || audioSource == null)
        {
            return;
        }

        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
